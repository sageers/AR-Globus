using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class move : MonoBehaviour
{

    public float speed = 0.1f;

    private Vector3 initalPosition;
    public Vector3 moveToPosition;
    public bool Move;
    private float step;
    
    private void Start()
    {
        initalPosition = transform.localPosition;
        step = speed * Time.deltaTime;
    }

    public void MoveObject()
    {
        Move = true;



        //transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.5f, 1, 0), 0.1f);
    }

    private void Update()
    {
        if (Move)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(-0.5f, 1, 0), step);
        }
        
       
        
    }
}
