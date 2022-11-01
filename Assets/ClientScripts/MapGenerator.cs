using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    const string FILENAME = "map.txt";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnApplicationQuit()
    {
        generateMap();
    }
    public void generateMap()
    {
        string map = "";
        //// FORMAT:
        //// type=1(floor) + center + width + lenght + angle
        //// type=2(ladder) + center + width + lenght
        //// type=3(spawn) + center
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
            map = map+ "1 "+ obj.transform.position.x +" "+ obj.transform.position.y + " " + obj.transform.localScale.x + " " +obj.transform.localScale.y + " " +obj.transform.rotation.z+ Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Ladder"))
            map = map + "2 " + obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.localScale.x + " " + obj.transform.localScale.y+ Environment.NewLine;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Spawn"))
            map = map + "3 " + obj.transform.position.x + " " + obj.transform.position.y+Environment.NewLine;
        File.WriteAllText(FILENAME, map);
    }
}
