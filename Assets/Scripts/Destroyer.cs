using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    int _maxHealth = 5;
    [SerializeField] int _currentHealth;

    [SerializeField] float _speed = 7f;
    float _fireRate = 5f;
    float _canFire = -1f;

    Vector3 _offset = new Vector3(0, -1.5f, 0);


    [SerializeField] GameObject _explosion;
    [SerializeField] GameObject _missile;

    Collider2D _collider;

    [SerializeField] bool _movingLeft;
    bool _fireOn = true;

    Movement _values;


    SpawnManager _spawnManager;
    Player _player;

    public enum Movement
    {
        RightToLeft,
        LeftToRight
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if ( _player == null)
        {
            Debug.LogError("Player is null");
        }

        _collider = GetComponent<Collider2D>();
        if( _collider == null)
        {
            Debug.LogError("Collider is null");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if( _spawnManager == null)
        {
            Debug.LogError("Spawn manager is null");
        }

        _values = (Movement)Random.Range(0, 3);

     _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireMissile();
    }

    void CalculateMovement()
    {
        if (_movingLeft == false)
        {
            LeftToRight();
        }
        else
        {
            RightToLeft();
        }
    }

    void RightToLeft()
    {
        transform.Translate((Vector3.left) * _speed * Time.deltaTime);
        if(transform.position.x < -10.3f)
        {
            _movingLeft = false;
        }
    }

    void LeftToRight()
    {
        transform.Translate((Vector3.right) * _speed * Time.deltaTime);
        if (transform.position.x > 10.3f)
        {
            _movingLeft = true;
        }
    }

    void FireMissile()
    {
        if (Time.time > _canFire && _fireOn == true)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_missile, transform.position + _offset, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Player":
                _player.Damage(1);
                _currentHealth--;
                break;
            case "Laser":
                Destroy(other.gameObject);
                _currentHealth--;
                break;
            case "The Boom":
                Destroy(other.gameObject);
                _currentHealth -= 5;
                break;
            case "Enemy Laser":
                return;
            default:
                break;
        }
        if (other.tag == "Missile")
        {
            Missile missile = other.GetComponent<Missile>();
            if (missile != null && missile.PlayerMissile() == true)
            {
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _player.AddScore(50);
                _speed = 0;
                _collider.enabled = false;
                _fireOn = false;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }

        if (_currentHealth <= 0)
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _player.AddScore(50);
            _speed = 0;
            _collider.enabled = false;
            _fireOn = false;
            Destroy(gameObject, 1f);
        }
    }
}
