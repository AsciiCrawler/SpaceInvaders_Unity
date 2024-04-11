using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject laser;

    public InputAction xAxis;
    public InputAction fire;

    void Start()
    {
        this.xAxis.Enable();
        this.fire.Enable();
    }

    void Update()
    {
        float xAxisValue = this.xAxis.ReadValue<float>();
        this.transform.position += new Vector3(xAxisValue * speed * Time.deltaTime, 0.0f, 0.0f);
        if(this.fire.triggered)
            Instantiate(this.laser, this.transform.position, this.transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.gameManager.StartGame();
    }
}