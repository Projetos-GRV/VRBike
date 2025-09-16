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

    [Header("Prefabs")]
    [SerializeField] private GameObject _preafabCoin;
    [SerializeField] private GameObject _prefabDirection;
    [SerializeField] private GameObject _prefabSpeedMultiplier;

    private List<Vector2> _currentPath;

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            CreateCollectables();
        }
    }

    public void CreateCollectables()
    {
        _signalParent.ClearChilds();
        _collectablesParent.ClearChilds();

        _currentPath = new List<Vector2>();
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

            // Se n�o for o �ltimo n�, ajusta a rota��o do sinal e instancia moedas no caminho
            if (i < path.Count - 1)
            {
                var nextNode = path[i + 1];
                var dir = (nextNode.transform.position - node.transform.position).normalized;

                // Rota��o do sinal apontando para o pr�ximo n�
                signal.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                // Instanciar moedas entre node e nextNode
                int numCoins = _coinsPerSegment; // quantidade de moedas definida em vari�vel
                for (int c = 1; c <= numCoins; c++)
                {
                    float t = (float)c / (numCoins + 1); // fra��o ao longo do segmento
                    Vector3 coinPos = Vector3.Lerp(node.transform.position, nextNode.transform.position, t);

                    var coin = Instantiate(_preafabCoin, _collectablesParent);
                    coin.transform.position = coinPos + _cointOffet;

                    if (c == Mathf.Ceil(numCoins / 2))
                    {
                        Instantiate(_prefabSpeedMultiplier, coinPos + _speedMultiplierOffset, Quaternion.identity, _collectablesParent);
                    }
                }
            }

            // Registrar posi��o 2D no caminho
            _currentPath.Add(new Vector2(node.transform.position.x, node.transform.position.z));
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

    public void HandleGameViewChanged(bool isActive)
    {
        if (isActive) return;

        _signalParent.ClearChilds();
        _collectablesParent.ClearChilds();
    }
}
