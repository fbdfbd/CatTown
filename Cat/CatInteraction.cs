using System.Collections;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    private Coroutine nameTagCoroutine;
    private const float NameTagDelay = 1f;

    private void OnEnable()
    {
        CatController catController = GetComponent<CatController>();
        if (catController != null)
        {
            catController.OnClicked += HandleCatClicked;
            catController.OnMouseOver += HandleCatMouseOver;
            catController.OnMouseExitEvent += HandleCatMouseExit;
        }
        else
        {
            Debug.LogError("고양이없음");
        }
    }

    private void OnDisable()
    {
        CatController catController = GetComponent<CatController>();
        if (catController != null)
        {
            catController.OnClicked -= HandleCatClicked;
            catController.OnMouseOver -= HandleCatMouseOver;
            catController.OnMouseExitEvent -= HandleCatMouseExit;
        }
    }

    private void HandleCatClicked(Cat cat)
    {
        UIManager.Instance.SelectCat(cat);
    }

    private void HandleCatMouseOver(Cat cat)
    {
        if (nameTagCoroutine != null)
        {
            StopCoroutine(nameTagCoroutine);
        }
        nameTagCoroutine = StartCoroutine(ShowNameTagAfterDelay(cat));
    }

    private void HandleCatMouseExit(Cat cat)
    {
        if (nameTagCoroutine != null)
        {
            StopCoroutine(nameTagCoroutine);
            nameTagCoroutine = null;
        }
        if (cat.CatNameTag != null)
        {
            cat.CatNameTag.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowNameTagAfterDelay(Cat cat)
    {
        yield return new WaitForSeconds(NameTagDelay);
        if (cat.CatNameTag != null)
        {
            cat.CatNameTag.gameObject.SetActive(true);
        }
    }
}
