using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerSim;
using MidProject;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

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
    public Transform worldCanvasTrasfrom;
    MapInstance map;
    private static ComSim instance;
    List<Figure> figures = new List<Figure>();
    private GameObject player;
    private int playerID;
    private float monsterCount = 0;
    private float monstersAlive = 0;
    public Slider hpSlider;
    public Slider proggressSlider;
    public GameObject mainMenu;
    public GameObject restartMenu;
    private GameObject mapBackground;
    private ComSim(){}
    void Start()
    {
        instance = this;
    }
    public static ComSim getInstance()
    {
        return instance;
    }
    public void setupMap(int mapID)
    {
        if (mapBackground != null)
            Destroy(mapBackground);
        if (map != null)
            map.closeInstance();
        removeFigures();
        monstersAlive = 0;
        monsterCount = 0;
        map = new MapInstance(mapID);
        mapBackground = Instantiate(PrefabHolder.instance.getMapByID(mapID), worldCanvasTrasfrom);
        map.addPlayer();
    }
    public void removeFigures()
    {
        foreach (Figure fig in figures)
            Destroy(fig.gameObjectReference);
        figures.Clear();
    }
    public void quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void setMainMenu(bool set)
    {
        if (mapBackground != null)
            Destroy(mapBackground);
        restartMenu.SetActive(!set);
        mainMenu.SetActive(set);
        mainMenu.transform.parent.gameObject.SetActive(set);
    }
    public void setRestartMenu(bool set)
    {
        if (mapBackground != null)
            Destroy(mapBackground);
        mainMenu.SetActive(!set);
        restartMenu.SetActive(set);
        restartMenu.transform.parent.gameObject.SetActive(set);
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
        map.enableMovement(playerID, data.integers[0], data.booleans[0]);
    }
    private void handleSkillRequest(DataContainer data)
    {
        map.playerUseSkill(playerID, data.integers[0], data.integers[1]);
    }
    // CLIENT
    private void handleNewLifeOfFigure(DataContainer data)
    {
        Figure figure = getFigureByID(data.integers[0]);
        float margin = 1f;
        GameObject popup = Instantiate(PrefabHolder.instance.getDamagePopup());
        popup.GetComponent<TextMeshPro>().text = data.floats[1].ToString();
        popup.transform.position = figure.gameObjectReference.transform.position + new Vector3(0, figure.gameObjectReference.GetComponent<SpriteRenderer>().bounds.size.y / 2 + margin);
        if (data.floats[0] <= 0)
        {
            figure.gameObjectReference.GetComponent<Animator>().SetTrigger("Die");
            monstersAlive--;
            proggressSlider.value = 1 - (monstersAlive / monsterCount);
            if (figure.figureID == playerID)
            {
                setRestartMenu(true);
                map.closeInstance();
                removeFigures();
            }
        }
        else
        {
            figure.gameObjectReference.GetComponent<Animator>().SetTrigger("Hit");
            figure.gameObjectReference.GetComponent<AnimationControl>().playSound("Hit");
        }
        //if (figure.figureID == playerID)
        //    hpSlider.value = data.floats[2];
        //else
        figure.gameObjectReference.GetComponent<AnimationControl>().setHealth(data.floats[2]);

    }
    private void handleFigureSkill(DataContainer data)
    {
        Figure figure = getFigureByID(data.integers[0]);
        figure.gameObjectReference.GetComponent<Animator>().SetTrigger("Skill");
        GameObject skill = Instantiate(PrefabHolder.instance.getSkillByID(data.integers[1]));
        skill.transform.position = new Vector3(data.floats[0], data.floats[1], 0);
    }
    private void handleNewLocationOfFigure(DataContainer data) 
    {
        Figure figure = getFigureByID(data.integers[0]);
        if (figure.gameObjectReference == null)
            return;
        if (figure.gameObjectReference.transform.position == new Vector3(data.floats[0], data.floats[1], 0))
            return;
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
                monsterCount++;
                monstersAlive = monsterCount;
                figures.Add(new Figure(figureID, monster));
                break;
        }
    }
    private void handleSetBool(DataContainer data)
    {
        Figure figure = getFigureByID(data.integers[0]);
        figure.gameObjectReference.GetComponent<Animator>().SetBool(data.strings[0], data.booleans[0]);
    }
    private void handleSetTrigger(DataContainer data)
    {
        Figure figure = getFigureByID(data.integers[0]);
        figure.gameObjectReference.GetComponent<Animator>().SetTrigger(data.strings[0]);
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
                case (int)MsgCoder.ServerToClient.setBool:
                    handleSetBool(cont);
                    break;
                case (int)MsgCoder.ServerToClient.setTrigger:
                    handleSetTrigger(cont);
                    break;
            }
            
        }
    }
    public void Update()
    {
        executeOrders();
    }


}
