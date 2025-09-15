using UnityEngine;

[CreateAssetMenu(fileName = "StarClick", menuName = "Conditions/StarClickCondition")]
public class StarClickCondition : ItemCondition
{
    public int click;
    public override bool IsCondition()
    {
        return GameManager.Instance.ClickCountCheck(click);
    }
}