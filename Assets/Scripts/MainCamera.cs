using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        if(_animator == null)
        {
            Debug.LogError("Animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraShake()
    {
        _animator.SetTrigger("Camera Shake");
    }
}
