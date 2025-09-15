using System.Collections;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [SerializeField]
    private Cat cat;
    [SerializeField]
    private CatVisualController catVisualController;
    [SerializeField]
    private CatController catController;
    [SerializeField]
    private CatSound catSound;
    private Coroutine currentMovementCoroutine;
    private Vector3 targetPos;
    private GameObject workObject;

    public void MoveToState(CatState state)
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        switch (state)
        {
            case CatState.Work:
                currentMovementCoroutine = StartCoroutine(MoveToWork());
                break;
            case CatState.Rest:
                currentMovementCoroutine = StartCoroutine(MoveToRest());
                break;
            case CatState.Idle:
                currentMovementCoroutine = StartCoroutine(MoveToIdle());
                break;
            case CatState.Battle:
                currentMovementCoroutine = StartCoroutine(MoveToBattle());
                break;
        }
    }

    private IEnumerator MoveToWork()
    {
        PrepareForMovement("Run");

        workObject = WorkManager.Instance.WorkMoveCat();
        Debug.Log($"일하러 간 곳: {workObject.name}, x좌표: {workObject.transform.position.x}");
        yield return MoveToPosition(workObject.transform.position);
        catVisualController.RotateCenterCat();
        StartCoroutine(FinalizeMovement("Work", 15f));
    }

    private IEnumerator MoveToRest()
    {
        yield return new WaitForSeconds(0.1f);

        catVisualController.SetAnim("Walk");
        catSound.PlayCatMeowSFX();
        catVisualController.SetAnim("Sleep");

        StartCoroutine(catController.CatResting());

        catController.CatCycleStart();
        catVisualController.SetCatMaterials(cat.CatSp);
    }

    private IEnumerator MoveToIdle()
    {
        PrepareForMovement("Walk");
        SetRandomPos();
        yield return MoveToPosition(targetPos);
        catVisualController.RotateCenterCat();
        StartCoroutine(FinalizeMovement("Idle", Random.Range(2f, 5f)));
    }

    private IEnumerator MoveToBattle()
    {
        yield return null;
    }

    private void PrepareForMovement(string animationState)
    {
        catVisualController.SetAnim(animationState);
        catSound.PlayCatStepSFX(true);
    }

    private IEnumerator FinalizeMovement(string animationState, float delay)
    {
        catSound.PlayCatStepSFX(false);
        catVisualController.SetAnim(animationState);

        yield return new WaitForSeconds(delay);

        if(workObject != null)
        {
            WorkManager.Instance.WorkingComplete(workObject);
            catController.CatWorkCompleteToState();
        }  

        workObject = null;
        catController.CatCycleStart();
        catVisualController.SetCatMaterials(cat.CatSp);
    }

    private void SetRandomPos()
    {
        float randomX = Random.Range(-100f, 100f);

        if (transform.position.x >= 140)
            randomX = Random.Range(-80, -30);
        else if (transform.position.x <= -140)
            randomX = Random.Range(30, 80);

        float targetZ = randomX < 0 ? -6f : 0f;
        targetPos = new Vector3(randomX + transform.position.x, transform.position.y, targetZ);
    }

    private IEnumerator MoveToPosition(Vector3 destination)
    {
        SetRotation(destination);
        yield return MoveToTarget(destination);
        catSound.PlayCatMeowSFX();
    }

    private void SetRotation(Vector3 destination)
    {
        if (destination.x > transform.position.x)
            catVisualController.RotateRightCat();
        else
            catVisualController.RotateLeftCat();
    }

    private IEnumerator MoveToTarget(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, cat.CatMoveSpeed * Time.deltaTime * 3f);
            yield return null;
        }
        transform.position = destination;
    }
}
