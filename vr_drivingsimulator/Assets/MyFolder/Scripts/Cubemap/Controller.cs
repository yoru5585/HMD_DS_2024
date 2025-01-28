using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class Controller : MonoBehaviour
{
    [SerializeField] Transform ovrCameraTrans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void test()
    {
        Vector2 axis_pri = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 axis_sec = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        ovrCameraTrans.position += new Vector3(axis_pri.x, axis_sec.y, axis_pri.y);
    }

    
}
