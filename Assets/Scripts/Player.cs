using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    float _speedBoostMultiplier = 1.5f;
    float _sprintSpeed = 3f;

    private float _xBound = 11.3f;
    private Vector3 _offset = new Vector3(0, 1f, 0);

    [SerializeField] float _fireRate = 0.2f;
    float _canFire = -1f;

    [SerializeField] int _shieldHP;
    [SerializeField] int _lives = 3;
    [SerializeField] int _currentHP;
    int _score;
    int _startingAmmo = 15;
    [SerializeField] int _currentAmmoCount;
    [SerializeField] int _theBoomAmmoCount;
    [SerializeField] int _maxFuel = 100;
    [SerializeField] int _currentFuel;

    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _laserClip;
    AudioSource _audioSource;


    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] GameObject _theBoom;
    [SerializeField] GameObject _shield;
    [SerializeField]
    GameObject[] _visualDamage;

    [SerializeField] bool _isTripeShotActive = false;
    [SerializeField] bool _isSpeedBoosted = false;
    [SerializeField] bool _isShieldActive = false;
    [SerializeField] bool _theBoomActive = false;

    WaitForSeconds _powerUpCoolDownTimer = new WaitForSeconds(5f);

    SpawnManager _spawnManager;
    UIManager _uiManager;
    GameManager _gameManager;

    SpriteRenderer _shieldColor;



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

        _currentFuel = _maxFuel;
        _currentAmmoCount = _startingAmmo;
        _currentHP = _lives;
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




        if (Input.GetKey(KeyCode.LeftShift) && _isSpeedBoosted == false)
        {
            transform.Translate(direction * sprintSpeed * Time.deltaTime);
            _currentFuel-= 10 * (int)Time.deltaTime;
        }
        else if (_isSpeedBoosted == true)
        {
            transform.Translate(direction * boostSpeed * Time.deltaTime);
        }
        else if (_isSpeedBoosted == false && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= _xBound)
        {
            transform.position = new Vector3(-_xBound, transform.position.y, 0);
        }
        else if (transform.position.x <= -_xBound)
        {
            transform.position = new Vector3(_xBound, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripeShotActive == false && _currentAmmoCount >= 1)
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
            _audioSource.PlayOneShot(_laserClip);
            _currentAmmoCount--;
        }
        else if (_isTripeShotActive == true && _currentAmmoCount >= 3)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserClip);
            _currentAmmoCount -= 3;
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

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldHP--;
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
            else if (_shieldHP == 0)
            {
                _isShieldActive = false;
                _shield.SetActive(false);
            }
        }
        else
        {
            _currentHP--;
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
        }

        if (_currentHP < 1)
        {
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position, 1f);
            _gameManager.GameOver();
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
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


    //IEnumerator ThrusterUse()
    //{

    //}
}




/*while you are using the thrusters the thrusters will be counting down from 100 and when they reach 0 you will not 
 * be able to use the thrusters until they reach 100 again. but if you use it and it doesnt reach 0 then you will be able 
 * to use it again. when it reaches 0 it will be overloaded which is why you cant use it until its at 100 again. the ui
 * will show the thruster percentage only while its being used and its charging but may just be on the whole time*/