using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    [SerializeField] private Rigidbody map;
    [SerializeField] private float speed;
    [Range(0.05f, 1f)]
    [SerializeField] private float rotSpeed;
    public static Submarine Instance;
    float moveInput;
    float yAxisRot;
    float xAxisRot;

    private bool[] inputs;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        inputs = new bool[6];
    }
    public void FixedUpdate()
    {
        moveInput = 0;
        yAxisRot = 0;
        xAxisRot = 0;
        if (inputs[0])
        {
            moveInput += 1;
        }
        if (inputs[1])
        {
            moveInput -= 1;
        }
        if (inputs[2])
        {
            yAxisRot -= 1;
        }
        if (inputs[3])
        {
            yAxisRot += 1;
        }
        if (inputs[4])
        {
            xAxisRot -= 1;
        }
        if (inputs[5])
        {
            xAxisRot += 1;
        }
        Move();
    }
    public void Move()
    {
        map.velocity = transform.forward * moveInput * -speed;
        map.rotation = map.rotation * Quaternion.Euler(-xAxisRot * rotSpeed, -yAxisRot * rotSpeed, 0);
        ServerSend.MapPositionAndRotation(map.transform);
    }
    public void SetInput(bool[] _inputs)
    {
        inputs = _inputs;
    }
}
