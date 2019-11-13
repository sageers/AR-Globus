using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public void MoveObject()
    {
        transform.Translate(-0.5f,1f,-0.1f);
    }
}
