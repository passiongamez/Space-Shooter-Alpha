using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Laser : MonoBehaviour
{
    [SerializeField] float _speed = 8f;

    [SerializeField] bool _isEnemyLaser = false;
    [SerializeField] bool _isBackFire = false;

    Player _player;

    void Start()
    {
       _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is null");
        }
    }

  
    void Update()
    {
      if(_isEnemyLaser == false || _isBackFire == true)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {        
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 10)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(gameObject);
            }      
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }      
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void BackFiring()
    {
        _isBackFire = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(_isEnemyLaser == true && other.tag == "Player")
        {
            _player.Damage(1);
            Destroy(gameObject);
        }   
        if(_isEnemyLaser == true && other.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
