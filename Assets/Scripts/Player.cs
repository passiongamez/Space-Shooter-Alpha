﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    float _speedBoostMultiplier = 2f;

    private float _xBound = 11.3f;
    private Vector3 _offset = new Vector3(0, 1f, 0);

    [SerializeField] float _fireRate = 0.2f;
    float _canFire = -1f;

    [SerializeField] int _lives = 3;
    int _score;

    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _tripleShotPrefab;
    [SerializeField] GameObject _shield;

    [SerializeField] bool _isTripeShotActive = false;
    [SerializeField] bool _isSpeedBoosted = false;
    [SerializeField] bool _isShieldActive = false;

    WaitForSeconds _powerUpCoolDownTimer = new WaitForSeconds(5f);

    SpawnManager _spawnManager;
    UIManager _uiManager;
    GameManager _gameManager;


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is null");
        }

        _uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

        if(_uiManager == null)
        {
            Debug.LogError("Ui manager is null");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }


    void Update()
    {
     CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
          FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            transform.Translate(direction * _speed * Time.deltaTime);

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

        if(_isTripeShotActive == false)
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }
        else if(_isTripeShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

    }

    public void Damage()
    {
        if(_isShieldActive == true) 
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
        }

        if (_lives < 1)
        {
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
        _speed *= _speedBoostMultiplier;
        StartCoroutine(SpeedCountdown());
    }

    IEnumerator SpeedCountdown()
    {
        yield return _powerUpCoolDownTimer;
        _isSpeedBoosted = false;
        _speed /= _speedBoostMultiplier;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
