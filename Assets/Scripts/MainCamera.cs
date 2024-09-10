using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if(_animator == null)
        {
            Debug.LogError("Animator is null");
        }
    }

    public void CameraShake()
    {
        _animator.SetTrigger("Camera Shake");
    }
}
