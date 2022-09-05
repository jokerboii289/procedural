using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{

    [SerializeField] Transform[] rayPoint;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.up*.1f, Color.blue);
        if(Physics.Raycast(transform.position,-Vector3.up,out RaycastHit hit,Mathf.Infinity))
        {
            print(hit.transform);
            if (hit.transform.CompareTag("ground"))
            {
                transform.position =hit.point;
            }
        }
    }
} 
