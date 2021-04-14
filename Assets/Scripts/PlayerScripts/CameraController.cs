using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 referencePos = Vector3.zero;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            referencePos = transform.position;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            cameraOffset = referencePos - transform.position;
        }
    }
}
