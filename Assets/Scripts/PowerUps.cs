﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] float _speed = 3f;

    [SerializeField] int _powerUpID;

    Player _player;
    GravitationalBelt _gravitationalBelt;

    [SerializeField] AudioClip _powerUpClip;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        _gravitationalBelt = GameObject.Find("Player").GetComponentInChildren<GravitationalBelt>();
        if (_gravitationalBelt == null)
        {
            Debug.LogError("Gravitational belt is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        _player.ActivateTripleShot();
                        break;
                    case 1:
                        _player.SpeedBoost();
                        break;
                    case 2:
                        _player.ActivateShield();
                        break;
                    case 3:
                        _player.AmmoRefill();
                        break;
                    case 4:
                        _player.IncreaseHealth();
                        break;
                    case 5:
                        _player.SecondaryPowerUp();
                        break;
                    case 6:
                        _player.ActivateHomingMissile();
                        break;
                        case 7:
                        _player.Depower();
                        break;
                    default:
                        Debug.Log("No powerup collected");
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position, 1f);
            Destroy(gameObject);
        }
    }

    public void ActivateGravityForce()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 currentPos = transform.position;

        Vector3.MoveTowards(currentPos, playerPos, (_speed + 3) * Time.deltaTime);
    }
}
