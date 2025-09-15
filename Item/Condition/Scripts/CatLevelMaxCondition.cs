using UnityEngine;

[CreateAssetMenu(fileName = "CatMaxLevel", menuName = "Conditions/CatLevelMaxCondition")]
public class CatLevelMaxCondition : ItemCondition
{
    public override bool IsCondition()
    {
        return CatManager.Instance.IsCatFullLevel();
    }
}
