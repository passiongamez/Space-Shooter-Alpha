using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private float xBound = 11.3f;
    private Vector3 _offset = new Vector3(-0, 1f, 0);
    [SerializeField] float _fireRate = 0.2f;
    float _canFire = -1f;

    [SerializeField] GameObject _laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, 0);
        }
        else if (transform.position.x <= -xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, 0);
        }
    }
}
