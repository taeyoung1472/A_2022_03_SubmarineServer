using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController characterController;
    public Transform shootOrigin;
    public float gravity = -9.81f;
    public float speed = 2f;
    public float jumpSpeed = 2.5f;
    public float throwForce = 600f;
    public float health;
    public float maxHealth = 100f;

    private bool[] inputs;
    private float yVelocity = 0;
    public int itemAmount = 0;
    public int maxItemAmount = 3;
    [SerializeField] private Rigidbody submarineRb;

    bool isCanMove = true;
    public bool IsCanMove { get { return isCanMove; } set { isCanMove = value; } }

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        speed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
        submarineRb = NetworkManager.Instance.Submarine.GetComponent<Rigidbody>();
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if(health <= 0)
        {
            return;
        }
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }
        Move(_inputDirection);
    }
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= speed;

        if (characterController.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity += jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;

        characterController.Move(_moveDirection);
        ServerSend.PlayerPositionAndRotation(this, _inputDirection);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        if (isCanMove)
        {
            inputs = _inputs;
            transform.rotation = _rotation;
        }
        else
        {
            inputs = new bool[5];
        }
    }
}
