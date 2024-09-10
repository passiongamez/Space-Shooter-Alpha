using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCannon : MonoBehaviour
{
    Player _player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player = other.GetComponent<Player>();

            if( _player != null)
            {
                _player.Damage(6);
            }
        }
    }

}
