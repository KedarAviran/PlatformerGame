using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerSim;
using MidProject;
using System;

public class ComSim : MonoBehaviour
{
    // Start is called before the first frame updatea
    public struct Figure
    {
        public int figureID;
        public GameObject gameObjectReference;
        public Figure(int figureID, GameObject reference)
        {
            this.figureID = figureID;
            this.gameObjectReference = reference;
        }
        
    }
    MapInstance map;
    public static ComSim instance;
    List<Figure> figures = new List<Figure>();
    private GameObject player;
    private int playerID;
    
    void Start()
    {
        instance = this;
        map = new MapInstance(0);
        map.addPlayer();
    }
    public Figure getFigureByID(int figureID)
    {
        foreach (Figure fig in figures)
            if (fig.figureID == figureID)
                return fig;
        return figures[0];
    }
    public void log(string msg)
    {
        Debug.Log(msg);
    }
    public void receiveMsgServer(byte[] data)
    {
        DataContainer cont = MsgCoder.getDataContainerServer(data);
        switch (cont.requestType)
        {
            case (int)MsgCoder.ClientToServer.moveRequest:
                handleMoveRequest(cont);
                break;
            case (int)MsgCoder.ClientToServer.SkillRequest:
                handleSkillRequest(cont);
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
        map.moveFigure(playerID, data.integers[0]);
    }
    private void handleSkillRequest(DataContainer data)
    {
        map.playerUseSkill(playerID, data.integers[0], data.integers[1]);
    }
    // CLIENT
    private void handleNewLifeOfFigure(DataContainer data)
    {
        Figure figure = getFigureByID(data.integers[0]);
        if (data.floats[0] <= 0)
        {
            figure.gameObjectReference.GetComponent<AnimationController>().setAnimation("Die");
            figure.gameObjectReference.SetActive(false);
        }
        else
            figure.gameObjectReference.GetComponent<AnimationController>().setAnimation("Hit");

    }
    private void handleFigureSkill(DataContainer data)
    {

    }
    private void handleNewLocationOfFigure(DataContainer data) 
    {
        Figure figure = getFigureByID(data.integers[0]);
        if (figure.gameObjectReference != null)
            figure.gameObjectReference.transform.position = new Vector3(data.floats[0], data.floats[1], 0);
    }
    private void handleNewFigure(DataContainer data)
    {
        int figureID = data.integers[0];
        int figureType = data.integers[2];
        switch (data.integers[1])
        {
            case (int)MsgCoder.Figures.player:
                player = Instantiate(PrefabHolder.instance.getFigureByType(figureType));
                player.name = "Player ID: " + figureID;
                player.transform.position = new Vector3(data.floats[0], data.floats[1], 0);
                playerID = figureID;
                figures.Add(new Figure(figureID, player));
                break;
            case (int)MsgCoder.Figures.monster:
                GameObject monster = Instantiate(PrefabHolder.instance.getFigureByType(figureType));
                monster.name = "monster ID: " + figureID;
                monster.transform.position = new Vector3(data.floats[0], data.floats[1], 0);
                figures.Add(new Figure(figureID, monster));
                break;
        }
    }
    List<DataContainer> cmds = new List<DataContainer>();
    public void executeOrders()
    {
        while (cmds.Count > 0)
        {
            DataContainer cont = cmds[0];
            cmds.RemoveAt(0);
            if (cont == null)
                return;
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
            
        }
    }
    public void Update()
    {
        executeOrders();
    }


}
