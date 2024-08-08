using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;

    [SerializeField] float _circleEndTime;

    Animator _enemyAnim;

    Collider2D _enemyCollider;

    Player _player;

    [SerializeField] GameObject _laserPrefab;

    [SerializeField] AudioClip _explosionClip;
    AudioSource _audioSource;

    float _fireRate = 3f;

    float _canFire = -1f;

    float _time;

    bool _fireOn = true;

    EnemyMovement values;

    SpawnManager _spawnManager;


    public enum EnemyMovement
    {
        StraightDown,
        MoveLeft,
        MoveRight,
        //ZigZag,
        DiagonalLeft,
        DiagonalRight
    }




    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        _enemyAnim = GetComponent<Animator>();

        _enemyCollider = GetComponent<Collider2D>();

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("audio source is null");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

       values = (EnemyMovement)Random.Range(0, 5);
        Debug.Log(values.ToString());
    }


    void Update()
    {
        CalculateMovement();
        FireLaser();
    }


    void CalculateMovement()
    {
        switch (values)
        {
            case EnemyMovement.StraightDown:
                StraightDown();
                break;
            case EnemyMovement.MoveLeft:
                MoveLeft();
                break;
            case EnemyMovement.MoveRight:
                MoveRight();
                break;
            /*case EnemyMovement.ZigZag:
                ZigZag();
                break;*/
            case EnemyMovement.DiagonalLeft:
                DiagonalLeft();
                break;
            case EnemyMovement.DiagonalRight:
                DiagonalRight();
                break;
            default:
                break;
        }



        if (transform.position.y < -6)
        {
            float _randomX = Random.Range(-9.4f, 9.4f);

            transform.position = new Vector3(_randomX, 8, 0);
        }
        if(transform.position.x > 12)
        {
            float randomX = Random.Range(1, 5);

            transform.position = new Vector3(-12, randomX, 0);
        }
        if(transform.position.x < -12)
        {
            float randomX = Random.Range(1, 5);

            transform.position = new Vector3(12, randomX, 0);
        }
    }
    void StraightDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);      
    }

    void MoveLeft()
    {
        _spawnManager.MoveLeft();
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }

    void MoveRight()
    {
        _spawnManager.MoveRight();
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    /*void ZigZag()
    {
        int amplitude = 20;
        float frequency = 5f;
        float x = Mathf.Cos(Time.time);
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        float z = transform.position.z;
        Vector3 movement = new Vector3(x, y, z);
        _circleEndTime = Time.time + 2 * Mathf.PI;


        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y == Random.Range(2, 5))
        {
            transform.position = (movement * Time.deltaTime);
        }
        if(Time.time >= _circleEndTime)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
     


    }*/

    void DiagonalLeft()
    {
        transform.Translate(new Vector3(-.8f, -.8f, 0) * _speed * Time.deltaTime);
    }

    void DiagonalRight()
    {
        transform.Translate(new Vector3(.8f, -.8f, 0) * _speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player": 
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }
                _enemyAnim.SetTrigger("OnEnemyDeath");
                _audioSource.PlayOneShot(_explosionClip);
                _speed = 0;
                _enemyCollider.enabled = false;
                _fireOn = false;
                Destroy(gameObject, 2.3f);
                break;
            case "Laser":
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                _enemyAnim.SetTrigger("OnEnemyDeath");
                _audioSource.PlayOneShot(_explosionClip);
                _speed = 0;
                _enemyCollider.enabled = false;
                _fireOn = false;
                Destroy(gameObject, 2.3f);
                break;
            case "Enemy Lasers":
                return;
            case "The Boom":
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                _enemyAnim.SetTrigger("OnEnemyDeath");
                _audioSource.PlayOneShot(_explosionClip);
                _speed = 0;
                _enemyCollider.enabled = false;
                _fireOn = false;
                Destroy(gameObject, 2.3f);
                break;
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire && _fireOn == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLasers = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLasers.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
}
