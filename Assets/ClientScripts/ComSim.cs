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
        map.addPlayer();
    }
    public void receiveMsgServer(byte[] data)
    {
        DataContainer cont = MsgCoder.getDataContainerServer(data);
        switch (cont.requestType)
        {
            case (int)MsgCoder.ClientToServer.moveRequest:
                handleMoveRequest(cont);
                break;
            case (int)MsgCoder.ClientToServer.useSkill:
                handleUseSkill(cont);
                break;
        }
    }
    public void receiveMsgClient(byte[] data)
    {
        DataContainer cont = MsgCoder.getDataContainerClient(data);
        cmds.Add(cont);
    }
    // SERVER
    private void handleMoveRequest(DataContainer data)
    {

    }
    private void handleUseSkill(DataContainer data)
    {

    }
    // CLIENT
    List<DataContainer> cmds = new List<DataContainer>(); 
    private void handleNewLifeOfFigure(DataContainer data)
    {

    }
    private void handleFigureSkill(DataContainer data)
    {

    }
    private void handleNewLocationOfFigure(DataContainer data) 
    {
        player.transform.position = new Vector3(data.floats[0], data.floats[1], 0);
    }
    private void handleNewFigure(DataContainer data)
    {
        player = Instantiate(playerGameObject);
        player.name = "Player ID: " + data.integers[0];
        player.transform.position=new Vector3(data.floats[0], data.floats[1],0);
    }
    public void executeOrders()
    {
        while(cmds.Count>0)
        {
            DataContainer cont = cmds[0];
            switch (cont.requestType)
            {
                case (int)MsgCoder.ServerToClient.newLifeOfFigure:
                    handleNewLifeOfFigure(cont);
                    break;
                case (int)MsgCoder.ServerToClient.FigureSkill:
                    handleFigureSkill(cont);
                    break;
                case (int)MsgCoder.ServerToClient.newLocationOfFigure:
                    handleNewLocationOfFigure(cont);
                    break;
                case (int)MsgCoder.ServerToClient.newFigure:
                    handleNewFigure(cont);
                    break;
            }
            cmds.Remove(cont);
        }
    }
    public void Update()
    {
        executeOrders();
    }


}
