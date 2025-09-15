using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int itemNumber;
    [SerializeField] private string itemName;
    [SerializeField] private string description; 
    [SerializeField] private string condition;
    [SerializeField] private bool isCompleted;
    [SerializeField] private Sprite image;
    [SerializeField] private ItemCondition itemCondition;

    public int ItemNumber 
    { 
        get { return itemNumber; } 
        set { itemNumber = value; } 
    }

    public string ItemName 
    { 
        get { return itemName; } 
        set { itemName = value; } 
    }

    public string Description 
    { 
        get { return description; } 
        set { description = value; } 
    }

    public string Condition 
    { 
        get { return condition; } 
        set { condition = value; } 
    }

    public bool IsCompleted
    { 
        get { return isCompleted; } 
        set { isCompleted = value; } 
    }

    public Sprite Image 
    { 
        get { return image; } 
        set { image = value; } 
    }

    public ItemCondition ItemCondition 
    { 
        get { return itemCondition; } 
        set { itemCondition = value; } 
    }

    public bool CanObtain()
    {
        if (isCompleted) { return false; }
        if (!itemCondition.IsCondition()) { return false; }
        return true;
    }
}
