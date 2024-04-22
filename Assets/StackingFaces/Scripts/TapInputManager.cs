using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TapInputManager : MonoBehaviour
{

    public event Action<Vector2> Pressed;
    public event Action<Vector2> Dragged;
    public event Action<Vector2> Released;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
