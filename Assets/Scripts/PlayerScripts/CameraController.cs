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

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elasped = 0.0f;
        startFollow = false;
        while (elasped < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elasped += Time.deltaTime;

            yield return null;
        }

        startFollow = true;
    }

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
