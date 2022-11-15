using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using MidProject;

namespace ServerSim
{
    static class DataHolder
    {
        const string MAPLOC = "map.txt";
        const string FIGURELOC = "figures.txt";
        const string SKILLSLOC = "skills.txt";
        private static List<Map> maps = new List<Map>();
        private static List<Figure> figures = new List<Figure>();
        private static List<Skill> skills = new List<Skill>();
        private static float StringToFloat(string num)
        {
            return float.Parse(num, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        private static void LoadMapData(string fileLoc)
        {
            List<Floor> floors = new List<Floor>();
            List<Ladder> ladders = new List<Ladder>();
            List<Spawn> spawns = new List<Spawn>();
            List<Wall> walls = new List<Wall>();
            List<NPC> npcs = new List<NPC>();
            string[] lines = File.ReadAllText(fileLoc).Split(Environment.NewLine);
            foreach (string line in lines)
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
        private static void LoadFiguresData(string fileLoc)
        {
            string[] lines = File.ReadAllText(fileLoc).Split(Environment.NewLine);
            foreach (string line in lines)
            {
                string[] parameters = line.Split(" ");
                if (parameters[0] == "")
                    break;
                switch (int.Parse(parameters[0]))
                {
                    case (int)MsgCoder.Figures.monster:
                        figures.Add(new Monster(int.Parse(parameters[1]), new Colider2D(Vector2.Zero, StringToFloat(parameters[2]), StringToFloat(parameters[3]), 0), StringToFloat(parameters[4]), StringToFloat(parameters[5]), StringToFloat(parameters[6]), StringToFloat(parameters[7])));
                        break;
                    case (int)MsgCoder.Figures.player:
                        figures.Add(new Player(int.Parse(parameters[1]), new Colider2D(Vector2.Zero, StringToFloat(parameters[2]), StringToFloat(parameters[3]), 0), StringToFloat(parameters[4]), StringToFloat(parameters[5]), StringToFloat(parameters[6])));
                        break;
                    case (int)MsgCoder.Figures.npc:
                        break;
                }
            }
        }
        private static void LoadSkillsData(string fileLoc)
        {
            string[] lines = File.ReadAllText(fileLoc).Split(Environment.NewLine);
            foreach (string line in lines)
            {
                string[] parameters = line.Split(" ");
                skills.Add(new Skill(int.Parse(parameters[0]), new Colider2D(Vector2.Zero, StringToFloat(parameters[1]), StringToFloat(parameters[2]), StringToFloat(parameters[3])), new Vector2(StringToFloat(parameters[4]), StringToFloat(parameters[5])), StringToFloat(parameters[6]), StringToFloat(parameters[7])));
            }
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
        public static Player getPlayer(int playerType)
        {
            if (figures.Count == 0)
                LoadFiguresData(FIGURELOC);
            foreach (Figure fig in figures)
                if (fig is Player)
                    if (((Player)fig).getPlayerType() == playerType)
                        return ((Player)fig).Clone();
            return null;
        }
        public static Monster getMonster(int monsterID)
        {
            if (figures.Count == 0)
                LoadFiguresData(FIGURELOC);
            foreach (Figure fig in figures)
                if (fig is Monster)
                    if (((Monster)fig).getMonsterType() == monsterID)
                        return ((Monster)fig).Clone();
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
