using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    public struct Spawn
    {
        public Vector2 pos;
        public int figureType;
        public Spawn(Vector2 pos, int figureType)
        {
            this.pos = pos;
            this.figureType = figureType;
        }
    }
    class Map
    {
        private int mapID;
        private List<Spawn> spawns;
        private List<NPC> npcs;
        private List<Ladder> ladders;
        private List<Floor> floors;
        private List<Wall> walls;
        private Vector2 spawnPos;
        public Map(int mapID, List<Spawn> spawns, List<NPC> npcs, List<Ladder> ladders, List<Floor> floors , Vector2 spawnPos , List<Wall> walls)
        {
            this.mapID = mapID;
            this.spawns = spawns;
            this.npcs = npcs;
            this.ladders = ladders;
            this.floors = floors;
            this.spawnPos = spawnPos;
            this.walls = walls;
        }
        public int getMapID()
        {
            return mapID;
        }
        public List<Spawn> getSpawns()
        {
            return spawns;
        }
        public List<NPC> getNpcs()
        {
            return npcs;
        }
        public List<Wall> GetWalls()
        {
            return walls;
        }
        public List<Ladder> getLadders()
        {
            return ladders;
        }
        public List<Floor> getFloors()
        {
            return floors;
        }
        public Vector2 getSpawnPoint()
        {
            return spawnPos;
        }
    }
}
