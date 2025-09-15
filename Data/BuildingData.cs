using UnityEngine;
using UnityEngine.UI;

public enum LevelUpNeedProcution { Wood, Stone, Iron, Food, None }

[CreateAssetMenu(fileName = "Data", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private int needWood;
    [SerializeField] private int needStone;
    [SerializeField] private int needIron;
    [SerializeField] private float needConstructSec;

    [SerializeField] private string buildingPrefabName;

    [SerializeField] private Image buildingImage;
    [SerializeField] private string buildingName;
    [SerializeField] private int buildingCode;
    [SerializeField] private string description;

    [SerializeField] private LevelUpNeedProcution production;
    [SerializeField] private int levelupNeedProductionValue;
    [SerializeField] private int levelupAddProductionValue;

    [SerializeField] private int productionWood;
    [SerializeField] private int productionStone;
    [SerializeField] private int productionIron;
    [SerializeField] private int productionFood;

    public int NeedWood => needWood;
    public int NeedStone => needStone;
    public int NeedIron => needIron;
    public float NeedConstructSec => needConstructSec;

    public string BuildingPrefabName => buildingPrefabName;

    public Image BuildingImage => buildingImage;
    public string BuildingName => buildingName;
    public int BuildingCode => buildingCode;
    public string Description => description;

    public LevelUpNeedProcution Production => production;
    public int LevelupNeedProductionValue => levelupNeedProductionValue;
    public int LevelupAddProductionValue => levelupAddProductionValue;

    public int ProductionWood => productionWood;
    public int ProductionStone => productionStone;
    public int ProductionIron => productionIron;
    public int ProductionFood => productionFood;
}
