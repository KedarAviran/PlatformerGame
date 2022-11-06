using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    static class DataHolder
    {
        const string MAPLOC = "map.txt";
        const string MONSTERLOC = "monster.txt";
        private static List<Map> maps = new List<Map>();
        private static List<Monster> monsters = new List<Monster>();
        public static float StringToFloat(string num)
        {
            return float.Parse(num, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static void LoadMapData(string fileLoc)
        {
            //// FORMAT:
            //// type=1(floor) + center + width + lenght + angle
            //// type=2(ladder) + center + width + lenght
            //// type=3(spawn) + center
            List<Floor> floors = new List<Floor>();
            List<Ladder> ladders = new List<Ladder>();
            List<Spawn> spawns = new List<Spawn>();
            List<Wall> walls = new List<Wall>();
            List<NPC> npcs = new List<NPC>();
            string[] lines = File.ReadAllText(MAPLOC).Split(Environment.NewLine);
            foreach(string line in lines)
            {
                string[] parameters = line.Split(" ");
                switch (parameters[0])
                {
                    case "0":
                        walls.Add(new Wall(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), StringToFloat(parameters[3]), StringToFloat(parameters[4])));
                        break;
                    case "1":
                        floors.Add(new Floor(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), StringToFloat(parameters[3]), StringToFloat(parameters[4]), StringToFloat(parameters[5])));
                        break;
                    case "2":
                        ladders.Add(new Ladder(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), StringToFloat(parameters[3]), StringToFloat(parameters[4])));
                        break;
                    case "3":
                        spawns.Add(new Spawn(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), 0));
                        break;
                }
            }
            maps.Add(new Map(0, spawns, npcs, ladders, floors, spawns[0].pos,walls));
        }
        public static void LoadMonsterData(string fileLoc)
        {

        }
        public static Monster getMonster(int monsterID)
        {
            if (monsters.Count == 0)
                LoadMonsterData(MONSTERLOC);
            foreach (Monster mon in monsters)
                if (mon.getID() == monsterID)
                    return mon.Clone();
            return null;
        }
        public static Map getMap(int mapID)
        {
            if (maps.Count == 0)
                LoadMapData(MAPLOC);
            foreach (Map map in maps)
                if (map.getMapID() == mapID)
                    return map;
            return null;
        }
    }
}
