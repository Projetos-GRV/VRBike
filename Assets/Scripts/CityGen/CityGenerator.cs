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
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //if (i % 3 == 0 && j % 3 == 0)
                //{
                //    if (i != 0 && i != height - 1 && j != 0 && j != width - 1)
                //    {
                //        cityMatrix[j, i] = Instantiate(laneIntersection, new Vector3(j * 20, 0, i * 20), Quaternion.identity, cityParentTransform);
                //    }
                //    else
                //    {
                //        cityMatrix[j, i] = Instantiate(laneTIntersection, new Vector3(j * 20, 0, i * 20), Quaternion.AngleAxis(90, Vector3.up), cityParentTransform);
                //    }
                //}
                //else
                //{
                //    cityMatrix[j, i] = Instantiate(laneRegular, new Vector3(j * 20, 0, i * 20), Quaternion.identity, cityParentTransform);
                //}
                cityMatrix[j, i] = Instantiate(laneIntersection, new Vector3(j * 20, 0, i * 20), Quaternion.identity, cityParentTransform);
                yield return null;
            }
        }
    }

    IEnumerator UpdateChunks() { yield return null; }
}
