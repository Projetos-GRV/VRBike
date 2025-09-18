using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderBoardEntry : MonoBehaviour, IUIItemComponent
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI _txtRank;
    [SerializeField] private TextMeshProUGUI _txtScore;
    [SerializeField] private TextMeshProUGUI _txtName;
    [SerializeField] private Image _imgSprite;

    [Header("Parameters")]
    [SerializeField] private string _scoreFormat = "00000";

    [Header("Fist place paramaters")]
    [SerializeField] private int _fisrtPlaceHeight = 130;
    [SerializeField] private Color _firstPlaceColor = Color.green;
    [SerializeField] private int _firstPlaceFontSize = 80;
    [SerializeField] private Sprite _firstPlaceSprite;

    [Header("Second place paramaters")]
    [SerializeField] private int _secondPlaceHeight = 110;
    [SerializeField] private Color _secondPlaceColor = Color.yellow;
    [SerializeField] private int _secondPlaceFontSize = 60;
    [SerializeField] private Sprite _secondPlaceSprite;

    [Header("Default place paramaters")]
    [SerializeField] private int _defaultPlaceHeight = 90;
    [SerializeField] private Color _defaultPlaceColor = Color.white;
    [SerializeField] private int _defaultPlaceFontSize = 40;

    public void HandleItemComponent(object item, Action<object, GameObject> onClickCallback)
    {
        var entry = (LeaderboardEntry)item;

        _txtRank.text = $"{entry.Rank}º";
        _txtScore.text = entry.Score.ToString(_scoreFormat);
        _txtName.text = entry.PlayerName;

        Color color;
        int fontSize;
        int itemHeight;
        Sprite sprite;

        switch (entry.Rank)
        {
            case 1: 
                _txtRank.gameObject.SetActive(false); 
                fontSize = _firstPlaceFontSize; 
                color = _firstPlaceColor; 
                itemHeight = _fisrtPlaceHeight;
                sprite = _firstPlaceSprite; 

                break;

            case 2: 
                _txtRank.gameObject.SetActive(false); 
                fontSize = _secondPlaceFontSize; 
                color = _secondPlaceColor; 
                itemHeight = _secondPlaceHeight; 
                sprite = _secondPlaceSprite ;

                break;

            default: 
                fontSize = _defaultPlaceFontSize; 
                color = _defaultPlaceColor; 
                itemHeight = _defaultPlaceHeight; 
                sprite = null; 
                _imgSprite.gameObject.SetActive(false); 

                break;
        }

        _txtRank.fontSize = _txtScore.fontSize = _txtName.fontSize = fontSize;
        _txtRank.color = _txtScore.color = _txtName.color = color;
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, itemHeight);
        _imgSprite.sprite = sprite;
    }
}
