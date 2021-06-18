using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 referencePos = Vector3.zero;
    public bool startFollowX = false;
    public bool startFollowY = true;
    public float elapsedTime = 0f;
    public float duration = 3f;

    public bool displayMessage;

    private void OnTriggerEnter(Collider other)
    {
        //set reference position to player's position at the instance trigger enters wall collider
        //and player has already entered the camera follow collider
        if (other.CompareTag("Wall") && startFollowX)
        {
            referencePos = transform.position;
        }
        //set camera to follow player after entering camera follow collider
        if (other.gameObject.name == "CameraFollowTrigger")
        {
            startFollowX = true;
        }

        if (other.gameObject.name == "CreditsDialogueTrigger")
        {
            displayMessage = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //calculate camera follow offset
        if (other.CompareTag("Wall") && startFollowX)
        {
            cameraOffset = referencePos - transform.position;
        }
    }
}
