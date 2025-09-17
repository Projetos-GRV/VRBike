using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnvironmentCollectableManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GraphManager _graphManager;
    [SerializeField] private Transform _collectablesParent;
    [SerializeField] private Transform _signalParent;

    [Header("Parameters")]
    [SerializeField] private int _pathSize = 15;
    [SerializeField] private Vector3 _cointOffet = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _signalOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _speedMultiplierOffset = new Vector3(1, 0.5f, 0);
    [SerializeField] private int _nodeToCheck = 1;
    [SerializeField] private int _coinsPerSegment = 3;
    [SerializeField, Range(0, 1)] private float _chanceToCreateSpeedMultiplier = 0.4f;


    [Header("Prefabs")]
    [SerializeField] private GameObject _preafabCoin;
    [SerializeField] private GameObject _prefabDirection;
    [SerializeField] private GameObject _prefabSpeedMultiplier;

    private List<Vector2> _currentPath;
    private GraphManager.Node _lastNode;
    private Vector3 _lastDirection;

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            CreateCollectables();
        }
    }

    public void CreateCollectables()
    {
        _currentPath = new List<Vector2>();

        var path = new List<GraphManager.Node>();

        if (_lastNode == null)
        {
            path = _graphManager.GetRandomPath(_pathSize);
        }
        else
        {
            path = _graphManager.GetRandomPath(_lastNode, _lastDirection, _pathSize - 1, true);
        }

        for (int i = 0; i < path.Count; i++)
        {
            var node = path[i];

            // Instancia sinal em cada esquina
            var signal = Instantiate(_prefabDirection, _signalParent);
            signal.transform.position = node.transform.position + _signalOffset;

            if (signal.TryGetComponent(out SimpleCollectableController controller))
            {
                controller.OnCollected.AddListener(HandleSignalCollected);
            }

            // Se não for o último nó, ajusta a rotação do sinal e instancia moedas no caminho
            if (i < path.Count - 1)
            {
                var nextNode = path[i + 1];
                var dir = (nextNode.transform.position - node.transform.position).normalized;

                // Rotação do sinal apontando para o próximo nó
                signal.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                var indexToCreateSpeedMultiplier = UnityEngine.Random.Range(0, 1) <= _chanceToCreateSpeedMultiplier ? UnityEngine.Random.Range(0, _coinsPerSegment) : -1;

                // Instanciar moedas entre node e nextNode
                int numCoins = _coinsPerSegment; // quantidade de moedas definida em variável
                for (int c = 1; c <= numCoins; c++)
                {
                    float t = (float)c / (numCoins + 1); // fração ao longo do segmento
                    Vector3 coinPos = Vector3.Lerp(node.transform.position, nextNode.transform.position, t);

                    var coin = Instantiate(_preafabCoin, _collectablesParent);
                    coin.transform.position = coinPos + _cointOffet;

                    if ((c - 1) == indexToCreateSpeedMultiplier)
                    {
                        Instantiate(_prefabSpeedMultiplier, coinPos + _speedMultiplierOffset, Quaternion.identity, _collectablesParent);
                    }
                }


                _lastDirection = dir;
            }

            // Registrar posição 2D no caminho
            _currentPath.Add(new Vector2(node.transform.position.x, node.transform.position.z));

            _lastNode = node;
        }
    }

    private void HandleSignalCollected(GameObject signalInstance)
    {
        bool createMoreCoins = false;
        var pos2DCoin = new Vector2(signalInstance.transform.position.x, signalInstance.transform.position.z);

        for (int i = 0; i < _nodeToCheck; i++)
        {
            var node = _currentPath[_currentPath.Count - i - 1];

            if (Vector2.Distance(pos2DCoin, node) < 0.1f)
            {
                createMoreCoins = true;
                break;
            }
        }

        Destroy(signalInstance);

        if (createMoreCoins)
            CreateCollectables();
    }

    public void Reset()
    {
        _signalParent.ClearChilds();
        _collectablesParent.ClearChilds();

        _lastNode = null;
        _lastDirection = Vector3.zero;
    }

    public void HandleGameViewChanged(bool isActive)
    {
        if (isActive) return;

        Reset();
    }
}
