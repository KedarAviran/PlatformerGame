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
        const double UPDATETIMER = 0.1; // IN  MILISECONDS
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
        public Player getPlayerByID(int figureID)
        {
            foreach (Player player in players)
                if (player.getID() == figureID)
                    return player;
            return null;
        }
        public void addPlayer()
        {
            Player player = new Player(map.getSpawnPoint());
            player.setID(++figureIDCounter);
            players.Add(player);
            ComSim.instance.receiveMsgClient(MsgCoder.newFigureOrder(figureIDCounter, player.getPos()));
        }
        public void movePlayer(int figureID , int side)//1-right 2-left 3-jump 
        {
            Player player = getPlayerByID(figureID);
            player.move(side);
            checkFigureOnAir(player);
        }
        private void checkFigureOnAir(Figure figure)
        {
            if (figure.getVerticalVelocity() > 0)
                return;
            foreach (Floor floor in map.getFloors())
                if (floor.isColidingWithFigure(figure))
                {
                    figure.setOnAir(false);
                    return;
                }
            figure.setOnAir(true);  
        }
        public void Update()
        {
            TimeSpan delta;
            while (true)
            {
                delta = DateTime.UtcNow - time;
                if (UPDATETIMER < delta.TotalMilliseconds)
                {
                    time = DateTime.UtcNow;
                    UpdateInstance(delta);
                }
            }
        }
        public void UpdateInstance(TimeSpan delta)
        {
            foreach (Player player in players)
            {
                if (player.getOnAir())
                {
                    player.applyGravity(delta);
                    checkFigureOnAir(player);
                }
                player.sendUpdateToClient();
            }
        }
    }
}
