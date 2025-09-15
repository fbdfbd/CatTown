using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingPrefab;
    [SerializeField]
    private Transform contentPanel;
    [SerializeField]
    private List<BuildingData> buildingDataList;
    [SerializeField]
    private TMP_Text constructingBuilding;

    [SerializeField]
    private TMP_Text buildingTitle;
    [SerializeField]
    private TMP_Text buildingDescription;
    [SerializeField]
    private TMP_Text buildingProduction;
    [SerializeField]
    private TMP_Text levelText;

    [SerializeField]
    private GameObject levelUPPanel;
    [SerializeField]
    private TMP_Text levelUPPanelText;
    private Land selectedLand;

    void Start()
    {
        PopupBuildingList();
    }

    public void SetSelectedLand()
    {
        selectedLand = UIManager.Instance.selectedLand;
    }

    public BuildingData ReturnBuildingData(string buildingPrefabName)
    {
        foreach(BuildingData bdData in buildingDataList)
        {
            if (bdData.BuildingPrefabName == buildingPrefabName) return bdData;
        }
        return null;
    }

    private void PopupBuildingList()
    {
        foreach (BuildingData buildingData in buildingDataList)
        {
            GameObject newBuildingPanel = Instantiate(buildingPrefab, contentPanel);

            TMP_Text buildingNameText = newBuildingPanel.transform.GetChild(2).GetComponent<TMP_Text>();
            buildingNameText.text = buildingData.BuildingName;

            TMP_Text buildingProductionText = newBuildingPanel.transform.GetChild(3).GetComponent<TMP_Text>();
            buildingProductionText.text = $"나무 {buildingData.NeedWood}, 돌 {buildingData.NeedStone}, 철 {buildingData.NeedIron}";


            TMP_Text buildingDescriptionText = newBuildingPanel.transform.GetChild(4).GetComponent<TMP_Text>();
            buildingDescriptionText.text = buildingData.Description;

            Button button = newBuildingPanel.transform.GetChild(5).GetComponent<Button>();
            button.onClick.AddListener(() => OnBuildingSelected(buildingData));
        }
    }


    private void OnBuildingSelected(BuildingData buildingData)
    {
        SetSelectedLand();
        if (selectedLand == null)
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("땅이 선택되지 않았습니다");
            return;
        }

        bool canBuild = isBuildingPossible(buildingData);
        bool alreadyBuilt = LandManager.Instance.buildingState(buildingData.BuildingPrefabName);

        if(alreadyBuilt && canBuild)
        {
            Production.Instance.AddWood(buildingData.NeedWood);
            Production.Instance.AddIron(buildingData.NeedIron);
            Production.Instance.AddStone(buildingData.NeedStone);
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("해당 건물은 건설되었습니다\n(1개 건설 가능)");
            return;
        }

        else if (alreadyBuilt && !canBuild )
        { 
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("해당 건물은 건설되었습니다\n(1개 건설 가능)");
            return;
        }

        if (canBuild)
        {
            selectedLand.IsBuild = LandState.Construction;
            LandManager.Instance.buildingStateUpdate(buildingData.BuildingPrefabName, true);
            selectedLand.BuildingData = buildingData;
            selectedLand.BuildingLevel = 1;

            constructingBuilding.text = buildingData.BuildingName;
            Debug.Log($"Built {buildingData.BuildingName} on {selectedLand.name}");
            UIManager.Instance.UpdateBuildingUI(selectedLand);
            GameManager.Instance.UpdateResourceUI();
        }
        else
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("자원이 부족합니다");
        }
    }
    public void BuildingConstructPanelUpdate()
    {
        SetSelectedLand();
        constructingBuilding.text = selectedLand.BuildingData.BuildingName;
    }

    public void BuildingInfoPanelUpdate()
    {
        SetSelectedLand();
        buildingTitle.text = selectedLand.BuildingData.BuildingName;
        buildingDescription.text = selectedLand.BuildingData.Description;
        levelText.text = selectedLand.BuildingLevel.ToString();
        var resources = new[]
        {
            selectedLand.BuildingProductions[0] != 0 ? $"나무+{selectedLand.BuildingProductions[0]}/m " : null,
            selectedLand.BuildingProductions[1] != 0 ? $"돌+{selectedLand.BuildingProductions[1]}/m " : null,
            selectedLand.BuildingProductions[2] != 0 ? $"철+{selectedLand.BuildingProductions[2]}/m " : null,
            selectedLand.BuildingProductions[3] != 0 ? $"음식+{selectedLand.BuildingProductions[3]}/m " : null
        };
        buildingProduction.text = string.Join(" ", resources.Where(r => r != null));

    }

    private bool isBuildingPossible(BuildingData buildingData)
    {
        return Production.Instance.ConsumeWood(buildingData.NeedWood) &&
               Production.Instance.ConsumeStone(buildingData.NeedStone) &&
               Production.Instance.ConsumeIron(buildingData.NeedIron);
    }

    public void BuildingDemolish()
    {
        selectedLand.ResetBuilding();
    }

    public void BuildingLevelUpUISee()
    {
        if (selectedLand.BuildingData.Production == LevelUpNeedProcution.None)
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("레벨업이 불가능한 건물입니다");
            return;
        }
        levelUPPanel.SetActive(true);
        string procution = Production.Instance.ProductionStringKR(selectedLand.BuildingData.Production.ToString());
        levelUPPanelText.text = $"{procution}이/가 {selectedLand.BuildingNeedProduction}개 소모됩니다.";
    }


    public void BuildlingLevelUpBTN()
    {
        if (selectedLand.BuildingLevelUpStart())
        {
            UIManager.Instance.UpdateBuildingUI(selectedLand);
        }
        else
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("자원이 부족합니다");
        }
    }
}
