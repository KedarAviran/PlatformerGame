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
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Right));
        if (Input.GetKey(KeyCode.LeftArrow))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Left));
        if (Input.GetKey(KeyCode.Space))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Jump));
        if (Input.GetKey(KeyCode.UpArrow))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Up));
        if (Input.GetKey(KeyCode.DownArrow))
            ComSim.instance.receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Down));
        Camera.main.transform.position = transform.position - new Vector3(0,0,10);


    }
}
