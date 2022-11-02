using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ServerSim
{
    class MapInstance
    {
        private int figureIDCounter = 0;
        const double UPDATETIMER = 1; // IN SECONDS
        List<Player> players;
        List<Monster> monsters;
        Map map; // READ ONLY
        private DateTime time;
        public MapInstance(int mapID)
        {
            players = new List<Player>();
            map = DataHolder.getMap(mapID);
            //createMonsters(map.getSpawns());
            time = DateTime.UtcNow;
            //Update();
        }
        public void createMonsters(List<Spawn> spawns)
        {
            foreach(Spawn spawn in spawns)
            {
                Monster mon = DataHolder.getMonster(spawn.figureID);
                mon.setID(++figureIDCounter);
                mon.updatePosition(spawn.pos);
                monsters.Add(mon);
            }
        }
        public void addPlayer(Player player)
        {
            player.setID(++figureIDCounter);
            player.updatePosition(map.getSpawnPoint());
            players.Add(player);
        }
        public void Update()
        {
            TimeSpan delta;
            while (true)
            {
                delta = DateTime.UtcNow - time;
                if (UPDATETIMER > delta.TotalSeconds)
                {
                    time = DateTime.UtcNow;
                    UpdateInstance(delta);
                }
            }
        }
        public void UpdateInstance(TimeSpan delta)
        {
            // DO GRAVITY
            // if changed pos update
        }
    }
}
