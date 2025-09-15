using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GraphManager : MonoBehaviour
{
    [System.Serializable]
    public class Node
    {
        public Transform transform;
        public List<Node> neighbors = new List<Node>();

        public Node(Transform t)
        {
            transform = t;
        }
    }

    public List<Transform> transforms; // Lista de pontos na cena
    public Dictionary<Vector3, Node> graph = new Dictionary<Vector3, Node>();

    [Header("Graph Settings")]
    public float threshold = 0.1f; // tolerância para checar vizinhos
    public float expectedDistance = 1f; // distância esperada entre vizinhos

    [Header("Player Tracking")]
    public Transform player; // Transform do jogador
    private Node currentPlayerNode;

    [Header("Path Generation Settings")]
    [Range(0f, 1f)] public float probContinueStraight = 0.7f; // chance de continuar reto
    [Range(0f, 1f)] public float probForceTurn = 0.2f;        // chance de forçar curva mesmo tendo opção reta

    [Header("Debug Settings")]
    public bool drawDebug = true;
    public GameObject nodePrefab;
    public Material edgeMaterial;
    public Material pathMaterial;

    private List<GameObject> debugObjects = new List<GameObject>();
    private GameObject currentPathLine;

    public int pathSize = 15;

    public void HandleCityChanged(CityGenerator cityGenerator)
    {
        transforms = cityGenerator.StreetPositions.ToList();

        BuildGraph();

        if (drawDebug)
            DrawDebugGraph();
    }

    void Update()
    {
        if (player != null)
            currentPlayerNode = GetClosestNode(player.position);

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            GetRandomPath(pathSize);
        }
    }

    void BuildGraph()
    {
        graph.Clear();

        // Cria nós
        foreach (Transform t in transforms)
        {
            Vector3 pos = RoundVector(t.position);
            if (!graph.ContainsKey(pos))
            {
                graph.Add(pos, new Node(t));
            }
        }

        // Conecta vizinhos
        foreach (var kvp in graph)
        {
            Node node = kvp.Value;
            Vector3 pos = node.transform.position;

            foreach (var other in graph.Values)
            {
                if (other == node) continue;

                Vector3 diff = other.transform.position - pos;

                // Só conecta se a diferença for em UM eixo
                bool onlyOneAxis =
                    (Mathf.Abs(diff.x) > threshold && Mathf.Abs(diff.y) <= threshold && Mathf.Abs(diff.z) <= threshold) ||
                    (Mathf.Abs(diff.y) > threshold && Mathf.Abs(diff.x) <= threshold && Mathf.Abs(diff.z) <= threshold) ||
                    (Mathf.Abs(diff.z) > threshold && Mathf.Abs(diff.x) <= threshold && Mathf.Abs(diff.y) <= threshold);

                if (onlyOneAxis)
                {
                    float dist = diff.magnitude;

                    if (Mathf.Abs(dist - expectedDistance) <= threshold)
                    {
                        if (!node.neighbors.Contains(other))
                            node.neighbors.Add(other);

                        if (!other.neighbors.Contains(node))
                            other.neighbors.Add(node);
                    }
                }
            }
        }

        Debug.Log("Grafo montado com " + graph.Count + " nós.");
    }

    Node GetClosestNode(Vector3 position)
    {
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (var node in graph.Values)
        {
            float dist = Vector3.Distance(node.transform.position, position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        return closest;
    }

    /// <summary>
    /// Gera um caminho aleatório a partir do jogador com N passos.
    /// Privilegia caminhos retos conforme probabilidades configuradas.
    /// Agora começa na direção que o jogador está olhando.
    /// </summary>
    public List<Node> GetRandomPath(int length)
    {
        List<Node> path = new List<Node>();

        if (currentPlayerNode == null) return path;

        Node current = currentPlayerNode;
        path.Add(current);

        Vector3? lastDirection = null;

        // --- PASSO 1: tentar alinhar início com direção do jogador ---
        if (player != null && current.neighbors.Count > 0)
        {
            Vector3 forward = player.forward.normalized;

            Node best = null;
            float bestDot = -1f;

            foreach (var neighbor in current.neighbors)
            {
                Vector3 dir = (neighbor.transform.position - current.transform.position).normalized;
                float dot = Vector3.Dot(forward, dir);

                if (dot > bestDot)
                {
                    bestDot = dot;
                    best = neighbor;
                }
            }

            if (best != null)
            {
                path.Add(best);
                lastDirection = (best.transform.position - current.transform.position).normalized;
                current = best;
            }
        }

        // --- PASSO 2: continua normalmente ---
        for (int i = path.Count - 1; i < length; i++)
        {
            List<Node> candidates = new List<Node>();

            foreach (var neighbor in current.neighbors)
            {
                if (path.Count > 1 && neighbor == path[path.Count - 2])
                    continue; // evita voltar

                candidates.Add(neighbor);
            }

            if (candidates.Count == 0) break;

            Node chosen = null;

            if (lastDirection.HasValue)
            {
                Vector3 forwardDir = lastDirection.Value;

                Node straightCandidate = candidates.Find(n =>
                {
                    Vector3 dir = (n.transform.position - current.transform.position).normalized;
                    return Vector3.Dot(forwardDir, dir) > 0.9f;
                });

                if (straightCandidate != null)
                {
                    if (Random.value < probContinueStraight)
                    {
                        chosen = straightCandidate;
                    }
                    else if (Random.value < probForceTurn)
                    {
                        List<Node> turns = new List<Node>(candidates);
                        turns.Remove(straightCandidate);
                        if (turns.Count > 0)
                            chosen = turns[Random.Range(0, turns.Count)];
                    }
                }
            }

            if (chosen == null)
                chosen = candidates[Random.Range(0, candidates.Count)];

            lastDirection = (chosen.transform.position - current.transform.position).normalized;
            path.Add(chosen);
            current = chosen;
        }

        if (drawDebug)
            DrawPath(path);

        return path;
    }

    // ---------- DEBUG VISUAL ----------
    void DrawDebugGraph()
    {
        ClearDebug();

        foreach (var kvp in graph)
        {
            Node node = kvp.Value;
            Vector3 pos = node.transform.position;

            GameObject go;
            if (nodePrefab != null)
                go = Instantiate(nodePrefab, pos, Quaternion.identity);
            else
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            go.transform.position = pos;
            go.transform.localScale = Vector3.one * 0.5f;
            go.name = "Node_" + pos;
            debugObjects.Add(go);

            foreach (var neighbor in node.neighbors)
            {
                if (neighbor == null) continue;

                if (neighbor.transform.position.sqrMagnitude > node.transform.position.sqrMagnitude)
                {
                    GameObject line = new GameObject("Edge");
                    LineRenderer lr = line.AddComponent<LineRenderer>();
                    lr.positionCount = 2;
                    lr.SetPosition(0, pos);
                    lr.SetPosition(1, neighbor.transform.position);
                    lr.startWidth = 0.05f;
                    lr.endWidth = 0.05f;
                    lr.material = edgeMaterial != null ? edgeMaterial : new Material(Shader.Find("Sprites/Default"));
                    lr.startColor = Color.green;
                    lr.endColor = Color.green;

                    debugObjects.Add(line);
                }
            }
        }
    }

    void DrawPath(List<Node> path)
    {
        if (currentPathLine != null) Destroy(currentPathLine);

        if (path == null || path.Count < 2) return;

        currentPathLine = new GameObject("PathLine");
        LineRenderer lr = currentPathLine.AddComponent<LineRenderer>();
        lr.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            lr.SetPosition(i, path[i].transform.position + Vector3.up * 0.2f);
        }

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = pathMaterial != null ? pathMaterial : new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.yellow;
        lr.endColor = Color.red;
    }

    void ClearDebug()
    {
        foreach (var obj in debugObjects)
        {
            if (obj != null) Destroy(obj);
        }
        debugObjects.Clear();
    }

    Vector3 RoundVector(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x * 10f) / 10f,
            Mathf.Round(v.y * 10f) / 10f,
            Mathf.Round(v.z * 10f) / 10f
        );
    }
}
