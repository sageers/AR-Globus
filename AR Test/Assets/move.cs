using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    public Vector3 moveTo;

    public void MoveObject()
    {
        transform.localPosition = moveTo;
    }
}
