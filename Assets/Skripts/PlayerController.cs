using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float _Speed;

    Vector2 _Movement; // declare variable

    Rigidbody2D _Rigidbody; // get access to rigidbody
    public Animator animator; // Zugriff auf die Animationen

    PhotonView view;

    public void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Awake() // Awake() runs before Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnMovement(InputValue value) // OnMovement = On + name of action-input 
    {
        if (view.IsMine)
        {
            _Movement = value.Get<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        _Rigidbody.velocity = _Movement * _Speed;
    }

    void Update()       // �ndert Animation je nach Bewegung
    {
        animator.SetFloat("Horizontal", _Movement.x);
        animator.SetFloat("Vertical", _Movement.y);
        animator.SetFloat("SpeedAnim", _Movement.sqrMagnitude);
    }
}