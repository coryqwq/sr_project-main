using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatform : MonoBehaviour
{
    Rigidbody rb;
    public GameObject[] platform;
    private float elapsed = 0;
    private float duration = 0;
    private bool flag = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        elapsed += Time.deltaTime;
        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            DisablePlatforms();
            duration = elapsed + 0.2f;
            flag = true;
            
        }
        if(elapsed > duration && flag)
        {
            EnablePlatforms();
            flag = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("PlatformTrigger"))
        {
            DisablePlatforms();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlatformTrigger"))
        {
            EnablePlatforms();
        }
    }

    void DisablePlatforms()
    {
        for (int i = 0; i < platform.Length; i++)
        {
            platform[i].GetComponent<BoxCollider>().enabled = false;
        }
    }
    void EnablePlatforms()
    {
        for (int i = 0; i < platform.Length; i++)
        {
            platform[i].GetComponent<BoxCollider>().enabled = true;
        }
    }
}
