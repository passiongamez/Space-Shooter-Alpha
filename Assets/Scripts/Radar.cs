using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    Enemy _enemy;


    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();

        if( _enemy == null)
        {
            Debug.LogError("Enemy is null");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PowerUp")
        {
            _enemy.DestroyPowerUp();
        }
    }
}
