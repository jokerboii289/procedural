using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Interface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Implement test = new Implement();
        test.jump = Gamer;
        DEtect(test,test);
        Temp temp = new Temp();
        temp.A = 9;
        
        print(temp.A);
        Marrigold(temp);
    }
    
    public void Marrigold(IMYinterface on)
    {
        on.Shoot();
    }

    public void DEtect(IMYinterface sad,IMYinterfaceTwo one)
    {
        sad.OnTestEvent += Gamer;

        sad.Damage();
        sad.val = 5;
        print(sad.val);
        one.Shoot();      
    }  

    void Gamer(object obj,EventArgs e)
    {
        print("gamer");
    }
    void Gamer()
    {
        print("gamerbro");
    }
}

public interface IMYinterface:IMYinterfaceTwo //note in interface only three things can be added 1.events 2.properties 3.methods
{
    event EventHandler OnTestEvent;
  
    int val
    {
        get;set;
    }
    void Damage();//default public 
}

public interface IMYinterfaceTwo
{
    void Shoot();
}

public class Implement:IMYinterface,IMYinterfaceTwo
{
    public event EventHandler OnTestEvent; 

    public delegate void Jump();
    public Jump jump;

    public void Damage()
    {
        OnTestEvent?.Invoke(this, EventArgs.Empty);
        jump?.Invoke();
        Debug.Log("chutiya");
    }

    public int val
    {
        get; set;
    }

    public void Shoot()
    {
        Debug.Log("gandu");
    }
}

public class Temp : IMYinterface
{

    private float k;
    private int a;
    public int A
    {
        get { return a; }
        set { a = value; }
    }

    public event EventHandler OnTestEvent;

    public int val
    {
        get; set;
    }
    public void Damage()//default public 
    {

    }

    public void Shoot()
    {
        Debug.Log("Blank"+a);
        
    }
}