using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidProject;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Right,true));
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Left, true));
        if (Input.GetKeyDown(KeyCode.Space))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Jump, true));
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Up, true));
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Down, true));
        if (Input.GetKeyDown(KeyCode.C))
            if (this.gameObject.GetComponent<SpriteRenderer>().flipX)
                ComSim.getInstance().receiveMsgServer(MsgCoder.skillRequest(1, MsgCoder.Direction.Right));
            else
                ComSim.getInstance().receiveMsgServer(MsgCoder.skillRequest(1, MsgCoder.Direction.Left));

        if (Input.GetKeyUp(KeyCode.RightArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Right, false));
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Left, false));
        if (Input.GetKeyUp(KeyCode.UpArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Up, false));
        if (Input.GetKeyUp(KeyCode.DownArrow))
            ComSim.getInstance().receiveMsgServer(MsgCoder.moveRequest(MsgCoder.Direction.Down, false));


        float halfViewport = Camera.main.orthographicSize * Camera.main.aspect;
        if (transform.position.x + halfViewport < 51f && transform.position.x - halfViewport > -51f)
            Camera.main.transform.position = transform.position;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y + 5, -10);
    }
}
