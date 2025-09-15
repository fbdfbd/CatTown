using UnityEngine;

[CreateAssetMenu(fileName = "BuildingAllActive", menuName = "Conditions/BuildingAllActiveCondition")]
public class BuildingAllActiveCondition : ItemCondition
{
    public override bool IsCondition()
    {
        return LandManager.Instance.isBuildingEveryActive();
    }
}