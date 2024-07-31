using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TheBoom : MonoBehaviour
{
    float _speed = 3f;

    SpriteShapeRenderer _spriteShapeRenderer;

    [SerializeField] AudioClip _boomExplosion;
    AudioSource _audioSource;

    [SerializeField] GameObject _theBoomExplosion;

    WaitForSeconds _theBoomTimer = new WaitForSeconds(1f);

    Collider2D _collider;


    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        if( _collider != null)
        {
            _collider.enabled = false;
        }

        _audioSource = GetComponent<AudioSource>();
        if( _audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }

        StartCoroutine(TheBoomTimer());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();

            _spriteShapeRenderer.enabled = false;
            Destroy(other.gameObject);
            Destroy(gameObject, 3f);
        }
    }

    IEnumerator TheBoomTimer()
    {
        yield return _theBoomTimer;
        Instantiate(_theBoomExplosion, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_boomExplosion);
        _speed = 0;
        _collider.enabled = true;
    }
}
