using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerSim;
using MidProject;

public class ComSim : MonoBehaviour
{
    // Start is called before the first frame update
    MapInstance map;
    public static ComSim instance;
    public GameObject playerGameObject;
    private GameObject player;
    void Start()
    {
        map = new MapInstance(0);
        instance = this;
        map.addPlayer(new Player());
        player = Instantiate(playerGameObject);
    }
    public void sendMsgToServer(byte[] data)
    {
        receiveMsgClient(data);
    }
    public void receiveMsgServer(byte[] data)
    {
        DataContainer cont = MsgCoder.getDataContainerServer(data);
        //handle msg
    }
    public void sendMsgToClient(byte[] data)
    {
        receiveMsgServer(data);
    }
    public void receiveMsgClient(byte[] data)
    {
        DataContainer cont = MsgCoder.getDataContainerClient(data);
        switch (cont.requestType)
        {
            default:
                break;
        }
        //handle msg
    }
    private void handleMovePlayer(DataContainer data) // CLIENT
    {

    }

}
