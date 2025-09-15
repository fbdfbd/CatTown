using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    private List<GameObject> itemGoToInfoButton = new List<GameObject>();
    private List<bool> itemGoToInfoButtonActive = new List<bool>();
    [SerializeField]
    private GameObject ItemInfoPanel;
    [SerializeField]
    private GameObject itemListParents;
    [SerializeField]
    private TMP_Text itemTitle;
    [SerializeField]
    private TMP_Text itemDescription;
    [SerializeField]
    private TMP_Text itemCondition;

    private void Awake()
    {
        itemGoToInfoButton.Clear();
        itemGoToInfoButtonActive.Clear();

        foreach (Transform child in itemListParents.transform)
        {
            itemGoToInfoButton.Add(child.gameObject);
            itemGoToInfoButtonActive.Add(false);
        }
    }


    public void ItemInfoButtonLink(ItemData data)
    {
        int num = data.ItemNumber;
        itemGoToInfoButtonActive[num - 1] = true;
        itemGoToInfoButton[num - 1].transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ItemInfoButtonOff()
    {
        for (int i = 0; i < itemGoToInfoButton.Count; i++)
        {
            itemGoToInfoButton[i].transform.GetChild(1).gameObject.SetActive(false);
            itemGoToInfoButtonActive[i] = false;
        }
    }

    public void ItemInfoButton(int num)
    {
        if (!itemGoToInfoButtonActive[num - 1])
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("수집되지 않았습니다");
        }
        else
        {
            ItemInfoPanel.SetActive(true);
            ItemInfoUpdate(num);
        }
    }


    private void ItemInfoUpdate(int num)
    {
        ItemData itemdata = ItemManager.Instance.SearchItem(num);
        itemTitle.text = itemdata.ItemName;
        itemDescription.text = itemdata.Description;
        itemCondition.text = itemdata.Condition;
    }
}
