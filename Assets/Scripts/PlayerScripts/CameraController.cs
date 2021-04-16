using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 referencePos = Vector3.zero;
    public bool startFollow = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && startFollow)
        {
            referencePos = transform.position;
        }
        if (other.gameObject.name == "CameraFollowTrigger")
        {
            startFollow = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall") && startFollow)
        {
            cameraOffset = referencePos - transform.position;
        }
    }
}
