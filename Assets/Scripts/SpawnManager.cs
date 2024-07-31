using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;

    WaitForSeconds _waitTime = new WaitForSeconds(5f);
    WaitForSeconds _rareSpawnTime = new WaitForSeconds(10f);

    [SerializeField] GameObject _enemyContainer;

    [SerializeField] GameObject[] _powerUps;
    

    bool _onPlayerDeath = false;

   

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUps());
        StartCoroutine(RareSpawns());
    }


    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3f);
        while (_onPlayerDeath == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9.4f, 9.4f), 8, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return _waitTime;
        }
    }

    public void OnPlayerDeath()
    {
        _onPlayerDeath = true;
    }

    IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(3f);
        while (_onPlayerDeath == false)
        {
            WaitForSeconds randomSpawnTime = new WaitForSeconds(Random.Range(3, 8));
            Vector3 spawnPos = new Vector3(Random.Range(-9.4f, 9.4f), 8, 0);
            int randomPowerUps = Random.Range(0, 5);

            yield return randomSpawnTime;
            Instantiate(_powerUps[randomPowerUps], spawnPos, Quaternion.identity);
        }
    }

    IEnumerator RareSpawns()
    {
        while(_onPlayerDeath == false)
        {
            WaitForSeconds rareSpawnTime = new WaitForSeconds(Random.Range(10f, 15f));
            Vector3 spawnPos = new Vector3(Random.Range(-9.4f, 9.4f), 8, 0);

            yield return rareSpawnTime;
            Instantiate(_powerUps[5], spawnPos, Quaternion.identity);
        }
    }
}

