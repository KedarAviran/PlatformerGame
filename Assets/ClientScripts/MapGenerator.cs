using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ServerSim;

public class MapGenerator : MonoBehaviour
{
    const string MAPFILENAME = "map.txt";
    const string MONSTERSFILENAME = "map.txt";
    public GameObject floor;
    public GameObject ladder;
    public GameObject wall;
    public bool GenerateMap;
    public bool LoadFromServer;
    public enum ObjectType
    {
        wall,
        floor,
        ladder,
        spawn,
    }
    void Start()
    {
        if (LoadFromServer)
            loadMap();
    }
    private void OnApplicationQuit()
    {
        if (GenerateMap)
            generateMap();
    }
    public float StringToFloat(string num)
    {
        return float.Parse(num, System.Globalization.CultureInfo.InvariantCulture);
    }
    //public void generateMonsters()
    //{
    //    string monsters = "";
    //    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Monsters"))
    //        monsters = monsters + obj.name + " " + obj.transform.localScale.x + " " + obj.transform.localScale.y + Environment.NewLine;
    //    File.WriteAllText(MONSTERSFILENAME, monsters);
    //}
    public void loadMap()// FOR TESTING ONLY
    {
        string[] lines = File.ReadAllText(MAPFILENAME).Split(Environment.NewLine);
        foreach (string line in lines)
        {
            string[] parameters = line.Split(" ");
            if (parameters[0] == "")
                break;
            switch (int.Parse(parameters[0]))
            {
                case (int)MapGenerator.ObjectType.wall:
                    GameObject wal = Instantiate(wall);
                    wal.transform.position = new Vector3(StringToFloat(parameters[1]), StringToFloat(parameters[2]), 0);
                    wal.transform.localScale = new Vector3(StringToFloat(parameters[3]), StringToFloat(parameters[4]), 0);
                    break;
                case (int)MapGenerator.ObjectType.floor:
                    GameObject flr = Instantiate(floor);
                    flr.transform.position = new Vector3(StringToFloat(parameters[1]), StringToFloat(parameters[2]), 0);
                    flr.transform.localScale = new Vector3(StringToFloat(parameters[3]), StringToFloat(parameters[4]),0);
                    break;
                case (int)MapGenerator.ObjectType.ladder:
                    GameObject ldr = Instantiate(ladder);
                    ldr.transform.position = new Vector3(StringToFloat(parameters[1]), StringToFloat(parameters[2]), 0);
                    ldr.transform.localScale = new Vector3(StringToFloat(parameters[3]), StringToFloat(parameters[4]), 0);
                    break;
                case (int)MapGenerator.ObjectType.spawn:
                    //spawns.Add(new Spawn(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), 0));
                    break;
            }
        }
    }
    public void generateMap()
    {
        string map = "";
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Wall"))
            map = map + (int)ObjectType.wall + " " + obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.localScale.x + " " + obj.transform.localScale.y + Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
            map = map+ (int)ObjectType.floor+ " "+ obj.transform.position.x +" "+ obj.transform.position.y + " " + obj.transform.localScale.x + " " +obj.transform.localScale.y + " " +obj.transform.rotation.z+ Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Ladder"))
            map = map + (int)ObjectType.ladder + " " + obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.localScale.x + " " + obj.transform.localScale.y+ Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Spawn"))
            map = map + (int)ObjectType.spawn + " " + obj.transform.position.x + " " + obj.transform.position.y + " " + obj.name + Environment.NewLine;
        File.WriteAllText(MAPFILENAME, map);
    }
}
