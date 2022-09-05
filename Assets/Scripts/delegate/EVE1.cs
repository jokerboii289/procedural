using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVE1 : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        EVE.OnFire += Eve_OnFire;        

    }

    private void Eve_OnFire(object sender, EVE.ONfire e)
    {
        print("event called"+ e.ok);
    }
}
