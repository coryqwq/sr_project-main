using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    CameraController playerControllerScript;
    public float speed = 1f;
    public Vector3 offset;

    private void Start()
    {
        playerControllerScript = player.GetComponent<CameraController>();
    }
    private void FixedUpdate()
    {
        Vector3 position = Vector3.Lerp(transform.position, (player.transform.position + new Vector3(playerControllerScript.cameraOffset.x, 0, 0) + offset), speed * Time.deltaTime);
        transform.position = new Vector3(position.x, position.y * 0.5f, position.z);
    }
}
