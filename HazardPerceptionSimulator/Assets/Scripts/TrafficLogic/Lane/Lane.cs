using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [SerializeField] protected List<Transform> pathPoints = new List<Transform>();
    [field: SerializeField] public bool isFarRightLane { get; private set; } = true;
    [field: SerializeField] public bool isFarLeftLane { get; private set; } = true;

    private float laneLength = 0f;

    public float LaneLength { get => laneLength; }

    public virtual Transform GetPathPoint(int index)
    {
        if (index == pathPoints.Count)
            return null;
        return pathPoints[index];
    }

    public Transform GetPathPoint(float path)
    {
        float laneLength = 0;
        for (int i = 1; i < pathPoints.Count; i++)
        {
            laneLength += Vector3.Distance(pathPoints[i].position, pathPoints[i - 1].position);

            if (laneLength >= path)
                return pathPoints[i];
        }
        return pathPoints[pathPoints.Count - 1];
    }
    
    public float GetPathByPointOnCollider(Collider collider, Vector3 point)
    {
        float laneLength = 0;
        Transform endColliderPoint = GetPathPoint(System.Convert.ToInt32(collider.name.Split('-')[1]));
        for (int i = 1; i < pathPoints.Count; i++)
        {
            if (pathPoints[i] == endColliderPoint)
            {
                point.y = pathPoints[i - 1].position.y;
                laneLength += Vector3.Distance(point, pathPoints[i - 1].position);
                break;
            }

            laneLength += Vector3.Distance(pathPoints[i].position, pathPoints[i - 1].position);
        }
        if (laneLength > this.laneLength)
            laneLength = this.laneLength;
        return laneLength;
    }

    public Vector3 GetEndSwitchTrafficLanePosition(float path)
    {
        float laneLength = 0;
        for (int i = 1; i < pathPoints.Count; i++)
        {
            float distance = Vector3.Distance(pathPoints[i].position, pathPoints[i - 1].position);

            if (laneLength + distance >= path)
            {
                var e = (pathPoints[i].position - pathPoints[i - 1].position).normalized * path;
                return pathPoints[i - 1].position + (pathPoints[i].position - pathPoints[i - 1].position).normalized * path;
            }
            laneLength += distance;
        }
        return pathPoints[pathPoints.Count - 1].position;
    }

    public void UpdatePathPointsList()
    {
        pathPoints.Clear();
        laneLength = 0;
        pathPoints = GetComponentsInChildren<PathPoint>().Select(point => point.transform).ToList();
        for (int i = 1; i < pathPoints.Count; i++)
            laneLength += Vector3.Distance(pathPoints[i - 1].position, pathPoints[i].position);
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
