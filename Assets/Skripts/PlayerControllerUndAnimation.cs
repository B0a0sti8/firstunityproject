/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float _Speed;

    Vector2 _Movement; // declare variable

    Rigidbody2D _Rigidbody; // get access to rigidbody
    public Animator animator;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnMovement(InputValue value) // OnMovement = On + name of action-input 
    {
        _Movement = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        _Rigidbody.velocity = _Movement * _Speed;
    }

    void Update()
    {
        animator.SetFloat("Horizontal", _Movement.x);
        animator.SetFloat("Vertical", _Movement.y);
        animator.SetFloat("Speed", _Movement.sqrMagnitude);
    }

}
*/
