using UnityEngine;

[ExecuteAlways]
public class LandSorting : MonoBehaviour
{
    public Vector3 startPosition = Vector3.zero;
    public float spacing = 9.0f;

    private void Start()
    {
        AlignObjectsHorizontally();
    }

    private void AlignObjectsHorizontally()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            Vector3 newPosition = startPosition + new Vector3(count * spacing, 0, 0);
            child.localPosition = newPosition;
            count++;
        }
    }
}
