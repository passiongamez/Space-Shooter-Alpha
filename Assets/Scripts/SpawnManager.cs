using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] _enemies;

    WaitForSeconds _waitTime = new WaitForSeconds(2f);
    WaitForSeconds _waveTime = new WaitForSeconds(3f);
    WaitForSeconds _destroyerWait = new WaitForSeconds(3);

    [SerializeField] GameObject _enemyContainer;

    [SerializeField] GameObject[] _powerUps;
    [SerializeField] GameObject _boss;

    [SerializeField] int _waves = 0;
    [SerializeField] int _enemiesToSpawn = 1;
    [SerializeField] int _enemiesSpawned;
    public int[] percent = { 21, 19, 15, 14, 12, 8, 6, 5 };
    [SerializeField] int _totalOfPercent;
    [SerializeField] int _randomNumber;
    public int[] enemyChance = { 50, 30, 20 };
    [SerializeField] int _totalOfEnemyChance;
    [SerializeField] int _randomNumber2;


    bool _onPlayerDeath = false;
   [SerializeField] bool _movesRight = false;
   [SerializeField] bool _movesLeft = false;

    UIManager _uiManager;
    Destroyer _destroyer;

    private void Start()
    {
        _uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("Ui manager is null");
        }

        _destroyerWait = new WaitForSeconds(Random.Range(3, 7));
        foreach(int percent in percent)
        {
            _totalOfPercent += percent;
        }

        foreach(int enemyChance in enemyChance)
        {
            _totalOfEnemyChance += enemyChance;
        }
    }


    public void StartSpawning()
    {
        StartCoroutine(StartWaves());
        StartCoroutine(SpawnPowerUps());
    }

    public void OnPlayerDeath()
    {
        _onPlayerDeath = true;
    }

    IEnumerator SpawnPowerUps()
    {
        yield return _waitTime;
        while (_onPlayerDeath == false)
        {
            WaitForSeconds randomSpawnTime = new WaitForSeconds(Random.Range(3, 8));
            Vector3 spawnPos = new Vector3(Random.Range(-12f, 12f), 11, 0);

            yield return randomSpawnTime;
            _randomNumber = Random.Range(0, _totalOfPercent);

            for(int i = 0; i < percent.Length; i++)
            {
               if(_randomNumber <= percent[i])
                {
                    Instantiate(_powerUps[i], spawnPos, Quaternion.identity);
                    break;
                }
                else
                {
                    _randomNumber -= percent[i];
                }
            }           
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
            _enemiesSpawned = 0;
            _uiManager.UpdateWave(_waves);
            if(_waves % 5 == 0)
            {
                Vector3 bossSpawnPos = new Vector3(0, 16, 0);
                _enemiesToSpawn = _waves;

                GameObject boss = Instantiate(_boss, bossSpawnPos, Quaternion.identity);
                boss.transform.parent = _enemyContainer.transform;

                while (_enemiesSpawned < _enemiesToSpawn)
                {
                    _randomNumber2 = Random.Range(0, _totalOfEnemyChance);
                    for (int i = 0; i < enemyChance.Length; i++)
                    {
                        if (_randomNumber2 <= enemyChance[i])
                        {
                            if (_movesRight == true)
                            {
                                float xBounds = 15.5f;
                                float yBounds = Random.Range(1, 8);
                                Vector3 spawnPos = new Vector3(-xBounds, yBounds, 0);

                                GameObject newEnemy = Instantiate(_enemies[i], spawnPos, Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;
                            }
                            else if (_movesLeft == true)
                            {
                                float xBounds = 15.5f;
                                float yBounds = Random.Range(1, 8);
                                Vector3 spawnPos = new Vector3(xBounds, yBounds, 0);

                                GameObject newEnemy = Instantiate(_enemies[i], spawnPos, Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;
                            }
                            else if (i == 2)
                            {
                                float yBound = Random.Range(1, 7);
                                Vector3 startPos1 = new Vector3(15f, yBound, 0);
                                Vector3 startPos2 = new Vector3(-15f, yBound, 0);
                                Vector3[] startPos = { startPos1, startPos2 };

                                GameObject newEnemy = Instantiate(_enemies[i], startPos[Random.Range(0, 2)], Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;
                            }
                            else
                            {
                                Vector3 spawnPos = new Vector3(Random.Range(-15f, 15f), 11, 0);
                                GameObject newEnemy = Instantiate(_enemies[i], spawnPos, Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;

                            }
                            break;
                        }
                        else
                        {
                            _randomNumber2 -= enemyChance[i];
                        }
                    }
                    _enemiesSpawned++;
                    yield return _waitTime;
                }
            }
            else
            {
                _enemiesToSpawn = _enemiesToSpawn + Random.Range(2, 5);
                while (_enemiesSpawned < _enemiesToSpawn)
                {
                    _randomNumber2 = Random.Range(0, _totalOfEnemyChance);
                    for (int i = 0; i < enemyChance.Length; i++)
                    {
                        if (_randomNumber2 <= enemyChance[i])
                        {
                            if (_movesRight == true)
                            {
                                float xBounds = 15.5f;
                                float yBounds = Random.Range(1, 8);
                                Vector3 spawnPos = new Vector3(-xBounds, yBounds, 0);

                                GameObject newEnemy = Instantiate(_enemies[i], spawnPos, Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;
                            }
                            else if (_movesLeft == true)
                            {
                                float xBounds = 15.5f;
                                float yBounds = Random.Range(1, 8);
                                Vector3 spawnPos = new Vector3(xBounds, yBounds, 0);

                                GameObject newEnemy = Instantiate(_enemies[i], spawnPos, Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;
                            }
                            else if (i == 2)
                            {
                                float yBound = Random.Range(1, 7);
                                Vector3 startPos1 = new Vector3(15f, yBound, 0);
                                Vector3 startPos2 = new Vector3(-15f, yBound, 0);
                                Vector3[] startPos = { startPos1, startPos2 };

                                GameObject newEnemy = Instantiate(_enemies[i], startPos[Random.Range(0, 2)], Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;
                            }
                            else
                            {
                                Vector3 spawnPos = new Vector3(Random.Range(-15f, 15f), 11, 0);
                                GameObject newEnemy = Instantiate(_enemies[i], spawnPos, Quaternion.identity);
                                newEnemy.transform.parent = _enemyContainer.transform;

                            }
                            break;
                        }
                        else
                        {
                            _randomNumber2 -= enemyChance[i];
                        }
                    }
                    _enemiesSpawned++;
                    yield return _waitTime;
                }
            }
            while (_enemyContainer.transform.childCount > 0)
            {
                yield return null;
            }
        }
    }

    public void AmmoSpawn()
    {
        StartCoroutine(SpawnAmmo());
    }

    IEnumerator SpawnAmmo()
    {
        while(_onPlayerDeath == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-12f, 12f), 11, 0);
            Instantiate(_powerUps[3], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(10f);
            break;
        }
    }
}




