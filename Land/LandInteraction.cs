using UnityEngine;

public class LandInteraction : MonoBehaviour
{
    private Land land;

    private void Start()
    {
        land = GetComponent<Land>();
        if (land != null)
        {
            land.OnClicked += HandleLandClicked;
        }
    }

    private void OnDestroy()
    {
        if (land != null)
        {
            land.OnClicked -= HandleLandClicked;
        }
    }

    private void HandleLandClicked(Land land)
    {
        Debug.Log($"Å¬¸¯µÈ Land: {land.name}");
        UIManager.Instance.SelectLand(land);
    }

}
