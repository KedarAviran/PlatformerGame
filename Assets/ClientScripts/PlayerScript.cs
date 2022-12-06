using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidProject;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
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
        if (Input.GetKey(KeyCode.C))
            if (this.gameObject.GetComponent<SpriteRenderer>().flipX)
                ComSim.instance.receiveMsgServer(MsgCoder.skillRequest(1, MsgCoder.Direction.Right));
            else
                ComSim.instance.receiveMsgServer(MsgCoder.skillRequest(1, MsgCoder.Direction.Left));
        float halfViewport = Camera.main.orthographicSize * Camera.main.aspect;
        if (transform.position.x + halfViewport < 51f && transform.position.x - halfViewport > -51f)
            Camera.main.transform.position = transform.position;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y + 5, -10);
    }
}
