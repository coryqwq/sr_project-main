using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 referencePos = Vector3.zero;
    public bool startFollow = false;

    public float elapsedTime = 0f;
    public float duration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        //set reference position to player's position at the instance trigger enters wall collider
        //and player has already entered the camera follow collider
        if (other.CompareTag("Wall") && startFollow)
        {
            referencePos = transform.position;
        }
        //set camera to follow player after entering camera follow collider
        if (other.gameObject.name == "CameraFollowTrigger")
        {
            startFollow = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //calculate camera follow offset
        if (other.CompareTag("Wall") && startFollow)
        {
            cameraOffset = referencePos - transform.position;
        }
    }
}
