using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;

   
    
    void Start()
    {
     
    }

    
    void Update()
    {
        float _randomX = Random.Range(-9.4f, 9.4f);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6)
        {
            transform.position = new Vector3(_randomX, 8, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            Destroy(gameObject);
        }
        else if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }
}
