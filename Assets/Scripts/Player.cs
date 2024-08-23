using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements.Experimental;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    float _speedBoostMultiplier = 1.5f;
    float _sprintSpeed = 3f;

    private float _xBound = 13.75f;
    private Vector3 _offset = new Vector3(0, 1f, 0);

    [SerializeField] float _fireRate = 0.2f;
    float _canFire = -1f;

    [SerializeField] float _maxFuel = 100;
    [SerializeField] float _currentFuel;

    [SerializeField] int _shieldHP;
    [SerializeField] int _lives = 3;
    [SerializeField] int _currentHP;
    int _score;
    int _startingAmmo = 15;
    [SerializeField] int _currentAmmoCount;
    [SerializeField] int _theBoomAmmoCount;

    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _laserClip;
    AudioSource _audioSource;


    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] GameObject _theBoom;
    [SerializeField] GameObject _shield;
    [SerializeField]
    GameObject[] _visualDamage;
    [SerializeField] GameObject _gravitationalBelt;

    [SerializeField] bool _isTripeShotActive = false;
    [SerializeField] bool _isSpeedBoosted = false;
    [SerializeField] bool _isShieldActive = false;
    [SerializeField] bool _theBoomActive = false;
    [SerializeField] bool _isThrusterActive = false;

    WaitForSeconds _powerUpCoolDownTimer = new WaitForSeconds(5f);

    SpawnManager _spawnManager;
    UIManager _uiManager;
    GameManager _gameManager;
    MainCamera _mainCamera;
    PowerUps _powerups;

    SpriteRenderer _shieldColor;


    public enum PowerUps
    {
        Speed,
        TripleShot,
        TheBoom,
        Shield,
        Ammo
    }



    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is null");
        }

        _uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("Ui manager is null");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }

        _shieldColor = _shield.GetComponent<SpriteRenderer>();
        if (_shieldColor == null)
        {
            Debug.LogError("color is null");
        }

        _mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();

        if(_mainCamera == null)
        {
            Debug.LogError("Main camera is null");
        }


        _currentFuel = _maxFuel;
        _currentAmmoCount = _startingAmmo;
        _currentHP = _lives;
        transform.position = new Vector3(0,0,0);
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if(Input.GetKeyDown(KeyCode.B) && Time.time > _canFire && _theBoomActive == true)
        {
            FireTheBoom();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float currentSpeed = _speed;
        float sprintSpeed = _speed + _sprintSpeed;
        float boostSpeed = (_speed + _sprintSpeed) * _speedBoostMultiplier;


        if(Input.GetKeyDown(KeyCode.LeftShift) && _currentFuel > 0)
        {
            _isThrusterActive = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isThrusterActive = false;
        }


        if (_isSpeedBoosted == true)
        {
            transform.Translate(direction * boostSpeed * Time.deltaTime);
        }
        else if (_isThrusterActive == true)
        {
            transform.Translate(direction * sprintSpeed * Time.deltaTime);
            _currentFuel -= Time.deltaTime * 20f;
        }
        else
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime);
        }

        if (_isThrusterActive == false && _currentFuel < _maxFuel)
        {
            Refuel();
        }


        if (_currentFuel < 0)
        {
            _isThrusterActive = false;
            _currentFuel = 0;
        }
        if (_currentFuel > _maxFuel)
        {
            _currentFuel = _maxFuel;
        }

        _uiManager.FuelUpdate(Mathf.RoundToInt(_currentFuel));

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 1), 0);

        if (transform.position.x >= _xBound)
        {
            transform.position = new Vector3(-_xBound, transform.position.y, 0);
        }
        else if (transform.position.x <= -_xBound)
        {
            transform.position = new Vector3(_xBound, transform.position.y, 0);
        }
    }

    void Refuel()
    {
      _currentFuel += Time.deltaTime * 40f;
    }

    void FireLaser()
    {
        if (_isTripeShotActive == false && _currentAmmoCount >= 1)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
            _audioSource.PlayOneShot(_laserClip);
            _currentAmmoCount--;
        }
        if (_isTripeShotActive == true && _currentAmmoCount >= 3)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserClip);
            _currentAmmoCount -= 3;
        }
        else if(_isTripeShotActive == true && _currentAmmoCount < 3 && _currentAmmoCount > 0)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
            _audioSource.PlayOneShot(_laserClip);
            _currentAmmoCount--;
        }

        if (_currentAmmoCount == 0)
        {
            _spawnManager.AmmoSpawn();
        }
        _uiManager.UpdateAmmoCount(_currentAmmoCount);
    }

    void FireTheBoom()
    {
        _canFire = Time.time + _fireRate;

        if(_theBoomAmmoCount > 0)
        {            
            Instantiate(_theBoom, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserClip);
            _theBoomAmmoCount--;
        }
        if(_theBoomAmmoCount == 0)
        {
            _theBoomActive = false;
        }
        _uiManager.BoomAmmoUpdate(_theBoomAmmoCount);
    }

    public void Damage(int damage)
    {
        if (_isShieldActive == true)
        {
            _shieldHP -= damage;
            if (_shieldHP == 3)
            {
                return;
            }
            else if (_shieldHP == 2)
            {
                _shieldColor.color = Color.yellow;
            }
            else if (_shieldHP == 1)
            {
                _shieldColor.color = Color.red;
            }
            else if (_shieldHP <= 0)
            {
                _shieldHP = 0;
                _isShieldActive = false;
                _shield.SetActive(false);
            }
        }
        else
        {
            _currentHP -= damage;
            _mainCamera.CameraShake();
            _uiManager.UpdateLives(_currentHP);
            if (_currentHP == 2)
            {
                _visualDamage[Random.Range(0, 2)].SetActive(true);
            }

            if (_currentHP == 1 && _visualDamage[0].activeInHierarchy)
            {
                _visualDamage[1].SetActive(true);
            }
            else if (_currentHP == 1 && _visualDamage[1].activeInHierarchy)
            {
                _visualDamage[0].SetActive(true);
            }
            if (_currentHP <= 0)
            {
                _currentHP = 0;
                AudioSource.PlayClipAtPoint(_explosionClip, transform.position, 1f);
                _gameManager.GameOver();
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
            }
        }


    }

    public void ActivateTripleShot()
    {
        _isTripeShotActive = true;
        StartCoroutine(TripleShotCountdown());
    }

    IEnumerator TripleShotCountdown()
    {
        yield return _powerUpCoolDownTimer;
        _isTripeShotActive = false;
    }

    public void SpeedBoost()
    {
        _isSpeedBoosted = true;
        StartCoroutine(SpeedCountdown());
    }

    IEnumerator SpeedCountdown()
    {
        yield return _powerUpCoolDownTimer;
        _isSpeedBoosted = false;

    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
        _shieldHP = 3;
        _shieldColor.color = Color.white;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoRefill()
    {
        _currentAmmoCount = _startingAmmo;
        _uiManager.UpdateAmmoCount(_currentAmmoCount);
    }

    public void IncreaseHealth()
    {
        if(_currentHP < _lives)
        {
            _currentHP++;

            if (_currentHP == _lives)
            {
                _visualDamage[0].SetActive(false);
                _visualDamage[1].SetActive(false);
            }
            else if(_currentHP == 2)
            {
                _visualDamage[Random.Range(0, _visualDamage.Length)].SetActive(false);
            }
        }
        else
        {
            return;
        }
        _uiManager.UpdateLives(_currentHP);
    }


    public void SecondaryPowerUp()
    {
        _theBoomActive = true;
        _theBoomAmmoCount = 3;
        _uiManager.BoomAmmoUpdate(_theBoomAmmoCount);
    }


    public void Depower()
    {
        PowerUps values = (PowerUps)Random.Range(0, 5);
        switch (values)
        {
            case PowerUps.Speed:
                _isSpeedBoosted = false;
                break;
            case PowerUps.TripleShot:
                _isTripeShotActive = false;
                break;
            case PowerUps.TheBoom:
                _theBoomActive = false;
                _theBoomAmmoCount = 0;
                _uiManager.BoomAmmoUpdate(_theBoomAmmoCount);
                break;
            case PowerUps.Shield:
                _isShieldActive = false;
                _shield.SetActive(false);
                break;
            case PowerUps.Ammo:
                if(_currentAmmoCount >= 6)
                {
                    _currentAmmoCount -= 5;
                }
                else
                {
                    _currentAmmoCount = 0;
                }
                _uiManager.UpdateAmmoCount(_currentAmmoCount);
                Debug.Log(values);
                break;
            default:
                return;
        }
    }
}




