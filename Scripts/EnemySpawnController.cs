using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefab;
    [SerializeField] List<Transform> spawnPoints;
    List<GameObject> enemies = new List<GameObject>();
    [SerializeField] int enemiesToWin = 10;
    void SpawnEnemy()
    {
        enemiesToWin--;
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomIndex];
        randomIndex = Random.Range(0, enemyPrefab.Count);
        GameObject newEnemy = Instantiate(enemyPrefab[randomIndex], spawnPoint.position, spawnPoint.rotation);
        enemies.Add(newEnemy);
    }

    bool LastEnemyDestroyed()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                return false;
        }

        return true;
    }
    void Update()
    {
        if (LastEnemyDestroyed())
        {
            if(enemiesToWin < 0)
                Menu.PlayerWin();
            else
                SpawnEnemy();
        }
    }
}
