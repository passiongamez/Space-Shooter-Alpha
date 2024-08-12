using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _destroyerPrefab;

    WaitForSeconds _waitTime = new WaitForSeconds(2f);
    WaitForSeconds _rareSpawnTime = new WaitForSeconds(10f);
    WaitForSeconds _waveTime = new WaitForSeconds(3f);
    WaitForSeconds _destroyerWait = new WaitForSeconds(3);

    [SerializeField] GameObject _enemyContainer;

    [SerializeField] GameObject[] _powerUps;

    [SerializeField] int _waves = 0;
    [SerializeField] int _enemiesToSpawn = 5;
    [SerializeField] int _enemiesSpawned;
    [SerializeField] int _destroyersToSpawn;
    public int[] percent = { 21, 19, 15, 14, 12, 10, 9 };
    [SerializeField] int total;
    [SerializeField] int _randomNumber;


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
            total += percent;
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
        yield return new WaitForSeconds(3f);
        while (_onPlayerDeath == false)
        {
            WaitForSeconds randomSpawnTime = new WaitForSeconds(Random.Range(3, 8));
            Vector3 spawnPos = new Vector3(Random.Range(-12f, 12f), 11, 0);

            yield return randomSpawnTime;
            _randomNumber = Random.Range(0, total);

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
            _destroyersToSpawn = Random.Range(1, 6);
            _enemiesToSpawn = (_enemiesToSpawn + Random.Range(3, 6)) + _destroyersToSpawn;
            _enemiesSpawned = 0;
            int destroyersSpawned = 0;
            _uiManager.UpdateWave(_waves);
            while (_enemiesSpawned < _enemiesToSpawn)
            {
                if (_movesRight == true)
                {
                    float xBounds = 15.5f;
                    float yBounds = Random.Range(1, 8);
                    Vector3 spawnPos = new Vector3(-xBounds, yBounds, 0);
                    GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                }
                else if (_movesLeft == true)
                {
                    float xBounds = 15.5f;
                    float yBounds = Random.Range(1, 8);
                    Vector3 spawnPos = new Vector3(xBounds, yBounds, 0);
                    GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    Vector3 spawnPos = new Vector3(Random.Range(-15f, 15f), 11, 0);
                    GameObject _newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                }
                _enemiesSpawned++;
                yield return _waitTime;

                while (destroyersSpawned < _destroyersToSpawn)
                {
                    float yBound = Random.Range(1, 7);
                    Vector3 startPos1 = new Vector3(15f, yBound, 0);
                    Vector3 startPos2 = new Vector3(-15f, yBound, 0);
                    Vector3[] startPos = { startPos1, startPos2 };

                    GameObject destroyer = Instantiate(_destroyerPrefab, startPos[Random.Range(0, 2)], Quaternion.identity);
                    destroyer.transform.parent = _enemyContainer.transform;

                    destroyersSpawned++;
                    _enemiesSpawned++;
                    yield return _destroyerWait;
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
            yield return new WaitForSeconds(5f);
            Vector3 spawnPos = new Vector3(Random.Range(-12f, 12f), 11, 0);
            Instantiate(_powerUps[3], spawnPos, Quaternion.identity);
            break;
        }
    }
}





