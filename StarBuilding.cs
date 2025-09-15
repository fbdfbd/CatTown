using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBuilding : MonoBehaviour
{
    private int clickThreshold = 20;
    private int clickCount = 0;

    private void OnMouseDown()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;
        Click();
    }

    private void Click()
    {
        clickCount++;
        GameManager.Instance.ClickCount++;

        if (clickCount >= clickThreshold)
        {
            AddRandomResource();
            SoundManager.Instance.PlaySFX("star");
            clickCount = 0;
        }
        else SoundManager.Instance.PlaySFX("starNot");
    }

    private void AddRandomResource()
    {
        float ran = Random.value;

        if (ran < 0.40f)
        {
            Production.Instance.AddWood(1);
        }
        else if (ran < 0.75f) 
        {
            Production.Instance.AddStone(1);
        }
        else
        { 
            Production.Instance.AddIron(1);
        }
        GameManager.Instance.UpdateResourceUI();
    }

}
