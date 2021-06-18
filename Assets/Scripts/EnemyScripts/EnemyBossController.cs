using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    public GameObject[] attack;
    public int maxHP = 1000;
    public int currentHP = 1000;

    public float attackRate0 = 0.5f;
    public float attackRate1 = 0.1f;
    public float attackRate2 = 10f;

    public Animator anim;

    public int attackCount0 = 0;
    public int attackCount1 = 0;

    public float elapsedTime = 0f;
    public float duration = 5f;

    public RectTransform hpBar;
    public float hpBarMax = 0f;

    public GameObject ps;
    public float particleLifetime = 0.5f;

    public bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = currentHP;
        anim = GetComponent<Animator>();
        hpBarMax = hpBar.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP > 0 
            && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
            && !GetComponent<Animator>().IsInTransition(0)
            && FindObjectOfType<PlayerController>().alive
            && GameObject.Find("LevelTitleText"))
        {
            int randomIndex = Random.Range(0, 3);
            anim.SetTrigger("attack" + randomIndex);

            Invoke("StartAttack" + randomIndex, 3);
        }

        if (attackCount0 > 8)
        {
            CancelInvoke("StartAttack0");
            attackCount0 = 0;
        }

        if (attackCount1 > 8)
        {
            CancelInvoke("StartAttack1");
            attackCount1 = 0;
        }

        if (currentHP <= 0)
        {
            alive = false;
            CancelInvoke();
            ps.SetActive(false);
            elapsedTime += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime / duration);
        }
        if (!FindObjectOfType<PlayerController>().alive)
        {
            CancelInvoke();
        }

        if (currentHP <= maxHP)
        {
            hpBar.sizeDelta = new Vector2((currentHP * (hpBarMax / maxHP)), hpBar.sizeDelta.y);
        }
    }

    void StartAttack0()
    {
        Invoke("StartAttack0", attackRate0);

        Instantiate(attack[0],
                    new Vector3(transform.position.x + Random.Range(-8, 13)
                    , attack[0].transform.position.y + Random.Range(0, 0.4f)
                    , attack[0].transform.position.z)
                    , attack[0].transform.rotation);

        attackCount0++;
    }
    void StartAttack1()
    {
        Invoke("StartAttack1", attackRate1);

        Instantiate(attack[1],
                    new Vector3(transform.position.x + Random.Range(-8, 13)
                    , attack[1].transform.position.y + Random.Range(-0.4f, 0.4f)
                    , attack[1].transform.position.z)
                    , attack[1].transform.rotation);

        attackCount1++;
    }
    void StartAttack2()
    {
        Instantiate(attack[2], attack[2].transform.position, attack[0].transform.rotation);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "SwordCollider")
        {
            currentHP--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SwordCollider")
        {
            if (PlayerPrefs.GetInt("PlayerMP") < 100)
            {
                PlayerPrefs.SetInt("PlayerMP", PlayerPrefs.GetInt("PlayerMP") + 5);
            }
            var main = ps.GetComponent<ParticleSystem>().main;
            main.startLifetime = particleLifetime;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        var main = ps.GetComponent<ParticleSystem>().main;
        main.startLifetime = 0;
    }
}
