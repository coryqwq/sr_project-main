using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0, 2, -10), Time.deltaTime * speed);
    }
}
