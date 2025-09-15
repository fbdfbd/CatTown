using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WorkKind { Wood, Stone }
public class Work : MonoBehaviour
{
    [SerializeField]
    private int workNumber;
    [SerializeField]
    private bool workState;
    [SerializeField]
    private bool workPossible;
    [SerializeField]
    private Cat workingCat;
    [SerializeField]
    private WorkKind thisWorkKind;
    private int workGetProduction;
    private int ranPos;
    public int WorkNumber => workNumber;
    public bool WorkState => workState;
    public bool WorkPossible => workPossible;
    public Cat WorkingCat => workingCat;
    public WorkKind ThisWorkKind => thisWorkKind;

    private void Awake()
    {
        workState = false;
        workPossible = true;
    }

    private void DisOnEnable()
    {
        RandomPosition();
    }

    private void OnEnable()
    {
        workState = false;
        workPossible = true;
    }

    public void RandomPosition()
    {
        float ranPos = Random.Range(-200f, 170f);
        transform.position = new Vector3(ranPos, transform.position.y, transform.position.z);
    }

    public bool WorkPossibleChange()
    {
        workPossible = !workPossible;
        return workPossible;
    }
    
    public void WorkCompleteGetProduction()
    {
        workGetProduction = Random.Range(1, 4);
        if (thisWorkKind == WorkKind.Wood)
        {
            Production.Instance.AddWood(workGetProduction);
        }
        else
        {
            Production.Instance.AddStone(workGetProduction);
        }
        Debug.Log($"일 완료 추가 자원 {workGetProduction}");
        GameManager.Instance.UpdateResourceUI();
    }

    public IEnumerator Regenerative()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(30f);
        gameObject.SetActive(true);
    }
}
