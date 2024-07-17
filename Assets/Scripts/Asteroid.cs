using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _speed = 20f;

    Collider2D _collider2D;

    [SerializeField] GameObject _explosionEffect;

    SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _collider2D = gameObject.GetComponent<Collider2D>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * _speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            _speed = 0;
            _collider2D.enabled = false;
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, .7f);
        }
    }


}
