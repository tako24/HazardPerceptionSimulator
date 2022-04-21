using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [SerializeField] protected List<Transform> pathPoints = new List<Transform>();

    public virtual Transform GetPathPoint(int index)
    {
        if (index == pathPoints.Count)
            return null;
        return pathPoints[index];
    }

    public void UpdatePathPointsList()
    {
        pathPoints.Clear();
        pathPoints = GetComponentsInChildren<PathPoint>().Select(point => point.transform).ToList();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count < 2)
            return;

        for (int i = 1; i < pathPoints.Count; i++)
        {
            if (pathPoints[i - 1] == null)
            {
                pathPoints.RemoveAt(i - 1);
                return;
            }

            if (pathPoints[i] == null)
            {
                pathPoints.RemoveAt(i);
                return;
            }

            Gizmos.DrawLine(pathPoints[i - 1].position, pathPoints[i].position);
        }
    }
#endif
}
