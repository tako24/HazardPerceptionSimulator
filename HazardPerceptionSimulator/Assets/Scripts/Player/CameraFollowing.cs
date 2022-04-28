using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;

    private void Start()
    {
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
    }

    private void LateUpdate()
    {
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
    }
}
