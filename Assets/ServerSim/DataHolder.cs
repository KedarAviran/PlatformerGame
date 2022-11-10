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
        const string MONSTERLOC = "monsters.txt";
        const string SKILLSLOC = "skills.txt";
        private static List<Map> maps = new List<Map>();
        private static List<Monster> monsters = new List<Monster>();
        private static List<Skill> skills = new List<Skill>();
        public static float StringToFloat(string num)
        {
            return float.Parse(num, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static void LoadMapData(string fileLoc)
        {
            List<Floor> floors = new List<Floor>();
            List<Ladder> ladders = new List<Ladder>();
            List<Spawn> spawns = new List<Spawn>();
            List<Wall> walls = new List<Wall>();
            List<NPC> npcs = new List<NPC>();
            string[] lines = File.ReadAllText(fileLoc).Split(Environment.NewLine);
            foreach(string line in lines)
            {
                string[] parameters = line.Split(" ");
                if (parameters[0] == "")
                    break;
                switch (int.Parse(parameters[0]))
                {
                    case (int)MapGenerator.ObjectType.wall:
                        walls.Add(new Wall(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), StringToFloat(parameters[3]), StringToFloat(parameters[4])));
                        break;
                    case (int)MapGenerator.ObjectType.floor:
                        floors.Add(new Floor(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), StringToFloat(parameters[3]), StringToFloat(parameters[4]), StringToFloat(parameters[5])));
                        break;
                    case (int)MapGenerator.ObjectType.ladder:
                        ladders.Add(new Ladder(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), StringToFloat(parameters[3]), StringToFloat(parameters[4])));
                        break;
                    case (int)MapGenerator.ObjectType.spawn:
                        spawns.Add(new Spawn(new Vector2(StringToFloat(parameters[1]), StringToFloat(parameters[2])), int.Parse(parameters[3])));
                        break;
                }
            }
            maps.Add(new Map(0, spawns, npcs, ladders, floors, new Vector2(-33, -11), walls));
        }
        public static void LoadMonsterData(string fileLoc)
        {
            string[] lines = File.ReadAllText(fileLoc).Split(Environment.NewLine);
            foreach (string line in lines)
            {
                string[] parameters = line.Split(" ");
                monsters.Add(new Monster(int.Parse(parameters[0]), new Colider2D(Vector2.Zero,StringToFloat(parameters[1]), StringToFloat(parameters[2]),0), int.Parse(parameters[3]), int.Parse(parameters[4])));
            }
        }
        public static void LoadSkillsData(string fileLoc)
        {

        }
        public static Skill getSkill(int skillID)
        {
            if (skills.Count == 0)
                LoadSkillsData(SKILLSLOC);
            foreach (Skill skill in skills)
                if (skill.getID() == skillID)
                    return skill;
            return null;
        }
        public static Monster getMonster(int monsterID)
        {
            if (monsters.Count == 0)
                LoadMonsterData(MONSTERLOC);
            foreach (Monster mon in monsters)
                if (mon.getMonsterType() == monsterID)
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
