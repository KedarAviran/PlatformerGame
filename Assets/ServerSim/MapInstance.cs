using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;
using MidProject;

namespace ServerSim
{
    class MapInstance
    {
        private int figureIDCounter = 0;
        const double UPDATETIMER = 0.005; // IN SECONDS
        List<Player> players;
        List<Monster> monsters;
        Map map; // READ ONLY
        private DateTime time;
        private float gravityFactor = 0.05f;
        public MapInstance(int mapID)
        {
            players = new List<Player>();
            map = DataHolder.getMap(mapID);
            //createMonsters(map.getSpawns());
            time = DateTime.UtcNow;
            Thread thread = new Thread(new ThreadStart(Update));
            thread.Start();
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
        public void addPlayer()
        {
            Player player = new Player(map.getSpawnPoint());
            player.setID(++figureIDCounter);
            players.Add(player);
            ComSim.instance.receiveMsgClient(MsgCoder.newFigureOrder(figureIDCounter, player.getPos()));
        }
        public void Update()
        {
            TimeSpan delta;
            while (true)
            {
                delta = DateTime.UtcNow - time;
                if (UPDATETIMER < delta.TotalSeconds)
                {
                    time = DateTime.UtcNow;
                    UpdateInstance(delta);
                }
            }
        }
        public void UpdateInstance(TimeSpan delta)
        {
            players[0].updatePosition(players[0].getPos() - new Vector2(0, gravityFactor));
            ComSim.instance.receiveMsgClient(MsgCoder.newLocationOrder(players[0].getID(), players[0].getPos()));
        }
    }
}
