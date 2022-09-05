using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Test2 test2;
    // Start is called before the first frame update
    void Start()
    {
        //Test2.salim =()=> { print("hgdgajd"); };
        //Test2.salim = new Test2.Salim(Ok); == ( Test2.salim =Ok;)
        //Test2.action += Draw;      
        //Test2.testFunc = Ok;

        //test2.G(() => { print("kg"); });// this mehtod is sbcribed to action event
    }

    private void Update()
    {
        //Test2.blegh(5);
    }

    void Draw()
    {
        print("bruh");
    }

    void Rami()
    {
        print("changed");
    }

    float Ok(int k)  
    {
        print("_");
        return .5f;       
    }
}
