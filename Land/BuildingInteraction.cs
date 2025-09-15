using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteraction : MonoBehaviour
{
    private Land thisBuildingLand;

    private void OnMouseDown()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;
        if (thisBuildingLand != null)
        {
            HandleBuildingClicked(thisBuildingLand);
        }
    }

    public void BuildingConnectLand(Land land)
    {
        thisBuildingLand = land;
        Debug.Log(land.name);
    }

    private void HandleBuildingClicked(Land land)
    {
        Debug.Log($"Å¬¸¯µÈ Land: {land.name}");

        UIManager.Instance.SelectLand(land);
    }
}
