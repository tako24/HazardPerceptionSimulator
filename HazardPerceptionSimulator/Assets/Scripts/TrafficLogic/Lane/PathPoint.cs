using UnityEngine;

public class PathPoint : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        Lane lane = GetComponentInParent<Lane>();
        if (lane)
            lane.UpdatePathPointsList();
    }
#endif
}
