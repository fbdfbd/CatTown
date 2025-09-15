using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject errorPanel;
    [SerializeField]
    private TMP_Text errorPanelText;

    [Header("UI PanelList")]
    [SerializeField]
    private List<GameObject> UIPanels;

    [Header("Home UI")]
    public GameObject Ãß°¡;

    [Header("Building UI")]
    public GameObject buildingInfoPanel;
    public GameObject buildingScrollPanel;
    public GameObject buildingConstructionPanel;
    public TMP_Text buildingNameText;
    public TMP_Text buildingLevelText;
    public TMP_Text buildingDescriptionText;
    public TMP_Text buildingProductionText;
    public BuildingPanel buildingPanel;
    public Land selectedLand;

    [Header("Cat UI")]
    public GameObject catInfoPanel;
    public GameObject catListPanel;
    public CatPanel catPanel;
    public Cat selectedCat;

    [Header("Item UI")]
    public ItemPanel itemPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        selectedLand = null;
        selectedCat = null;
    }

    public void ActivatePanel(string panelName)
    {
        foreach (var panel in UIPanels)
        {
            panel.SetActive(panel.name.Equals(panelName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public void SelectLand(Land land)
    {
        if (selectedLand == land)
        {
            DeselectLand();
            return;
        }

        if (selectedLand != null)
        {
            selectedLand.BorderOnOff(false);
        }

        selectedLand = land;
        selectedLand.BorderOnOff(true);
        UpdateBuildingUI(land);
    }

    private void DeselectLand()
    {
        if (selectedLand != null)
        {
            selectedLand.BorderOnOff(false);
            selectedLand = null;
        }
    }
    public void UpdateBuildingUI(Land land)
    {
        ActivatePanel("Building");

        buildingInfoPanel.SetActive(land.IsBuild == LandState.Building);
        buildingScrollPanel.SetActive(land.IsBuild == LandState.Empty);
        buildingConstructionPanel.SetActive(land.IsBuild == LandState.Construction);

        if (land.IsBuild == LandState.Construction)
        {
            buildingPanel.BuildingConstructPanelUpdate();
        }
        else if (land.IsBuild == LandState.Building)
        {
            buildingPanel.BuildingInfoPanelUpdate();
        }
    }

    public void SelectCat(Cat cat)
    {
        selectedCat = cat;
        ActivatePanel("Cat");
        catListPanel.SetActive(false);
        catInfoPanel.SetActive(true);
        catPanel.CatInfoUpdate(cat);
    }

    public void SelectedCatInfoUpdate(Cat cat)
    {
        if (!catInfoPanel.activeSelf) return;
        if (selectedCat == cat)
            catPanel.CatInfoUpdate(cat);
        else return;
    }

    public void CreateCat(Cat cat)
    {
        catPanel.CatListUpdate(cat);
    }

    public void ShowError(string error)
    {
        errorPanel.SetActive(true);
        errorPanelText.text = error;
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void UpdateItemList(ItemData itemData)
    {
        itemPanel.ItemInfoButtonLink(itemData);
    }

    public void InitializeUI()
    {
        catPanel.InitializeCatPanel();
        itemPanel.ItemInfoButtonOff();
        DeselectLand();
        selectedCat = null;
    }

    public BuildingData ReturnPrefabNameToBuildingData(string name)
    {
        return buildingPanel.ReturnBuildingData(name);
    }

    public void SetSelectedLandNull()
    {
        if(selectedLand != null)
        {
            selectedLand.BorderOnOff(false);
            selectedLand = null;
        }
    }
}
