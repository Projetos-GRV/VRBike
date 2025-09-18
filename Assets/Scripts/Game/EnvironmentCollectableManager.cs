using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnvironmentCollectableManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GraphManager _graphManager;
    [SerializeField] private Transform _signalParent;

    [Header("Parameters")]
    [SerializeField] private int _pathSize = 15;
    [SerializeField] private Vector3 _cointOffet = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _signalOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _speedMultiplierOffset = new Vector3(1, 0.5f, 0);
    [SerializeField] private int _nodeToCheck = 1;
    [SerializeField] private int _coinsPerSegment = 3;
    [SerializeField, Range(0, 1)] private float _chanceToCreateSpeedMultiplier = 0.4f;
    [SerializeField] private int _signalsActiveInSameTime = 3;

    [Header("Prefabs")]
    [SerializeField] private GameObject _preafabCoin;
    [SerializeField] private GameObject _prefabDirection;
    [SerializeField] private GameObject _prefabSpeedMultiplier;

    private List<Vector2> _currentPath;
    private GameObject _nextSignal;

    private List<GameObject> _signals;
    private int _signalIndex = 0;

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            CreateCollectables();
        }
    }

    public void CreateCollectables()
    {
        Debug.Log($"[{GetType()}][CreateCollectables] Cria caminho");
        _signalParent.ClearChilds();

        _currentPath = new List<Vector2>();
        _signals = new List<GameObject>();

        var path = _graphManager.GetRandomPath(_pathSize);

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

            signal.SetActive(false);
            _signals.Add(signal);

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

                    var coin = Instantiate(_preafabCoin, signal.transform);
                    coin.transform.position = coinPos + _cointOffet;

                    if ((c - 1) == indexToCreateSpeedMultiplier)
                    {
                        var speedMultiplierInstance = Instantiate(_prefabSpeedMultiplier, coinPos + _speedMultiplierOffset, Quaternion.identity, signal.transform);
                    }
                }
            }

            // Registrar posição 2D no caminho
            _currentPath.Add(new Vector2(node.transform.position.x, node.transform.position.z));
        }

        _signalIndex = 0;

        UpdateSignals();
    }

    private void UpdateSignals()
    {
        int beginIndex = Mathf.Min(_signalIndex, _signals.Count - 1);
        int endIndex = Mathf.Min(_signalIndex + _signalsActiveInSameTime, _signals.Count);
        
        for(int i =0; i<_signals.Count; i++)
        {
            _signals[i].SetActive(i >= beginIndex && i <  endIndex);
        }
    }

    private void HandleSignalCollected(GameObject signalInstance)
    {
        bool createMoreCoins = false;

        var rest = _signals.Count - (_signalIndex);
        var count = rest > _signalsActiveInSameTime ? _signalsActiveInSameTime : rest;
        var signalInstanceIndex = _signals.FindIndex(_signalIndex, count, s => s == signalInstance);

        if (signalInstanceIndex >= 0)
        {
            _signalIndex = signalInstanceIndex;
            UpdateSignals();

            createMoreCoins = _signalIndex >= (_signals.Count - 1);

            Debug.Log($"[{GetType()}][HandleSignalCollected] Index {_signalIndex} de {_signals.Count-1}");
        }

        if (createMoreCoins)
            CreateCollectables();
    }

    public void Reset()
    {
        _signalParent.ClearChilds();

        _nextSignal = null;
        _signals.Clear();
    }

    public void HandleGameViewChanged(bool isActive)
    {
        if (isActive) return;

        Reset();
    }
}
