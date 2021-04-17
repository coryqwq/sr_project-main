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
    private void FixedUpdate()
    {
        //camera follows player
        if (cameraControllerScript.startFollow)
        {
            Vector3 position = Vector3.Lerp(transform.position, (player.transform.position + new Vector3(cameraControllerScript.cameraOffset.x, 0, 0) + offset), speed * Time.deltaTime);
            transform.position = new Vector3(position.x, position.y * 0.5f, position.z);
        }
    }
}
