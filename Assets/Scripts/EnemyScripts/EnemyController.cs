using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private SpriteRenderer sr;
    PlayerStats playerStatsScript;

    public float speed = 1f;

    public float idleTimeMin = 1f;
    public float idleTimeMax = 3f;
    public float idleTime = 0f;

    public float walkDistanceMin = 3f;
    public float walkDistanceMax = 5f;
    public float walkDistance = 5f;

    public float pushForce = 3f;

    public float referencePos = 0f;
    public float direction = 1f;

    public bool startIdle = true;

    public bool flag0 = true;
    public bool flag1 = true;
    public bool flag2 = true;

    public int hp = 10;

    public float elapsedTime = 0f;
    public float duration = 1f;

    PlayerController playerControllerScript;

    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(0, 7, true);
        playerControllerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0);
        sr.sortingOrder = Random.Range(0, 2);
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
        //player pushes enemy on hit
        if (anim.GetBool("hit") && flag0)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.right * playerControllerScript.direction * pushForce, ForceMode.VelocityChange);
            flag0 = false;

        }
        else
        {
            flag0 = true;
        }

        //enemy walks for the walk distance
        if (Mathf.Abs(transform.position.x - referencePos) <= walkDistance && startIdle && !anim.GetCurrentAnimatorStateInfo(0).IsTag("death"))
        {
            if (!anim.GetBool("hit"))
            {
                rb.velocity = new Vector3(direction, 0, 0) * speed;
            }

            //flip sprite depending on direction
            if (direction == 1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (direction == -1)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            anim.SetBool("walk", true);

        }
        //enemy idle 
        else if (startIdle && !anim.GetCurrentAnimatorStateInfo(0).IsTag("death"))
        {
            rb.velocity = Vector3.zero;
            StartCoroutine(StartIdlePhase());
            startIdle = false;
        }

        //enemy death
        if (hp <= 0 && !anim.GetBool("hit") && flag1)
        {
            anim.SetTrigger("death");
            flag1 = false;
        }
        //enemy fade out
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("death"))
        {
            elapsedTime += Time.deltaTime;
            sr.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime / anim.GetCurrentAnimatorStateInfo(0).length);
            ps.startColor = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime / anim.GetCurrentAnimatorStateInfo(0).length);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
            {
                GameObject.Destroy(gameObject);
            }
        }

        //enemy fade in
        if (elapsedTime < duration && flag2)
        {
            elapsedTime += Time.deltaTime;
            sr.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(1, 1, 1, 1), elapsedTime / duration);

            if (sr.color == new Color(1, 1, 1, 1))
            {
                elapsedTime = 0;
                flag2 = false;
            }
        }


    }

    IEnumerator StartIdlePhase()
    {
        anim.SetBool("walk", false);
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
        if (other.gameObject.name == "RightEnemyDirectionTrigger")
        {
            direction = -1;
        }
        if (other.gameObject.name == "SwordCollider" && !anim.GetCurrentAnimatorStateInfo(0).IsTag("death"))
        {
            anim.SetBool("hit", true);
            hp--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SwordCollider")
        {
            if (PlayerPrefs.GetInt("PlayerMP") < 100)
            {
                PlayerPrefs.SetInt("PlayerMP", PlayerPrefs.GetInt("PlayerMP") + 10);
            }
            ps.startLifetime = 0.4f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SwordCollider")
        {
            ps.startLifetime = 0.0f;
            anim.SetBool("hit", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>());
        }
    }
}
