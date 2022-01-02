using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public Stat speed;

    Vector2 movement; // declare variable

    Rigidbody2D _Rigidbody; // get access to rigidbody
    public Animator animator; // Zugriff auf die Animationen

    PhotonView view;



    private void Awake() // Awake() runs before Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        view = GetComponent<PhotonView>();
        speed = GetComponent<PlayerStats>().movementSpeed;
    }

    private void OnMovement(InputValue value) // OnMovement = On + name of action-input 
    {
        if (view.IsMine)
        {
            movement = value.Get<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        speed = GetComponent<PlayerStats>().movementSpeed;
        _Rigidbody.velocity = movement * speed.GetValue();
    }

    void Update()       // Ändert Animation je nach Bewegung
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("SpeedAnim", movement.sqrMagnitude);
    }
}