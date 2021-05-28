﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeArduino : MonoBehaviour
{
    public Vector3 Position;
    // Start is called before the first frame update
    void Start()
    {
        Position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Position = new Vector3(Position.x + Input.GetAxis("Horizontal") * 15.0f * Time.deltaTime, Position.y + Input.GetAxis("Vertical") * 15.0f * Time.deltaTime, 0.0f);
        Position = new Vector3(Mathf.Clamp(Position.x, -17.0f, 17.0f), Mathf.Clamp(Position.y, -17.0f, 17.0f), 0.0f);
    }
}
