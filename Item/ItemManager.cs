using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField]
    private List<ItemData> items;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeItems()
    {
        foreach (ItemData item in items)
        {
            item.IsCompleted = false;
        }
    }

    public List<ItemData> GetItems()
    {
        return items;
    }

    public void CheckItem(int itemNumber)
    {
        Debug.Log($"아이템 체크 {itemNumber}");

        foreach (ItemData item in items)
        {
            if (item.ItemNumber == itemNumber)
            {
                Debug.Log($"아이템 찾음 {itemNumber}");
                if (item.CanObtain() && !item.IsCompleted)
                {
                    Debug.Log($"비교성공 {itemNumber}");
                    CompleteItem(item);
                }
                return;
            }
        }
    }

    private void CompleteItem(ItemData item)
    {
        Debug.Log($"Quest '{item.ItemName}' completed!");

        UIManager.Instance.UpdateItemList(item);
        SoundManager.Instance.PlaySFX("item");

        item.IsCompleted = true;
    }

    public void AddItem(ItemData newItem)
    {
        if (!items.Contains(newItem))
        {
            items.Add(newItem);
        }
    }

    public void RemoveItem(ItemData quest)
    {
        if (items.Contains(quest))
        {
            items.Remove(quest);
        }
    }

    public ItemData SearchItem(int num)
    {
        foreach (ItemData item in items)
        {
            if (item.ItemNumber == num)
            {
                return item;
            }
        }
        return null;
    }
}
