using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Player;

public class GravitationalBelt : MonoBehaviour
{
    [SerializeField] List<GameObject> _powerUpsInCollider = new List<GameObject>();

    PowerUps _powerups;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            foreach (GameObject powerup in _powerUpsInCollider)
            {
                _powerups = powerup.GetComponent<PowerUps>();
                _powerups.ActivateGravityForce();
                Debug.Log("activated gravity pull");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PowerUp")
        {
            _powerUpsInCollider.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _powerUpsInCollider.Remove(other.gameObject);
    }
}
