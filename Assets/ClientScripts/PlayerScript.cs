using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidProject;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(1));
        if (Input.GetKey(KeyCode.LeftArrow))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(2));
        if (Input.GetKey(KeyCode.Space))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(3));

    }
}
