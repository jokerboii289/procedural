using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spider : MonoBehaviour
{
    [Header(" raypoints")]
    public Transform[] rayPoints;
    [SerializeField]
    private Vector3[] rayPointPosition=new Vector3[4];

    [Header("footHolds")]
    public Transform[] targets;//foots
    private Vector3[] targetPosition= new Vector3[4];
    private int step ;
    private Vector3 bodyPos;
   
    public float speed;

    private Rigidbody rbody;

    private Action state;

    private bool once;
    // Start is called before the first frame update
    void Start()
    {
        step = 3;
        state = Grounded;
        rbody = GetComponent<Rigidbody>();
        once = false;

        foreach (Transform x in targets)
            x.SetParent(null);

        bodyPos = transform.position;
    }


    private void Update()
    {       
        Ray ray = new Ray(transform.position,-transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 1))
        {
            if (hit.distance < .15f)
            {
                if (!once)
                    state = Grounded;
               
                once = true;
            }
            else
                once = false;
        }
        //stick legs to ground
        Legs();
    }
    private void FixedUpdate()
    {
        state?.Invoke();
    }


    void Grounded()
    {
        rbody.velocity = transform.forward * speed;
    }


    void Legs()
    {
        BodyPos();
        BodyRotation();
        //for targets        
        Debug.DrawRay(transform.position, -Vector3.up * .1f, Color.blue);
        int index = 0;
        foreach (Transform x in targets)
        {
            if (Physics.Raycast(x.position+new Vector3(0,2,0), -x.up, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("ground"))
                {
                    x.position = hit.point;
                    targetPosition[index] = hit.point; //current position
                }
            }
            index++;
        }

        //for raypoint
        int count = 0;
        foreach (Transform x in rayPoints)
        {
            if (Physics.Raycast(x.position, -x.up, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("ground"))
                {
                    rayPointPosition[count] = hit.point;
                }
            }
            count++;
        }

        //alternate steps
        for (int i = 0; i < targets.Length; i++)
        {
            if ((rayPointPosition[i] - targets[i].position).magnitude > .25f)
            {
                if (step % 2 == 0 && i <= 1)
                {
                    // targets[i].position = rayPointPosition[i];
                    StartCoroutine(DelayMovement(targets[i], rayPointPosition[i]));
                }
                else if (step % 2 != 0 && i > 1)
                {
                    //targets[i].position = rayPointPosition[i];
                    StartCoroutine(DelayMovement(targets[i], rayPointPosition[i]));
                }
            }
        }

        //body orientation
        var vector1 = targets[0].position - targets[1].position;
        var vector2= targets[2].position - targets[3].position;

        // normal vector from cross product
        var result= Vector3.Cross(vector1, vector2);
        transform.rotation *= Quaternion.FromToRotation(transform.up, result);
    }

    IEnumerator DelayMovement(Transform target, Vector3 pos)
    {
        for (int i = 1; i <= 5; i++)
            target.position = Vector3.Lerp(target.position, pos, i/5);
        yield return new WaitForFixedUpdate();
    }


    void BodyPos()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("ground"))
            {
                if ((hit.point - bodyPos).magnitude > .25f)
                {
                    bodyPos = hit.point;
                    step++;
                }
            }
        }
    }

    //move legs when body rotation
    void BodyRotation()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if ((rayPointPosition[i] - targets[i].position).magnitude > .25f )
            {
                var vector1 = rayPointPosition[i] - bodyPos;
                var vector2 = targets[i].position - bodyPos;
                var angle = Vector3.Angle(vector1,vector2);
            }
        }
    }
}
