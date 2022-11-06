using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ServerSim;

public class MapGenerator : MonoBehaviour
{
    const string FILENAME = "map.txt";
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

    // Update is called once per frame
    void Update()
    {
        
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
    public void loadMap()// FOR TESTING ONLY
    {
        //// FORMAT:
        //// type=1(floor) + center + width + lenght + angle
        //// type=2(ladder) + center + width + lenght
        //// type=3(spawn) + center
        string[] lines = File.ReadAllText(FILENAME).Split(Environment.NewLine);
        foreach (string line in lines)
        {
            string[] parameters = line.Split(" ");
            switch (parameters[0])
            {
                case "0":
                    GameObject wal = Instantiate(wall);
                    wal.transform.position = new Vector3(StringToFloat(parameters[1]), StringToFloat(parameters[2]), 0);
                    wal.transform.localScale = new Vector3(StringToFloat(parameters[3]), StringToFloat(parameters[4]), 0);
                    break;
                case "1":
                    GameObject flr = Instantiate(floor);
                    flr.transform.position = new Vector3(StringToFloat(parameters[1]), StringToFloat(parameters[2]), 0);
                    flr.transform.localScale = new Vector3(StringToFloat(parameters[3]), StringToFloat(parameters[4]),0);
                    break;
                case "2":
                    GameObject ldr = Instantiate(ladder);
                    ldr.transform.position = new Vector3(StringToFloat(parameters[1]), StringToFloat(parameters[2]), 0);
                    ldr.transform.localScale = new Vector3(StringToFloat(parameters[3]), StringToFloat(parameters[4]), 0);
                    break;
                case "3":
                    //spawns.Add(new Spawn(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), 0));
                    break;
            }
        }
    }
    public void generateMap()
    {
        string map = "";
        //// FORMAT:
        //// type=1(floor) + center + width + lenght + angle
        //// type=2(ladder) + center + width + lenght
        //// type=3(spawn) + center
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Wall"))
            map = map + (int)ObjectType.wall + " " + obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.localScale.x + " " + obj.transform.localScale.y + Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
            map = map+ (int)ObjectType.floor+ " "+ obj.transform.position.x +" "+ obj.transform.position.y + " " + obj.transform.localScale.x + " " +obj.transform.localScale.y + " " +obj.transform.rotation.z+ Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Ladder"))
            map = map + (int)ObjectType.ladder + " " + obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.localScale.x + " " + obj.transform.localScale.y+ Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Spawn"))
            map = map + (int)ObjectType.spawn + " " + obj.transform.position.x + " " + obj.transform.position.y+Environment.NewLine;
        File.WriteAllText(FILENAME, map);
    }
}
