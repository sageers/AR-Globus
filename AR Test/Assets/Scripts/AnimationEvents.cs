using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void ToggleAnimation()
    {
        anim.SetBool("toggleAnim", !anim.GetBool("toggleAnim"));
    }
}
