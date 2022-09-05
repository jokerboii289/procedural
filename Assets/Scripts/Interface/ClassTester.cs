using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClassTester : MonoBehaviour
{
    public float variable=6;
    AbstarctClass state;
    Content test = new Content();
    // Start is called before the first frame update
    void Start()
    {
        state = test;
    }

    // Update is called once per frame
    void Update()
    {
        if(state!=null)
            state.Gamer();
    }

    public abstract void jojo();
    

    
}
