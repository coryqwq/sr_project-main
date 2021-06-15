using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    CameraController cameraControllerScript;
    public float speed = 1f;
    public Vector3 offset;
    private void Start()
    {
        cameraControllerScript = player.GetComponent<CameraController>();
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elasped = 0.0f;
        cameraControllerScript.startFollowY = false;

        while (elasped < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elasped += Time.deltaTime;

            yield return null;


        }
        cameraControllerScript.startFollowY = true;


    }

    private void FixedUpdate()
    {
        //camera follows player past camera trigger
        if (cameraControllerScript.startFollowX)
        {
            Vector3 position = Vector3.Lerp(transform.position, (player.transform.position + new Vector3(cameraControllerScript.cameraOffset.x, 0, 0) + offset), speed * Time.deltaTime);
            transform.position = new Vector3(position.x, position.y * 0.5f, position.z);
        }
        //camera follows players y postion, before the camera trigger
        else if(cameraControllerScript.startFollowY)
        {
            Vector3 position = Vector3.Lerp(transform.position, (new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z) + offset), speed * Time.deltaTime);
            transform.position = new Vector3(position.x, position.y * 0.5f, position.z);
        }
    }
}
