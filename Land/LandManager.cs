using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public static LandManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> building = new List<GameObject>();

    private Dictionary<string, bool> buildingCheck = new Dictionary<string, bool>();

    public GameObject landParents;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }  
    }

    private void Start()
    {
        if (GameManager.Instance.GameLoad) return;
        InitializeLands();
    }

    public void InitializeLands()
    {
        Land[] lands = landParents.GetComponentsInChildren<Land>();
        InitializeBuilding();
        foreach (var land in lands)
        {
            land.ResetBuilding();
        }
    }

    private void InitializeBuilding()
    {
        buildingCheck.Clear();
        foreach (var buildings in building)
        {
            
            buildingCheck.Add(buildings.name, false);
        }
    }

    public void buildingStateUpdate(string name, bool check)
    {
        buildingCheck[name] = check;
    }

    public bool buildingState(string name)
    {
        return buildingCheck[name];
    }

    public GameObject SetBuilding(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        foreach (var buildingObject in building)
        {
            if (buildingObject.name == name)
            {
                return buildingObject;
            }
        }
        return null;
    }

    public bool isBuildingEveryActive()
    {
        foreach (var buildingObject in building)
        {
            if (!buildingObject.activeSelf) return false;
        }
        return true;
    }
}

