using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public Stat speed;

    public Vector2 movement; // declare variable
    public Vector3 currentDirectionTrue = Vector3.zero;
    private Vector3 currentDirection1 = Vector3.zero;
    private Vector3 currentDirection2 = Vector3.zero;
    private Vector3 currentDirection3 = Vector3.zero;
    private Vector3 currentDirection4 = Vector3.zero;
    public Vector3 upDirection = Vector3.zero;

    public Transform rotationMeasurement;

    Rigidbody2D _Rigidbody; // get access to rigidbody
    public Animator animator; // Zugriff auf die Animationen

    public PhotonView view;



    private void Awake() // Awake() runs before Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        view = GetComponent<PhotonView>();
        speed = GetComponent<PlayerStats>().movementSpeed;
        rotationMeasurement = transform.Find("RotationMeasurement");
        upDirection.z = 1;
    }

    public void Movement(InputValue value) // OnMovement = On + name of action-input 
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
        if (movement != Vector2.zero)
        {
            currentDirection4 = currentDirection3;
            currentDirection3 = currentDirection2;
            currentDirection2 = currentDirection1;
            currentDirection1 = movement;

            if ((Mathf.Abs(currentDirection1.x) < 1  && Mathf.Abs(currentDirection1.y) < 1) || (Mathf.Abs(currentDirection2.x) < 1 && Mathf.Abs(currentDirection2.y) < 1) || (Mathf.Abs(currentDirection3.x) < 1 && Mathf.Abs(currentDirection3.y) < 1) || (Mathf.Abs(currentDirection4.x) < 1 && Mathf.Abs(currentDirection4.y) < 1)) 
            {
                currentDirectionTrue = currentDirection4;
            }
            else
            {
                currentDirectionTrue = currentDirection1;
            }
        }
        rotationMeasurement.rotation = Quaternion.LookRotation(currentDirectionTrue);
    }

    void Update()       // ?ndert Animation je nach Bewegung
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("SpeedAnim", movement.sqrMagnitude);
    }
}