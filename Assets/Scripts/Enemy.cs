using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;

    Animator _enemyAnim;

    Collider2D _enemyCollider;

    Player _player;

    [SerializeField] GameObject _laserPrefab;

    [SerializeField] AudioClip _explosionClip;
    AudioSource _audioSource;

    float _fireRate = 3f;

    float _canFire = -1f;

    bool _fireOn = true;



   
    
    void Start()
    {
     _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is null");
        }

        _enemyAnim = GetComponent<Animator>();
       
        _enemyCollider = GetComponent<Collider2D>();

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("audio source is null");
        }
    }

    
    void Update()
    {
        CalculateMovement();
        FireLaser();
    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            float _randomX = Random.Range(-9.4f, 9.4f);

            transform.position = new Vector3(_randomX, 8, 0);
        }
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
