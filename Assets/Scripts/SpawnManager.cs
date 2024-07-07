using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    WaitForSeconds _waitTime = new WaitForSeconds(5f);

    [SerializeField] GameObject _enemyContainer;

    bool _onPlayerDeath = false;
   

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    
    void Update()
    {

    }

    IEnumerator SpawnEnemies()
    {
        while (_onPlayerDeath == false)
        {
            Vector3 _spawnPos = new Vector3(Random.Range(-9.4f, 9.4f), 8, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, _spawnPos, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return _waitTime;
        }
    }

    public void OnPlayerDeath()
    {
        _onPlayerDeath = true;
    }
}

