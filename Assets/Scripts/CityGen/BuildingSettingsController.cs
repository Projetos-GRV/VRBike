using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSettingsController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<Image> _imgBanners;


    [Header("Parameters")]
    [SerializeField] private List<ResearchGroupSO> _researchGroupsSOs;

    private void Awake()
    {
        if (_researchGroupsSOs.Count == 0) return;

        var groupData = _researchGroupsSOs.GetRandomItem();

        _imgBanners.ForEach(img => img.sprite = groupData.Banner);
    }
}
