using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    float _speed = 10;

    [SerializeField] GameObject _explosion;
    GameObject _closestTarget;

    float _closestDistance;

    [SerializeField] bool _isPlayerMissile = false;

    Player _player;

    void Start()
    {
        _closestDistance = 100f;

        FindTarget();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(_isPlayerMissile == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else
        {
            ChaseTheEnemy();
        }

        if(Mathf.Abs(transform.position.y) >= 10)
        {
            Destroy(gameObject);
        }
        if(Mathf.Abs(transform.position.x) >= 15)
        {
            Destroy(gameObject);
        }
    }

    public void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemies)
        {
           Vector3 targetPos = enemy.transform.position;
           float distance = Vector3.Distance(targetPos, transform.position);
            if(distance < _closestDistance)
            {
                _closestDistance = distance;
                _closestTarget = enemy;
            }
        }
    }

    void ChaseTheEnemy()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if(_closestTarget != null)
        {
            Vector3 target = _closestTarget.transform.position;
            target.x = target.x - transform.position.x;
            target.y = target.y - transform.position.y;

            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isPlayerMissile == false)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.Damage(2);
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    public void IsPlayerMissile()
    {
        _isPlayerMissile = true;
    }

    public bool PlayerMissile()
    {
        return _isPlayerMissile;
    }
}
