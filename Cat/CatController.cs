using System;
using System.Collections;
using UnityEngine;

public enum CatState { Idle, Rest, Work, Battle }

public class CatController : MonoBehaviour
{
    private Cat cat;
    private CatMovement catMovement;

    public event Action<Cat> OnClicked;
    public event Action<Cat> OnMouseOver;
    public event Action<Cat> OnMouseExitEvent;

    [SerializeField]
    private CatState thisCatState;

    public CatState ThisCatState
    {
        get { return thisCatState; }
        set
        {
            if (thisCatState != value)
            {
                Debug.Log("고양이 상태 변경");
                thisCatState = value;
                ExecuteStateAction(thisCatState);
            }
        }
    }

    private void OnEnable()
    {
        cat = GetComponent<Cat>();
        catMovement = GetComponent<CatMovement>();
        thisCatState = CatState.Idle;

        ExecuteStateAction(thisCatState);
    }
    private void OnMouseDown()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;
        OnClicked?.Invoke(cat);
    }

    private void OnMouseEnter()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;
        OnMouseOver?.Invoke(cat);
    }

    private void OnMouseExit()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;
        OnMouseExitEvent?.Invoke(cat);
    }

    private void ExecuteStateAction(CatState state)
    {
        
        switch (state)
        {
            case CatState.Idle:
                CatStateTagUpdate("노는 중");
                catMovement.MoveToState(CatState.Idle);
                break;
            case CatState.Rest:
                CatStateTagUpdate("휴식 중");
                catMovement.MoveToState(CatState.Rest);
                break;
            case CatState.Work:
                CatStateTagUpdate("일하는 중");
                catMovement.MoveToState(CatState.Work);
                break;
            case CatState.Battle:
                CatStateTagUpdate("싸우는 중");
                catMovement.MoveToState(CatState.Battle);
                break;
        }
    }

    public void CatCycleStart()
    {
        if(cat.CatSp >= 15)
        {
            int ran = UnityEngine.Random.Range(0, 2);
            if (ran == 0)
            {
                ExecuteStateAction(CatState.Work);
            }
            else
            {
                ExecuteStateAction(CatState.Idle);
            }
        }
        else
        {
            ExecuteStateAction(CatState.Rest);
        }
        
    }


    public void CatWorkCompleteToState()
    {
        int ran1 = UnityEngine.Random.Range(1, 11);
        cat.CatSp -= ran1;
        int ran2 = UnityEngine.Random.Range(1, 11);
        cat.CatExp += ran2;
    }

    public IEnumerator CatResting()
    {
        while (cat.CatSp < 100)
        {
            yield return new WaitForSeconds(1f);
            cat.CatSp += 1;
        }
    }

    private void CatStateTagUpdate(string state)
    {
        cat.CatStateTag.text = state;
        UIManager.Instance.SelectedCatInfoUpdate(cat);
    }
}
