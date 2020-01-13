using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAllObjects : MonoBehaviour
{

    
    public static List<GameObject> objectsOnEarth = new List<GameObject>();

    public static bool move;

    public void toggleObjects()
    {
        move = !move;
    }

//    public void showAllObjects()
//    {
//        
//        
//        foreach (var movableObject in objectsOnEarth)
//        {
//            movableObject.GetComponent<move>().MoveObject();
//        }
//    }
    
}
