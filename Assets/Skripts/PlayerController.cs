using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : NetworkBehaviour
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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner) { Debug.Log("Thou are not the owner"); return; }

        //[SerializeField]
        PlayerStartPosition[] positions = GameObject.Find("PlayerSpawnPositions").GetComponentsInChildren<PlayerStartPosition>();
        Debug.Log("Finding Start Position");
        foreach (PlayerStartPosition pos in positions)
        {
            Debug.Log(pos);
            //if (pos.hasSpawned)
            //{
            pos.hasSpawned = false;
            transform.position = pos.transform.position;
            Debug.Log("Setting Player Position");
            break;
            //}
        }
    }


    private void Awake() // Awake() runs before Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        speed = GetComponent<PlayerStats>().movementSpeed;
        rotationMeasurement = transform.Find("RotationMeasurement");
        upDirection.z = 1;
    }

    public void Movement(InputValue value) // OnMovement = On + name of action-input 
    {
        if (IsOwner)
        {
            movement = value.Get<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return; }

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
        rotationMeasurement.eulerAngles = new Vector3 (0, 0, GetAngleFromVectorFloat(currentDirectionTrue));
    }

    void Update()       // Ändert Animation je nach Bewegung
    {
        if (!IsOwner) { return; }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("SpeedAnim", movement.sqrMagnitude);
    }

    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}