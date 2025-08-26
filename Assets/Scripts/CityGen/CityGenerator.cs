using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Tooltip("Largura e \"Altura\" (comprimento) deve levar em consideração o tamanho de cada tile (as estradas e concreto, por exemplo, possuem tamanho 20x20).")]
    public int width = 50;
    [Tooltip("Largura e \"Altura\" (comprimento) deve levar em consideração o tamanho de cada tile (as estradas e concreto, por exemplo, possuem tamanho 20x20).")]
    public int height = 50;
    public int blockSize = 2;
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
    private (int X1, int Y1, int X2, int Y2)[] residentialZone;
    private (int X1, int Y1, int X2, int Y2)[] commercialZone;

    private Transform cityParentTransform;
    // Start is called before the first frame update
    void Start()
    {
        cityMatrix = new GameObject[width, height];
        cityParentTransform = new GameObject("CityParent").transform;
        StartCoroutine(GenCity()); // evitar congelamentos caso largura X altura seja mt grande
    }

    // Update is called once per frame
    void Update()
    {
        // caso usada geracao um pouco mais procedural, usar transform do jogador para atualizar chunks
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
                else if (i % (blockSize + 1) == 0 && j % (blockSize + 1) == 0) // interseccoes
                {
                    if (i != 0 && i != height - 1 && j != 0 && j != width - 1)
                    {
                        //rotationQ = Quaternion.identity;
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
                else if (i % (blockSize + 1) != 0 && j % (blockSize + 1) != 0)
                {
                    // considerar zona
                    bool isCommercial = CheckIfCommercialZone(j, i) || true;
                    //bool isResidential = CheckIfResidentialZone(j, i);
                    InstantiateBuilding(j, i, isCommercial);
                    prefab = floorConcrete;
                }
                else // vias normais
                {
                    prefab = laneRegular;
                    if (j % (blockSize + 1) != 0)
                    {
                        rotationQ = Quaternion.AngleAxis(90, Vector3.up);
                    }
                    //else
                    //{
                    //    rotationQ = Quaternion.identity;
                    //}
                }
                cityMatrix[j, i] = Instantiate(prefab, new Vector3(j * 20, 0, i * 20), rotationQ, cityParentTransform);
                yield return null;
            }
        }
    }

    // TODO - caso se deseje gerar a cidade novamente
    IEnumerator DestroyAndRegenerate()
    {
        yield return null;
    }

    // TODO - caso haja atualizacao dinamica dos blocos para melhorar desempenho
    IEnumerator UpdateChunks() { yield return null; }

    // TODO - verificar se ja foi posta no bloco, talvez? evitar haverem multiplas construcoes num bloco
    private void InstantiateBuilding(float x, float y, bool commercial)
    {
        GameObject[] objects = commercial ? commercialBuildings : residentialBuildings;
        int idx = Random.Range(0, objects.Length - 1);
        float angle = 90;
        if ((x - 1) % 3 == 0)
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
        Instantiate(objects[idx], new Vector3(x * 20, 0, y * 20), Quaternion.AngleAxis(angle, Vector3.up), cityParentTransform);
    }

    private bool CheckIfCommercialZone(int x, int y)
    {
        if (this.commercialZone == null)
        {
            return false;
        }
        foreach (var (x1, y1, x2, y2) in this.commercialZone)
        {
            if ((x >= x1 && x < x2) && (y >= y1 && y < y2))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckIfResidentialZone(int x, int y)
    {
        if (this.residentialZone == null)
        {
            return false;
        }
        foreach (var (x1, y1, x2, y2) in this.residentialZone)
        {
            if ((x >= x1 && x < x2) && (y >= y1 && y < y2))
            {
                return true;
            }
        }
        return false;
    }
}
