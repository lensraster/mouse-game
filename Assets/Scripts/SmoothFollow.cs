using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 1f;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //   Vector3 targetPos = Vector3.Lerp(transform.position, target.position, smoothSpeed);
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothSpeed);
        transform.position = targetPos;
    }
}
