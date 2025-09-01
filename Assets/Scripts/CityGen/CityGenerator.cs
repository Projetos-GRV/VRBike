using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Tooltip("Largura e \"Altura\" (comprimento) deve levar em considera��o o tamanho de cada tile (as estradas e concreto, por exemplo, possuem tamanho 20x20).")]
    public int width = 50;
    [Tooltip("Largura e \"Altura\" (comprimento) deve levar em considera��o o tamanho de cada tile (as estradas e concreto, por exemplo, possuem tamanho 20x20).")]
    public int height = 50;
    public bool infinite = false;

    public int chunkRadius = 3;
    public int blockSize = 20;
    private int chunkSize = 2;
    private int stride;
    private long chunks = 0;

    public Transform player;

    public GameObject laneRegular;
    public GameObject laneBusStop;
    public GameObject laneIntersection;
    public GameObject laneTIntersection;
    public GameObject laneCorner;
    public GameObject floorConcrete;

    public GameObject[] commercialBuildings;
    public GameObject[] residentialBuildings;

    // C# <3 >>> Java </3
    private GameObject[,] cityMatrix;
    public Vector2Int[] residentialZones;
    private (int X1, int Y1, int X2, int Y2)[] commercialZone;


    private bool generatingCity = true;
    private Transform cityParentTransform;
    private Dictionary<Vector3, GameObject> loadedCunks = new Dictionary<Vector3, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        stride = chunkSize * blockSize + blockSize;
        cityMatrix = new GameObject[width, height];
        cityParentTransform = new GameObject("CityParent").transform;
        // evitar congelamentos caso largura X altura seja mt grande
        if (infinite)
        {
            // StartCoroutine(GenInfinite());
            GenInfinite();
        }
        else
        {
            StartCoroutine(GenCity());
        }
    }

    // Update is called once per frame

    void Update()
    {
        // caso usada geracao um pouco mais procedural, usar transform do jogador para atualizar chunks
        Vector3 playerPos = player.position;
        Vector2Int pChunk = new Vector2Int(0, 0)
        {
            x = Mathf.RoundToInt(playerPos.x / stride),
            y = Mathf.RoundToInt(playerPos.z / stride)
        };

        if (!generatingCity)
        {
            HashSet<Vector3> needed = new HashSet<Vector3>();
            // geracao de chunks proximas ao jogador
            for (int dx = -chunkRadius; dx <= chunkRadius; dx++)
            {
                for (int dy = -chunkRadius; dy <= chunkRadius; dy++)
                {
                    Vector3 cc = new Vector3(pChunk.x + dx, 0, pChunk.y + dy);
                    needed.Add(cc);
                    if (!loadedCunks.ContainsKey(cc))
                    {
                        GameObject chunk = GenerateChunk(string.Format("Chunk{0}", this.chunks++), cc);
                        chunk.transform.position = new Vector3(cc.x * stride, 0, cc.z * stride);
                        loadedCunks.Add(cc, chunk);
                    }
                    else if (!loadedCunks[cc].activeSelf)
                    {
                        loadedCunks[cc].SetActive(true);
                    }
                    // Instantiate(chunkParent, new Vector3((cc + 3) * 20, 0, 0), Quaternion.identity);
                }
            }

            // List<Vector3> toRemove = new List<Vector3>();

            foreach (var pair in this.loadedCunks)
            {
                if (!needed.Contains(pair.Key))
                {
                    pair.Value.SetActive(false);
                }
            }
        }

    }

    // um bloco central. os outros sao gerados no etorno... ou pelo menos deveriam... por enquanto nada
    async void GenInfinite()
    {
        // chunk do meio
        Vector3 playerPos = player.position;
        Vector2Int pChunk = new Vector2Int(0, 0)
        {
            x = Mathf.RoundToInt(playerPos.x / stride),
            y = Mathf.RoundToInt(playerPos.z / stride)
        };
        for (int dx = -chunkRadius; dx <= chunkRadius; dx++) // quantidade de chunks para gerar. posicionar depois
        {
            for (int dy = -chunkRadius; dy <= chunkRadius; dy++)
            {
                Vector3 cc = new Vector3(pChunk.x + dx, 0, pChunk.y + dy);
                GameObject chunk = GenerateChunk(string.Format("Chunk{0}", chunks++), cc);
                chunk.transform.position = new Vector3(cc.x * stride, 0, cc.z * stride);
                loadedCunks.Add(cc, chunk);
                // yield return null;
                await Task.Yield();
            }
        }
        generatingCity = false;
    }

    IEnumerator GenCity()
    {
        int rotationsCounter = 0;
        int[] rotations = { 90, 0, 180, 270 };
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Quaternion rotationQ = Quaternion.identity;
                GameObject prefab = laneRegular;
                if ((i == 0 || i == height - 1) && (j == 0 || j == width - 1)) // cantos
                {
                    rotationQ = Quaternion.AngleAxis(rotations[rotationsCounter++], Vector3.up);
                    prefab = laneCorner;
                }
                else if (i % (chunkSize + 1) == 0 && j % (chunkSize + 1) == 0) // interseccoes
                {
                    if (i != 0 && i != height - 1 && j != 0 && j != width - 1)
                    {
                        prefab = laneIntersection;
                    }
                    else
                    {
                        float angle = 0;
                        if (j == 0)
                            angle = 180;
                        else if (j == width - 1)
                            angle = 0;

                        if (i == 0)
                            angle = 90;
                        else if (i == height - 1)
                            angle = 270;

                        rotationQ = Quaternion.AngleAxis(angle, Vector3.up);
                        prefab = laneTIntersection;
                    }
                }
                else if (i % (chunkSize + 1) != 0 && j % (chunkSize + 1) != 0)
                {
                    // considerar zona
                    //bool isCommercial = CheckIfCommercialZone(j, i) || true;
                    bool isResidential = CheckIfResidentialZone(j, i);
                    InstantiateBuilding(j, i, isResidential, cityParentTransform);
                    prefab = floorConcrete;
                }
                else // vias normais
                {
                    prefab = laneRegular;
                    if (j % (chunkSize + 1) != 0)
                    {
                        rotationQ = Quaternion.AngleAxis(90, Vector3.up);
                    }
                }
                cityMatrix[j, i] = Instantiate(prefab, new Vector3(j * 20, 0, i * 20), rotationQ, cityParentTransform);
                yield return null;
            }
        }
    }

    // Chunks geradas por essa funcao tem sua posicao definida como (0,0,0).
    // A posicao desta deve ser definida por fora
    // intendedGridCoords serve apenas para verificar se o chunk se encontra em uma 
    // zona residencial.
    private GameObject GenerateChunk(string name, Vector3 intendedGridCoords)
    {
        GameObject chunkParent = new GameObject(name);
        chunkParent.transform.parent = cityParentTransform;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Quaternion rotationQ = Quaternion.identity;
                GameObject prefab = laneRegular;
                if (i % (chunkSize + 1) != 0 && j % (chunkSize + 1) != 0) // bloco. onde ficam as construcoes tais como casas, lojas e predios.
                {
                    // considerar zona
                    //bool isCommercial = CheckIfCommercialZone(j, i) || true;
                    bool isResidential = CheckIfResidentialZone((int)intendedGridCoords.x, (int)intendedGridCoords.z);
                    InstantiateBuilding(j, i, isResidential, chunkParent.transform);
                    prefab = floorConcrete;
                    Instantiate(prefab, new Vector3(j * 20, 0, i * 20), rotationQ, chunkParent.transform);
                }
            }
        }
        return chunkParent;
    }

    // TODO - verificar se ja foi posta no bloco, talvez? evitar haverem multiplas construcoes num bloco?
    private GameObject InstantiateBuilding(float x, float y, bool isResidential, Transform parentObject)
    {
        GameObject[] objects = isResidential ? residentialBuildings : commercialBuildings;
        int idx = Random.Range(0, objects.Length - 1);
        float angle = 90;
        if (((int)x - 1) % 3 == 0)
        {
            angle = 270;
        }
        // deslocar o posto de gasolina soh um pouquinho para nao ficar em cima da calcada
        if ("Building_Gas Station".IndexOf(objects[idx].name) >= 0)
        {
            if (angle == 90)
                x = ((x * 20) - 2.5f) / 20;
            else
                x = ((x * 20) + 2.5f) / 20;
        }
        Instantiate(objects[idx], new Vector3(x * 20, 0, y * 20), Quaternion.AngleAxis(angle, Vector3.up), parentObject);
        return objects[idx];
    }

    private bool CheckIfResidentialZone(int x, int y)
    {
        if (this.residentialZones == null)
        {
            return false;
        }
        for (int i = 0; i < this.residentialZones.Length; i += 2)
        {
            Vector2Int p1 = this.residentialZones[i];
            Vector2Int p2 = this.residentialZones[i + 1];
            if (x >= p1.x && x <= p2.x && y >= p1.y && y <= p2.y)
            {
                return true;
            }
        }
        return false;
    }
}
