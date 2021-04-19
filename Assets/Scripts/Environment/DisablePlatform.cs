using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatform : MonoBehaviour
{
    Rigidbody rb;
    public GameObject[] platform;
    private float elapsed = 0;
    private float duration = 0;
    private bool flag0 = false;
    private bool flag1 = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        elapsed += Time.deltaTime;
        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && Mathf.Abs(rb.velocity.x) < 0.1f && flag1)
        {
            DisablePlatforms();
            duration = elapsed + 0.2f;
            flag0 = true;
            
        }
        if(elapsed > duration && flag0 && flag1)
        {
            EnablePlatforms();
            flag0 = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("PlatformTrigger"))
        {
            DisablePlatforms();
        }
        flag1 = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlatformTrigger"))
        {
            EnablePlatforms();
        }
        flag1 = true;
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
