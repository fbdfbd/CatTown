using UnityEngine;

[CreateAssetMenu(fileName = "Production", menuName = "Conditions/ProductionCondition")]
public class ProductionCondition : ItemCondition
{
    public int amount;
    public override bool IsCondition()
    {
        return Production.Instance.CheckTotalProduction(amount);
    }
}
