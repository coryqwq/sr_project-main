using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] enemy;
    public float[] spawnRate;
    public float[] ySpawnPos;
    public int bound = 6;
    public int maxEnemyCount = 20;
    // Start is called before the first frame update
    void Start()
    {
        //start spawning all enemy types at their respective rates
        for (int i = 0; i < enemy.Length; i++)
        {
            StartCoroutine(SpawnEnemy(enemy[i], spawnRate[i], ySpawnPos[i]));
        }
    }

    IEnumerator SpawnEnemy(GameObject enemy, float spawnRate, float ySpawnPos)
    {
        //loop indefinitely
        while (true)
        {
            //spawn enemies if below enemy count limit
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemyCount)
            {
                Instantiate(enemy, new Vector3(transform.parent.position.x + Random.Range(-bound, bound), ySpawnPos, -0.5f), transform.rotation);
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
