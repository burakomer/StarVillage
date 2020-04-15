using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testweapon : MonoBehaviour
{
    public Animator animator;

    PlayerControls controls;

    private void Start()
    {
        controls = new PlayerControls();

        controls.Controls.Attack.performed += ctx => animator.SetTrigger("attack");
    }

    public void CancelAttack()
    {
        animator.SetTrigger("idle");
    }
}
