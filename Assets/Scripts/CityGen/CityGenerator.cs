using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

// q bagunça.....
public class CityGenerator : MonoBehaviour
{
    public int chunkRadius = 3;
    public int blockSize = 20;
    private int chunkSize = 2;
    private int stride;
    private long chunks = 0;

    public Transform player;

    public GameObject laneRegular;
    public GameObject laneBusStop;
    public GameObject laneIntersection;
    public GameObject floorConcrete;
    public GameObject floorGrass;

    public GameObject[] commercialBuildings;
    public GameObject[] residentialBuildings;
    public GameObject[] sidewalkObjects;
    public GameObject[] vehicles;

    public Vector2Int[] residentialZones;
    private (int X1, int Y1, int X2, int Y2)[] commercialZone;


    private bool generatingCity = true;
    private GameObject surroundingLanes;
    private Transform cityParentTransform;
    private Dictionary<Vector3, GameObject> loadedCunks = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector2Int, GameObject> streetsRelativeToPlayer = new Dictionary<Vector2Int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        stride = chunkSize * blockSize + blockSize;
        cityParentTransform = new GameObject("CityParent").transform;

        // cria as vias em um canto pra instanciar numa unica chamada de funcao depois em GenerateChunk
        surroundingLanes = new GameObject("Lanes");
        surroundingLanes.transform.parent = cityParentTransform;

        Instantiate(laneRegular, new Vector3(0, 0, -1 * (stride - blockSize)), Quaternion.identity, surroundingLanes.transform);
        Instantiate(laneRegular, new Vector3(0, 0, -1 * (stride - (blockSize * 2))), Quaternion.identity, surroundingLanes.transform);
        // postes de luz (nao emitem luz alguma, mas existem)
        Instantiate(sidewalkObjects[1], new Vector3(52.0f, 0, -1 * (stride - blockSize) - 11.5f), Quaternion.AngleAxis(90, Vector3.up), surroundingLanes.transform);
        Instantiate(sidewalkObjects[1], new Vector3(52.0f, 0, -1 * (stride - blockSize) - 11.5f), Quaternion.identity, surroundingLanes.transform);
        Instantiate(sidewalkObjects[1], new Vector3(8.5f, 0, -1 * (stride - (blockSize * 2)) + 11.5f), Quaternion.AngleAxis(-180, Vector3.up), surroundingLanes.transform);
        Instantiate(sidewalkObjects[1], new Vector3(8.5f, 0, -1 * (stride - (blockSize * 2)) + 11.5f), Quaternion.AngleAxis(-90, Vector3.up), surroundingLanes.transform);

        Instantiate(laneRegular, new Vector3((stride - blockSize), 0, 0), Quaternion.AngleAxis(90, Vector3.up), surroundingLanes.transform);
        Instantiate(laneRegular, new Vector3((stride - (blockSize * 2)), 0, 0), Quaternion.AngleAxis(90, Vector3.up), surroundingLanes.transform);
        // arvrinha :3
        Instantiate(sidewalkObjects[0], new Vector3((stride - blockSize) + 11.5f, 0, -8.5f), Quaternion.identity, surroundingLanes.transform);

        Instantiate(laneIntersection, new Vector3(0, 0, 0), Quaternion.identity, surroundingLanes.transform);

        surroundingLanes.transform.position = new Vector3(0, -100000, 0);

