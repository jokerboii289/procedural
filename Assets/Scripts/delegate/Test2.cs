
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class Test2 : MonoBehaviour
{
    private static int m=0; // normal members cant be called on nonstatic method;
    int jk;

    //public static event Action<>
    public delegate void Salim();
    public Salim salim;


    
    public static Action<int , int > action2;

    public static Func<int, float> testFunc;

    public Action action;
    //public static Func<void> lol;     //func doesnt take void as return type
    //float here is return type testfunc

    // Start is called before the first frame update
    void Start()
    {
        //salim = new Salim(Ok);
        //salim = () => { print("gandu"); };
        salim = delegate () { print("sahska"); };

        action = Ok;
    }

    // Update is called once per frame
    void Update()
    {
        var temp = m;
        //action2(1,2);
        //salim();
        action();
        if(testFunc!=null)
            testFunc(11);
        if(Time.time>3)
        {
            action = NO; 
        }
    }

    void Ok()
    {
        print("cat");
    }

    void NO()
    { 
        print("dog");
    }

    public static void blegh( int k)
    {
        m += k;
        print(m);
    }
     
    public void G(Action action)
    {
        this.action= action;
    }
}
   