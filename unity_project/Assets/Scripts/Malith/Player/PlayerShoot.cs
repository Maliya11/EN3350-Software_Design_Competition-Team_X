using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    PlayerControls controls;
    public Animator animator;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Land.Throw.performed += ctx => Throw();
    }

    void Throw()
    {
        animator.SetTrigger("throw");
    }
}
