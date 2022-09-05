using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    [SerializeField] Transform pole;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(45, transform.up);
       // transform.position+= Quaternion.AngleAxis(10,transform.up)*(pole.position - transform.position);
       // print(pole.position - transform.position);
    }
}
