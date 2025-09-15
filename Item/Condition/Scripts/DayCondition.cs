using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Conditions/DayCondition")]
public class DayCondition : ItemCondition
{
    public int day;
    public override bool IsCondition()
    {
        return Production.Instance.DayCheck(day);
    }
}