        // evitar congelamentos caso largura X altura seja mt grande
        GenInfinite();
    }

    // Update is called once per frame

    void Update()
    {
        // caso usada geracao um pouco mais procedural, usar transform do jogador para atualizar chunks
        Vector3 playerPos = player.position;
        // chunk onde o jogador se encontra... aproximadamente
        Vector2Int pChunk = new Vector2Int(0, 0)
        {
            x = Mathf.RoundToInt(playerPos.x / stride),
            y = Mathf.RoundToInt(playerPos.z / stride)
        };

        if (!generatingCity)
        {
            // guarda as chunks proximas ao jogador
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

            foreach (var pair in this.loadedCunks)
            {
                if (!needed.Contains(pair.Key))
                {
                    //Desativa em vez de remover. Determinismo
                    pair.Value.SetActive(false);
                }
            }
        }
    }

    async void GenInfinite()
    {
        Vector3 playerPos = player.position;
        Vector2Int pChunk = new Vector2Int(0, 0)
        {
            x = Mathf.RoundToInt(playerPos.x / stride),
            y = Mathf.RoundToInt(playerPos.z / stride)
        };

        for (int dx = -chunkRadius; dx <= chunkRadius; dx++)
        {
            for (int dy = -chunkRadius; dy <= chunkRadius; dy++)
            {
                Vector3 cc = new Vector3(pChunk.x + dx, 0, pChunk.y + dy);
                GameObject chunk = GenerateChunk(string.Format("Chunk{0}", chunks++), cc);
                chunk.transform.position = new Vector3(cc.x * stride, 0, cc.z * stride);
                loadedCunks.Add(cc, chunk);
                await Task.Yield();
            }
        }
        generatingCity = false;
    }

    // Chunks geradas por essa funcao tem sua posicao definida como (0,0,0).
    // A posicao desta deve ser definida por fora
    // intendedGridCoords serve apenas para verificar se o chunk se encontra em uma 
    // zona residencial.
    private GameObject GenerateChunk(string name, Vector3 intendedGridCoords)
    {
        GameObject chunkParent = new GameObject(name);
        chunkParent.transform.parent = cityParentTransform;

        Instantiate(surroundingLanes, new Vector3(0, 0, stride), Quaternion.identity, chunkParent.transform);
        HashSet<string> addedBuildings = new HashSet<string>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (i % (chunkSize + 1) != 0 && j % (chunkSize + 1) != 0) // bloco. onde ficam as construcoes tais como casas, lojas e outros predios.
                {
                    // considerar zona
                    //bool isCommercial = CheckIfCommercialZone(j, i) || true;
                    bool isResidential = CheckIfResidentialZone((int)intendedGridCoords.x, (int)intendedGridCoords.z);
                    GameObject building = InstantiateBuilding(j, i, isResidential, chunkParent.transform, addedBuildings);
                    GameObject floor = (GameObject) Instantiate(isResidential ? this.floorGrass : this.floorConcrete, new Vector3(j * 20, 0, i * 20), Quaternion.identity, chunkParent.transform);

                    if (building.name.Contains("Gas"))
                    {
                        Instantiate(vehicles[0], floor.transform.position + new Vector3(building.transform.eulerAngles.y == 90 ? 6.5f : -6.5f, 0, 0), Quaternion.identity, floor.transform);
                    }
                }
            }
        }
        // veiculos B)
        // vias "verticais"
        float carSpawnChance = 0.5f;
        if (Random.value < carSpawnChance)
        {
            Instantiate(vehicles[Random.Range(0, vehicles.Length - 1)], new Vector3(4.6f, 0, 1 * (stride - (blockSize * 2))), Quaternion.identity, chunkParent.transform);
        }
        if (Random.value < carSpawnChance)
        {
            Instantiate(vehicles[Random.Range(0, vehicles.Length - 1)], new Vector3(-4.6f, 0, 1 * (stride - blockSize)), Quaternion.AngleAxis(180, Vector3.up), chunkParent.transform);
        }
        // vias "horizontais"
        if (Random.value < carSpawnChance)
        {
            Instantiate(vehicles[Random.Range(0, vehicles.Length - 1)], new Vector3(1 * (stride - blockSize), 0, 4.6f + stride), Quaternion.AngleAxis(-90, Vector3.up), chunkParent.transform);
        }
        if (Random.value < carSpawnChance)
        {
            Instantiate(vehicles[Random.Range(0, vehicles.Length - 1)], new Vector3(-1 * (stride - blockSize) + stride, 0, -4.6f + stride), Quaternion.AngleAxis(90, Vector3.up), chunkParent.transform);
        }
        return chunkParent;
    }

    // TODO - verificar se ja foi posta no bloco, talvez? evitar haverem multiplas construcoes num bloco?
    private GameObject InstantiateBuilding(float x, float y, bool isResidential, Transform parentObject, HashSet<string> addedBuildings)
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

        if (!isResidential && !Regex.IsMatch(objects[idx].name, "Building_Residential.*"))
        {
            while (addedBuildings.Contains(objects[idx].name))
            {
                idx = Random.Range(0, objects.Length - 1);
            }
        }
        addedBuildings.Add(objects[idx].name);
        return (GameObject) Instantiate(objects[idx], new Vector3(x * 20, 0, y * 20), Quaternion.AngleAxis(angle, Vector3.up), parentObject);
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

    //IEnumerator GenCity()
    //{
    //    int rotationsCounter = 0;
    //    int[] rotations = { 90, 0, 180, 270 };
    //    for (int i = 0; i < height; i++)
    //    {
    //        for (int j = 0; j < width; j++)
    //        {
    //            Quaternion rotationQ = Quaternion.identity;
    //            GameObject prefab = laneRegular;
    //            if ((i == 0 || i == height - 1) && (j == 0 || j == width - 1)) // cantos
    //            {
    //                rotationQ = Quaternion.AngleAxis(rotations[rotationsCounter++], Vector3.up);
    //                prefab = laneCorner;
    //            }
    //            else if (i % (chunkSize + 1) == 0 && j % (chunkSize + 1) == 0) // interseccoes
    //            {
    //                if (i != 0 && i != height - 1 && j != 0 && j != width - 1)
    //                {
    //                    prefab = laneIntersection;
    //                }
    //                else
    //                {
    //                    float angle = 0;
    //                    if (j == 0)
    //                        angle = 180;
    //                    else if (j == width - 1)
    //                        angle = 0;

    //                    if (i == 0)
    //                        angle = 90;
    //                    else if (i == height - 1)
    //                        angle = 270;

    //                    rotationQ = Quaternion.AngleAxis(angle, Vector3.up);
    //                    prefab = laneTIntersection;
    //                }
    //            }
    //            else if (i % (chunkSize + 1) != 0 && j % (chunkSize + 1) != 0)
    //            {
    //                // considerar zona
    //                //bool isCommercial = CheckIfCommercialZone(j, i) || true;
    //                bool isResidential = CheckIfResidentialZone(j, i);
    //                InstantiateBuilding(j, i, isResidential, cityParentTransform);
    //                prefab = floorConcrete;
    //            }
    //            else // vias normais
    //            {
    //                prefab = laneRegular;
    //                if (j % (chunkSize + 1) != 0)
    //                {
    //                    rotationQ = Quaternion.AngleAxis(90, Vector3.up);
    //                }
    //            }
    //            cityMatrix[j, i] = Instantiate(prefab, new Vector3(j * 20, 0, i * 20), rotationQ, cityParentTransform);
    //            yield return null;
    //        }
    //    }
    //}
}
