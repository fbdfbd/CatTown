using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatVisualController : MonoBehaviour
{
    [SerializeField]
    private List<Material> catMaterials;

    [SerializeField]
    private SkinnedMeshRenderer targetMeshRenderer;

    [SerializeField]
    private GameObject catBody;

    [SerializeField]
    private Animator animator;


    private void MatChange(string matName)
    {
        Material mat = catMaterials.Find(material => material.name == matName);
        if (mat != null)
        {
            targetMeshRenderer.material = mat;
        }
        else
        {
            Debug.LogWarning($"mat없다 : {matName} ");
        }
    }

    public void RotateRightCat()
    {
        SetCatRotation(150f);
    }

    public void RotateLeftCat()
    {
        SetCatRotation(210f);
    }

    public void RotateCenterCat()
    {
        SetCatRotation(180f);
    }

    public void SetAnim(string name)
    {
        Debug.Log(name);
 
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Work", false);
        animator.SetBool("Sleep", false);

        switch (name)
        {
            case "Walk":
                animator.SetBool("Walk", true);
                break;
            case "Idle":
                animator.SetBool("Idle", true);
                break;
            case "Run":
                animator.SetBool("Run", true);
                break;
            case "Work":
                animator.SetBool("Work", true);
                break;
            case "Sleep":
                animator.SetBool("Sleep", true);
                break;
            default:
                Debug.LogWarning($"없는 애니메이션 {name}");
                break;
        }
    }

    private void SetCatRotation(float yRotation)
    {
        if (catBody != null)
        {
            Vector3 rotation = catBody.transform.eulerAngles;
            rotation.y = yRotation;
            catBody.transform.eulerAngles = rotation;
        }
        else
        {
            Debug.LogWarning("catBody가 설정되지 않았습니다.");
        }
    }

    public void SetCatMaterials(int catSP)
    {
        if (catSP >= 70) MatChange("Happy");
        else if (catSP >= 50) MatChange("Idle");
        else if (catSP >= 35) MatChange("Angry");
        else MatChange("Crying");
    }
}
