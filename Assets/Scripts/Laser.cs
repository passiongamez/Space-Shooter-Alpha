using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _speed = 8f;
    

    void Start()
    {
       
    }

  
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if(transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }
}
