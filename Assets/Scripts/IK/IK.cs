using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IK : MonoBehaviour
{
  
    //length
    public int chainLength;


    //target
    public Transform Target;

    [Range(0, 1)]
    public float SnapBackStrength = 1f;

    [SerializeField]
    protected float[] BonesLength;//target to origin
    protected float completeLength;
    [SerializeField]
    protected Transform[] Bones;
    [SerializeField]
    protected Vector3[] Positions;

    protected Vector3[] StartDirectionSucc;
    protected Quaternion[] StartRotationBone;
    protected Quaternion StartRotationTarget;
    protected Quaternion startRotationRoot;


    [Header("solver parameter")]
    public int Iterations = 10;

    //distance when the solver stops
    public float delta = 0.001f;

    [Header("POLE")]
    public Transform Pole;

    private void Awake()
    {
        Init();
    }
    
    void Init()
    {
        //initial array
        Bones = new Transform[chainLength + 1];
        Positions = new Vector3[chainLength + 1];
        BonesLength = new float[chainLength];
        StartDirectionSucc = new Vector3[chainLength+1];
        StartRotationBone = new Quaternion[chainLength+1];


        if(Target==null)
        {
            Target = new GameObject(gameObject.name + "Target").transform; // craeting target if it doesnt exist
            Target.position = transform.position; //set taget pos
        }
        StartRotationTarget = Target.rotation;

         
        completeLength = 0;

        //init data
        var current = transform;
        for (var i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            StartRotationBone[i] = current.rotation;

            if(i==Bones.Length-1)//leaf bone or last bone
            {
                StartDirectionSucc[i] = Target.position - current.position;
            }
            else
            {
                StartDirectionSucc[i] = Bones[i + 1].position - current.position;//direction of bones
                BonesLength[i] = (Bones[i + 1].position - current.position).magnitude; // storing lenght of the bones
                completeLength += BonesLength[i];
            }
            
            current = current.parent;
        }
    }

    private void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (Target == null)
            return;
        if (BonesLength.Length != chainLength)
            Init();

        //get position
        for (int i = 0; i < Bones.Length; i++)
            Positions[i] = Bones[i].position;

        var RootRot = (Bones[0].parent!=null)?Bones[0].parent.rotation:Quaternion.identity;
        var RootRotDiff = RootRot * Quaternion.Inverse(startRotationRoot);

        //calculation //possible to reach
        if((Target.position-Bones[0].position).sqrMagnitude>=completeLength*completeLength)
        {
            //just strech it
            var direction = (Target.position - Positions[0]).normalized;

            //set everything after root
            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
        }
        else
        {
            for(int iteration=0;iteration<Iterations;iteration++)
            {
                //back
                for(int i=Positions.Length-1;i>0;i--)
                {
                    if (i == Positions.Length - 1)
                        Positions[i] = Target.position;//set it to target
                    else
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i];//set in line on distance
                }

                //forward
                for (int i = 1; i < Positions.Length; i++)
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i-1];

                //close enough to target?
                if ((Positions[Positions.Length - 1] - Target.position).sqrMagnitude < delta * delta)
                    break;
            }
        }

        //move towards pole
        if(Pole!=null)
        {
            for(int i=1;i<Positions.Length-1;i++)
            {
                var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(Pole.position); //projected point
                var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];//rotating the plane normal to angle calculated on previous line            
            }
        }



        //set position  & rotation
        for (int i = 0; i < Positions.Length; i++)

        {
            if (i == Positions.Length - 1)
                Bones[i].rotation = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];
            else
                Bones[i].rotation = Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * StartRotationBone[i];

            Bones[i].position = Positions[i];
        }
    }

    private void OnDrawGizmos()
    {
        var current = this.transform;
        for(int i=0;i<chainLength && current.parent!=null;i++)
        {
            var scale = Vector3.Distance(current.position,current.parent.position)*0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position,Quaternion.FromToRotation(Vector3.up,current.parent.position-current.position),new Vector3(scale,Vector3.Distance(current.parent.position,current.position),scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * .5f, Vector3.one);
            current = current.parent;
        }   
    }
}
