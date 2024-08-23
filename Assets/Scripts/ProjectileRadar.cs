using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRadar : MonoBehaviour
{
    Enemy _enemy;

    public int[] dodgeChance = { 80, 11, 9 };

    int _totalDodgeChance;
    [SerializeField] int _randomDodgeChance;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();

        if (_enemy == null)
        {
            Debug.LogError("enemy is null");
        }

        foreach(int dodgeChance in dodgeChance)
        {
            _totalDodgeChance += dodgeChance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _randomDodgeChance = Random.Range(0, _totalDodgeChance);
            Debug.Log(_randomDodgeChance);
            for (int i = 0; i < dodgeChance.Length; i++)
            {
                if(_randomDodgeChance <= dodgeChance[i])
                {
                    switch (i)
                    {
                        case 0:
                            Debug.Log("return");
                            return;
                        case 1:
                            _enemy.DodgeLeft();
                            Debug.Log("dodge left");
                            break;
                        case 2:
                            _enemy.DodgeRight();
                            Debug.Log("dodge right");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _randomDodgeChance -= dodgeChance[i];
                }                
            }
        }
    }
}
