using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LandState { Empty, Construction, Building }

public class Land : MonoBehaviour
{
    private const float BUILDING_PRODUCTION_INTERVAL = 60f;

    [SerializeField]
    private LandState isBuild;
    [SerializeField]
    private BuildingData buildingData;
    [SerializeField]
    private int buildingLevel;
    [SerializeField]
    private int buildingNeedProduction;
    [SerializeField]
    private GameObject buildingObject;
    [SerializeField]
    private GameObject landBorder;

    private ParticleSystem particle;

    private List<int> buildingProductions = new List<int>();

    private LandBar landBar;

    private Coroutine productionCoroutine;

    public LandState IsBuild
    {
        get { return isBuild; }
        set
        {
            if (isBuild != value)
            {
                isBuild = value;
                if (!GameManager.Instance.GameLoad) StateChanged();
            }
        }
    }

    public BuildingData BuildingData
    {
        get { return buildingData; }
        set { buildingData = value; }
    }

    public int BuildingNeedProduction => buildingNeedProduction;

    public int BuildingLevel
    {
        get { return buildingLevel; }
        set { buildingLevel = value; }
    }

    public GameObject LandBorder
    {
        get { return landBorder; }
        set { landBorder = value; }
    }

    public List<int> BuildingProductions
    {
        get { return buildingProductions; }
        set { buildingProductions = value; }
    }

    public GameObject BuildingObject
    {
        get { return buildingObject; }
        set { buildingObject = value; }
    }


    public Action<Land> OnClicked;

    private void OnMouseDown()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;
        OnClicked?.Invoke(this);
    }

    private void Start()
    {
        InitializeLand();
    }

    public void InitializeLand()
    {
        landBar = GetComponent<LandBar>();

        if (buildingObject == null)
        {
            isBuild = LandState.Empty;
            buildingData = null;
            buildingLevel = 0;
            buildingProductions = new List<int> { 0, 0, 0, 0 };
        }
    }

    public void StateChanged()
    {
        switch ( isBuild )
        {
            case LandState.Building:
                CompleteBuilding();
                BuildingCreate(BuildingData.BuildingPrefabName);

                if (UIManager.Instance.selectedLand == this)
                    UIManager.Instance.UpdateBuildingUI(this);
                SetParticle();

                ItemManager.Instance.CheckItem(2);

                if (productionCoroutine != null)
                    StopCoroutine(productionCoroutine);
                productionCoroutine = StartCoroutine(BuildingProduction());
                break;
            case LandState.Construction:
                landBar.StartFilling();
                break;
            case LandState.Empty:
                if (productionCoroutine != null)
                {
                    StopCoroutine(productionCoroutine);
                    productionCoroutine = null;
                }
                break;
        }
    }

    private void SetParticle()
    {
        particle = buildingObject.GetComponentInChildren<ParticleSystem>();
        if (GameManager.Instance.GameLoad) return;
        particle.Play();
        SoundManager.Instance.PlaySFX("up2");
    }
    public void BuildingCreate(string name)
    {
        Debug.Log(name);
        buildingObject = LandManager.Instance.SetBuilding(name);
        buildingObject.SetActive(true);
        buildingObject.transform.position = new Vector3(transform.position.x, 0, 40);
        buildingObject.GetComponent<BuildingInteraction>().BuildingConnectLand(this);

    }
    public void CompleteBuilding()
    {
        if (GameManager.Instance.GameLoad) return;

        buildingData = BuildingData;

        buildingLevel = 1;
        buildingNeedProduction = buildingData.LevelupNeedProductionValue;
        buildingProductions[0] = buildingData.ProductionWood;
        buildingProductions[1] = buildingData.ProductionStone;
        buildingProductions[2] = buildingData.ProductionIron;
        buildingProductions[3] = buildingData.ProductionFood;

        isBuild = LandState.Building;
    }

    public void ResetBuilding()
    {
        isBuild = LandState.Empty;
        
        if (buildingObject != null)
        {
            LandManager.Instance.buildingStateUpdate(buildingData.BuildingPrefabName, false);
            buildingObject.SetActive(false);
            buildingObject = null;
        }
        buildingData = null;
        particle = null;
        buildingNeedProduction = 0;
        buildingLevel = 0;
        
    }

    public void BorderOnOff(bool state)
    {
        landBorder.SetActive(state);
    }

    private IEnumerator BuildingProduction()
    {
        while(true)
        {
            yield return new WaitForSeconds(BUILDING_PRODUCTION_INTERVAL);
            Production.Instance.AddWood(buildingProductions[0]);
            Production.Instance.AddStone(buildingProductions[1]);
            Production.Instance.AddIron(buildingProductions[2]);
            Production.Instance.AddFood(buildingProductions[3]);
            GameManager.Instance.UpdateResourceUI();
        }
    }

    public bool BuildingLevelUpStart()
    {
        if (LevelUpPossible())
        {
            BuildingLevelUp();
            GameManager.Instance.UpdateResourceUI();
            return true;
        }
        else
            return false;
    }

    private bool LevelUpPossible()
    {
       if(buildingData.Production == LevelUpNeedProcution.Wood)
       {
           return Production.Instance.ConsumeWood(buildingNeedProduction);
       }
       else if (buildingData.Production == LevelUpNeedProcution.Stone)
       {
            return Production.Instance.ConsumeStone(buildingNeedProduction);
       }
       else if(buildingData.Production == LevelUpNeedProcution.Iron)
       {
            return Production.Instance.ConsumeIron(buildingNeedProduction);
       }
       else
       {
            return Production.Instance.ConsumeFood(buildingNeedProduction);
        }
    }

    private void BuildingLevelUp()
    {
        buildingLevel++;
        buildingNeedProduction *= 2;
        for (int i = 0; i < buildingProductions.Count; i++)
        {
            if (buildingProductions[i] > 0)
            {
                buildingProductions[i] = buildingProductions[i] + buildingData.LevelupAddProductionValue;
            }
        }
        SoundManager.Instance.PlaySFX("up2");

    }
}
