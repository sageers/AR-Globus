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
   
    private float step;
    
    private void Start()
    {
        MoveAllObjects.objectsOnEarth.Add(gameObject);
        initalPosition = transform.localPosition;
        step = speed * Time.deltaTime;
    }

   

    private void Update()
    {
        if (MoveAllObjects.move)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveToPosition, step);
        }
        
       
        
    }
}
