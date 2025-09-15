using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCollectableManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GraphManager _graphManager;
    [SerializeField] private Transform _collectablesParent;

    [Header("Parameters")]
    [SerializeField] private int _pathSize = 15;
    [SerializeField] private Vector3 _cointOffet = new Vector3(0, 0.5f, 0);
    [SerializeField] private int _nodeToCheck = 3;

    [Header("Prefabs")]
    [SerializeField] private GameObject _preafabCoin;

    private List<Vector2> _currentPath;

    public void CreateCollectables()
    {
        _currentPath = new List<Vector2>();
        var path = _graphManager.GetRandomPath(_pathSize);

        foreach (var node in path)
        {
            var coin = Instantiate(_preafabCoin, _collectablesParent);
            coin.transform.position = node.transform.position + _cointOffet;

            coin.GetComponent<CoinController>().OnCollected.AddListener(HandleCoinCollected);

            _currentPath.Add(new Vector2(node.transform.position.x, node.transform.position.z));
        }
    }

    private void HandleCoinCollected(CoinController coin)
    {
        Destroy(coin.gameObject);

        bool createMoreCoins = false;
        var pos2DCoin = new Vector2(coin.transform.position.x, coin.transform.position.z);

        for(int i = 0; i < _nodeToCheck; i++)
        {
            var node = _currentPath[_currentPath.Count - i - 1];

            if (Vector2.Distance(pos2DCoin, node) < 0.1f)
            {
                createMoreCoins = true;
                break;
            }
        }

        if (createMoreCoins)
            CreateCollectables();
    }
}
