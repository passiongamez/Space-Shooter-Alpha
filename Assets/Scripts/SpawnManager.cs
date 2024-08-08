using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;

    WaitForSeconds _waitTime = new WaitForSeconds(2f);
    WaitForSeconds _rareSpawnTime = new WaitForSeconds(10f);
    WaitForSeconds _waveTime = new WaitForSeconds(3f);

    [SerializeField] GameObject _enemyContainer;

    [SerializeField] GameObject[] _powerUps;

    [SerializeField] int _waves = 0;
    [SerializeField] int _enemiesToSpawn = 5;
    [SerializeField] int _enemiesSpawned;


    bool _onPlayerDeath = false;
    bool _movesRight = false;
    bool _movesLeft = false;


    public void StartSpawning()
    {
        StartCoroutine(StartWaves());
        StartCoroutine(SpawnPowerUps());
        StartCoroutine(RareSpawns());
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
        while (_onPlayerDeath == false)
        {
            WaitForSeconds rareSpawnTime = new WaitForSeconds(Random.Range(10f, 15f));
            Vector3 spawnPos = new Vector3(Random.Range(-9.4f, 9.4f), 8, 0);
            int randomRarePowerups = Random.Range(6, 3);

            yield return rareSpawnTime;
            Instantiate(_powerUps[randomRarePowerups], spawnPos, Quaternion.identity);
        }
    }

    public void MoveRight()
    {
        _movesRight = true;
    }

    public void MoveLeft()
    {
        _movesLeft = true;
    }

    IEnumerator StartWaves()
    {
        while (_onPlayerDeath == false)
        {
            yield return _waveTime;
            _waves++;
            _enemiesToSpawn = _enemiesToSpawn + Random.Range(3, 6);
            _enemiesSpawned = 0;
            //update ui with wave number
            while (_enemiesSpawned < _enemiesToSpawn)
            {
                if (_movesRight == true)
                {
                    float xBounds = 12f;
                    float yBounds = Random.Range(1, 5);
                    Vector3 spawnPos = new Vector3(-xBounds, yBounds, 0);
                    GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                    //yield return _waitTime;
                }
                else if (_movesLeft == true)
                {
                    float xBounds = 12f;
                    float yBounds = Random.Range(1, 5);
                    Vector3 spawnPos = new Vector3(xBounds, yBounds, 0);
                    GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                    //yield return _waitTime;
                }
                else
                {
                    Vector3 spawnPos = new Vector3(Random.Range(-9.4f, 9.4f), 8, 0);
                    GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                    //yield return _waitTime;
                }
                _enemiesSpawned++;
                yield return _waitTime;
            }
            while (_enemyContainer.transform.childCount > 0)
            {
                yield return null;
            }
        }
    }
}





