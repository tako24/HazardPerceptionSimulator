using System.Linq;
using UnityEngine;

public class TrafficLane : Lane
{
    [Header("Colliders creation")]
    [SerializeField] private Transform collidersHolder;
    [SerializeField] private string trafficLaneCollidersTag = "TrafficLane";

    protected virtual void Awake()
    {
        if (pathPoints == null || pathPoints.Count < 2)
            return;
        gameObject.GetComponentsInChildren<BoxCollider>().ToList().ForEach(boxCollider => Destroy(boxCollider));
        CreateLaneColliders();
    }

    private void CreateLaneColliders()
    {
        for (int i = 1; i < pathPoints.Count; i++)
        {
            Transform collider = new GameObject($"{i - 1}-{i}").transform;
            collider.parent = collidersHolder;
            collider.tag = trafficLaneCollidersTag;

            float distance = Vector3.Distance(pathPoints[i - 1].position, pathPoints[i].position);
            Vector3 normalize = Vector3.Normalize(pathPoints[i].position - pathPoints[i - 1].position);

            BoxCollider boxCollider = collider.gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(distance, 1f, 1f);
            boxCollider.isTrigger = true;

            collider.position = pathPoints[i - 1].position + normalize * distance / 2;
            collider.rotation = Quaternion.LookRotation(pathPoints[i].position - collider.position);
            collider.Rotate(Vector3.up, 90f);
            collider.position += Vector3.up;
        }
    }

    // первый вариант с коллайдерами. Лежит тут на всякий случай

    //[SerializeField] [Range(0f, 1f)] private float startLaneColliderLenghtCoefficient = 0.6f;
    //[SerializeField] [Range(0f, 1f)] private float endLaneColliderLenghtCoefficient = 0.1f;

    /*private void CreateStartLaneCollider(Transform startLanePoint, Transform connectedPoint)
    {
        Transform collider = new GameObject($"Collider - Lane start").transform;
        collider.parent = collidersHolder;
        collider.tag = "LaneStart";

        float distance = Vector3.Distance(startLanePoint.position, connectedPoint.position);
        Vector3 normalize = Vector3.Normalize(connectedPoint.position - startLanePoint.position);

        BoxCollider boxCollider = collider.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(distance * startLaneColliderLenghtCoefficient, 2f, 3f);
        boxCollider.isTrigger = true;

        collider.rotation = Quaternion.LookRotation(connectedPoint.position - startLanePoint.position);
        collider.Rotate(Vector3.up, 90f);
        collider.position = startLanePoint.position + normalize * boxCollider.size.x / 2;
        collider.position += Vector3.up;
    }

    private void CreateEndLaneCollider(Transform endLanePoint, Transform connectedPoint)
    {
        Transform collider = new GameObject($"Collider - Lane end").transform;
        collider.parent = collidersHolder;
        collider.tag = "LaneEnd";

        float distance = Vector3.Distance(endLanePoint.position, connectedPoint.position);
        Vector3 normalize = Vector3.Normalize(connectedPoint.position - endLanePoint.position);

        BoxCollider boxCollider = collider.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(distance * endLaneColliderLenghtCoefficient, 2f, 3f);
        boxCollider.isTrigger = true;

        collider.rotation = Quaternion.LookRotation(connectedPoint.position - endLanePoint.position);
        collider.Rotate(Vector3.up, 90f);
        collider.position = endLanePoint.position + normalize * boxCollider.size.x / 2;
        collider.position += Vector3.up;
    }*/
}
