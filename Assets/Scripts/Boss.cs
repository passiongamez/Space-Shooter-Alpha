using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Vector3 _startPos = new Vector3(0, 16, 0);

    float _speed = 3f;
    float _fireRateLaser = 2f;
    float _missileFireRate = 3f;
    float _canFireLaser = 1f;
    float _canFireMissile = 3f;
    float _canFireCannon = 5f;
    float _bigCannonFireRate = 20f;

    int _maxHP = 50;
    [SerializeField] int _currentHP;

    bool _fireOn = true;

    WaitForSeconds _explosionWaitTime = new WaitForSeconds(1f);
    WaitForSeconds _chargeClip1Wait = new WaitForSeconds(1.6f);
    WaitForSeconds _chargeClip2Wait = new WaitForSeconds(2.9f);

    Collider2D _collider;

    AudioSource _audioSource;

    ParticleSystem _chargeParticles;

    [SerializeField] AudioClip _chargeClip1;
    [SerializeField] AudioClip _chargeClip2;
    [SerializeField] AudioClip _bigCannonFire;


    [SerializeField] GameObject _explosion;
    [SerializeField] GameObject[] _lasers;
    [SerializeField] GameObject _missile;
    [SerializeField] GameObject _bigCannon;

    Player _player;
    Laser _laser;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        if(_collider == null)
        {
            Debug.LogError("Collider is null");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }

        _audioSource = GetComponent<AudioSource>();

        _chargeParticles = GetComponent<ParticleSystem>();
        if(_chargeParticles == null)
        {
            Debug.LogError("particle system is null");
        }

        

        transform.position = _startPos;

        _currentHP = _maxHP;
    }

    void Update()
    {
        if(transform.position.y > 9)
        {
            Movement();
        }

        if(_fireOn == true)
        {
            FireLasers();
            DropMissiles();
        }

        if(Time.time > _canFireCannon && _fireOn == true)
        {
            _bigCannonFireRate = Random.Range(20f, 41f);
            _canFireCannon = Time.time + _bigCannonFireRate;
            StartCoroutine(BigCannon());
        }
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                _currentHP--;
                _player.Damage(1);
                break;
            case "Laser":
                _currentHP--;
                Destroy(other);
                break;
            case "The Boom":
                _currentHP -= 5;
                Destroy(other);
                break;
            case "Missile":
                Missile missile = other.GetComponent<Missile>();
                if (missile != null && missile.PlayerMissile() == true)
                {
                    _currentHP -= 10;
                    Destroy(other);
                }
                else
                {
                    return;
                }
                break;
            default:
                break;
        }

        if(_currentHP <= 0)
        {
            _currentHP = 0;
            _collider.enabled = false;
            _fireOn = false;
            _player.AddScore(100);
            StartCoroutine(ExplosionFinale());
            Destroy(gameObject, 5f);
        }
    }

    IEnumerator ExplosionFinale()
    {
        Vector3[] explosionPoints = { new Vector3(-6, 5, 0), new Vector3(6, 5, 0), new Vector3(-.37f, 6, 0), new Vector3(-5.5f, 6, 0), new Vector3(5.5f, 6, 0) };

        while (true)
        {
            Vector3 randomPoint = explosionPoints[Random.Range(0, explosionPoints.Length)];
            Instantiate(_explosion, randomPoint, Quaternion.identity);
            yield return _explosionWaitTime;
        }
    }

    void FireLasers()
    {
        Vector3 laserPosition1 = new Vector3(4.34f, 2, 0);
        Vector3 laserPosition2 = new Vector3(-5.2f, 2, 0);

        if(Time.time > _canFireLaser)
        {
            _fireRateLaser = Random.Range(2, 4);
            _canFireLaser = Time.time + _fireRateLaser;
            GameObject rightLaser = Instantiate(_lasers[0], laserPosition1, Quaternion.identity);
            GameObject leftLaser = Instantiate(_lasers[0], laserPosition2, Quaternion.identity);
            Laser _laser = rightLaser.GetComponent<Laser>();
            for(int i = 0; i < _lasers.Length; i++)
            {
                _laser.AssignEnemyLaser();
            }
        }
    }

    void DropMissiles()
    {
        Vector3[] spawnDrops = { new Vector3(-5.3f, 6.5f, 0), new Vector3(5.3f, 6.5f, 0) };
        Vector3 randomSpawn = spawnDrops[Random.Range(0, spawnDrops.Length)];

       if(Time.time > _canFireMissile)
        {
            _missileFireRate = Random.Range(5, 11);
            _canFireMissile = Time.time + _missileFireRate;
            Instantiate(_missile, randomSpawn, Quaternion.identity);
        } 
    }

    IEnumerator BigCannon()
    {
        Vector3 bigCannonOffset = new Vector3(-.4f, -9, 0);
        WaitForSeconds bigCannonWait = new WaitForSeconds(Random.Range(20f, 40f));

        _audioSource.PlayOneShot(_chargeClip1, 1f);
        _chargeParticles.Play();
        yield return _chargeClip1Wait;
        _audioSource.PlayOneShot(_chargeClip2, 1f);
        yield return _chargeClip2Wait;
        _chargeParticles.Stop();
        GameObject bigCannon = Instantiate(_bigCannon, transform.position + bigCannonOffset, Quaternion.identity);
        _audioSource.PlayOneShot(_bigCannonFire, 1f);
        Destroy(bigCannon, 5f);
    }
}
