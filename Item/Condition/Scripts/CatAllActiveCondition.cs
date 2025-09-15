
using UnityEngine;

[CreateAssetMenu(fileName = "CatAllActive", menuName = "Conditions/CatAllActiveCondition")]
public class CatAllActiveCondition : ItemCondition
{
    public override bool IsCondition()
    {
        return CatManager.Instance.IsCatEveryActive();
    }
}