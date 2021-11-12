using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float _Speed;

    Vector2 _Movement; // declare variable

    Rigidbody2D _Rigidbody; // get access to rigidbody

    private void Awake() // Awake() runs before Start()
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
}
