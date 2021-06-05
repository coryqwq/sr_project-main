using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;

    public float speed = 1f;

    public float idleTimeMin = 1f;
    public float idleTimeMax = 3f;
    public float idleTime = 0f;

    public float walkDistanceMin = 3f;
    public float walkDistanceMax = 5f;
    public float walkDistance = 5f;

    public float referencePos = 0f;
    public float direction = 1f;

    public bool startIdle = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GetNextValues();
    }

    void GetNextValues()
    {
        //get current position at idle
        referencePos = transform.position.x;
        //generate random idle time
        idleTime = Random.Range(idleTimeMin, idleTimeMax);
        //generate random walk distance
        walkDistance = Random.Range(walkDistanceMin, walkDistanceMax);

        //generate random direction
        if (Random.value >= 0.5f)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - referencePos) <= walkDistance && startIdle)
        {
            rb.velocity = new Vector3(direction, 0, 0) * speed;
            if(direction == 1)
            {
                GetComponent<SpriteRenderer>().flipX = true;

            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else if(startIdle)
        {
            rb.velocity = Vector3.zero;
            StartCoroutine(StartIdlePhase());
            startIdle = false;
        }
    }

    IEnumerator StartIdlePhase()
    {
        GetNextValues();
        yield return new WaitForSeconds(idleTime);
        startIdle = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "LeftEnemyDirectionTrigger")
        {
            direction = 1;
        }
        if (other.gameObject.name == "RightnemyDirectionTrigger")
        {
            direction = -1;
        }
    }
}
