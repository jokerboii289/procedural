using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EVE : MonoBehaviour
{  
    public Transform p1, p2;
    public GameObject p3;
    public static event EventHandler<ONfire> OnFire;
   
    public class ONfire:EventArgs  //for creating custom parameters make a class of it and derive it from Eventargs
    {
        public float ok;
    }
    private void Start()
    {
        OnFire?.Invoke(this, new ONfire{ok=6});
    }

    private void Update()
    {
        MidPoint();
    }
            
    void MidPoint()
    {
        var x = (p1.position.x+p2.position.x)/2;
        var y = (p1.position.y + p2.position.y) / 2;
        var z = (p1.position.z + p2.position.z) / 2;

        var point = new Vector3(x,y,z);
       //print(point);
        p3.transform.position = point;
    }
}