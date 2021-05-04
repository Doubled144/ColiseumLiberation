using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuClassBase : MonoBehaviour
{
    //Private
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("Animation Speed", 1f);
    }


}