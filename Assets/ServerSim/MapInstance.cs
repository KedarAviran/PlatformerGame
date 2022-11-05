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
        const double UPDATETIMER = 0.01; // IN  SECONDS
        List<Player> players;
        List<Monster> monsters;
        Map map; // READ ONLY
        private const float DISTANCEFROMGROUND = 0f;
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
        public void movePlayer(int figureID , int dir)
        {
            Player player = getPlayerByID(figureID);
            if (dir != (int)MsgCoder.Direction.Up && dir != (int)MsgCoder.Direction.Down)
            {
                player.move(dir);
                checkFigureOnAir(player);
            }
            else
            {
                if (isOnLadder(figureID))
                {
                    clearFloors(figureID);
                    player.setOnAir(false);
                    player.move(dir);
                }
            }
                
        }
        public void clearFloors(int figureID)
        {
            foreach (Floor floor in map.getFloors())
                floor.removeFigure(figureID);
        }
        private bool isOnLadder(int figureID)
        {
            Player player = getPlayerByID(figureID);
            foreach (Ladder ldr in map.getLadders())
                if (ldr.isOnLadder(player.GetColider2D()))
                    return true;
            return false;
        }
        private void checkFigureOnAir(Figure figure)
        {
            if (figure.getVerticalVelocity() > 0)
                return;
            foreach (Floor floor in map.getFloors())
                if (floor.checkFigureColision(figure))
                {
                    figure.setOnAir(false);
                    figure.updatePosition(new Vector2(figure.getPos().X, floor.getYofFloor() + (figure.getPos().Y - figure.GetColider2D().getBotLeft().Y) + DISTANCEFROMGROUND));
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
                if (UPDATETIMER < delta.TotalSeconds)
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
