using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPositionController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _bikeTransform;
    [SerializeField] private GraphManager _graphManager;
    
    public void ResetPlayerPosition()
    {
        Debug.Log($"ResetPlayerPosition   {_graphManager.GetPlayerNode().transform.position}");
        var referencePoint = _graphManager.GetPlayerNode().transform.position;

        var newPositionPlayer = new Vector3(referencePoint.x, _playerTransform.position.y, referencePoint.z);
        var newPositionBike = new Vector3(referencePoint.x, _bikeTransform.position.y, referencePoint.z);

        _playerTransform.position = newPositionPlayer;
        _bikeTransform.position = newPositionBike;
    }
}
