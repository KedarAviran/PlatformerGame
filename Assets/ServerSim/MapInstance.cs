﻿using System;
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
            monsters = new List<Monster>();
            map = DataHolder.getMap(mapID);
            createMonsters(map.getSpawns());
            time = DateTime.UtcNow;
            Thread thread = new Thread(new ThreadStart(Update));
            thread.Start();
        }
        public void createMonsters(List<Spawn> spawns)
        {
            foreach(Spawn spawn in spawns)
            {
                Monster mon = DataHolder.getMonster(spawn.figureType);
                mon.setID(++figureIDCounter);
                mon.updatePosition(spawn.pos);
                monsters.Add(mon);
                ComSim.instance.receiveMsgClient(MsgCoder.newFigureOrder(figureIDCounter, (int)MsgCoder.Figures.monster, mon.getMonsterType(), mon.getPos()));
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
            Player player = DataHolder.getPlayer(1);
            player.updatePosition(map.getSpawnPoint());
            player.setID(++figureIDCounter);
            players.Add(player);
            ComSim.instance.receiveMsgClient(MsgCoder.newFigureOrder(figureIDCounter, (int)MsgCoder.Figures.player, player.getPlayerType(), player.getPos()));
        }
        public void moveFigure(int figureID , int dir)
        {
            Figure figure = getPlayerByID(figureID);
            if (dir != (int)MsgCoder.Direction.Up && dir != (int)MsgCoder.Direction.Down)
            {
                figure.move(dir);
                checkFigureOnAir(figure);
                wallCheck(figure);
                if (figure is Player)
                    ((Player)figure).setOnLadder(false);
            }
            else
            {
                if (isOnLadder(figureID))
                {
                    clearFloors(figureID);
                    ((Player)figure).setOnLadder(true);
                    figure.setOnAir(false);
                    figure.move(dir);
                }
                else
                    ((Player)figure).setOnLadder(false);
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
                    if (figure.getOnAir())
                        figure.updatePosition(new Vector2(figure.getPos().X, floor.getYofFloor() + (figure.getPos().Y - figure.GetColider2D().getBotLeft().Y) + DISTANCEFROMGROUND));
                    figure.setOnAir(false);
                    if (figure is Monster)
                        ((Monster)figure).setPatrolLimit(floor.GetColider2D().getTopLeft().X, floor.GetColider2D().getTopRight().X);
                    return;
                }
            figure.setOnAir(true);  
        }
        private bool wallCheck(Figure figure)
        {
            foreach (Wall wall in map.GetWalls())
                if (wall.isColiding(figure))
                {
                    float wallLeftX = wall.GetColider2D().getBotLeft().X;
                    float wallRightX = wall.GetColider2D().getBotRight().X;
                    float figureRightX= figure.GetColider2D().getBotRight().X;
                    float figureLeftX = figure.GetColider2D().getBotLeft().X;
                    if (figureRightX >= wallLeftX && figureLeftX < wallLeftX)
                        figure.updatePosition(new Vector2(wallLeftX - Math.Abs(figure.getPos().X - figureLeftX), figure.getPos().Y));
                    else
                        figure.updatePosition(new Vector2(wallRightX + Math.Abs(figure.getPos().X -figureLeftX), figure.getPos().Y));
                    return true;
                } 
            return false;
        }
        public void playerUseSkill(int playerID,int skillID,int dir)
        {
            Player player = getPlayerByID(playerID);
            Skill skill = DataHolder.getSkill(skillID);
            if (player.isSkillOnCD(skillID))
                return;
            player.setSkillLastUse(skillID);
            Colider2D skillColider = skill.GetColider2D();
            if (dir == (int)MsgCoder.Direction.Right)
                skillColider.updateColider(player.getPos() + skill.getReletivePos());
            else
                skillColider.updateColider(player.getPos() + new Vector2(-1 * skill.getReletivePos().X, skill.getReletivePos().Y));
            ComSim.instance.receiveMsgClient(MsgCoder.figureSkillOrder(playerID, skillID, skillColider.getCenter()));
            foreach (Monster mon in monsters)
                if (mon.GetColider2D().isParallelColiding(skillColider))
                    mon.gotAttacked(skill.getDamage(),player);
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
                foreach (Monster mon in monsters)
                    if (player.GetColider2D().isParallelColiding(mon.GetColider2D()))
                        player.gotAttacked(mon.getDamage());
                if (player.getOnAir()&& !player.getOnLadder())
                    player.applyGravity(delta);
                checkFigureOnAir(player);
                player.sendUpdateToClient();
            }
            foreach (Monster mon in monsters)
            {
                mon.patrol();
                mon.aggroMove();
                if (mon.getOnAir())
                    mon.applyGravity(delta);
                wallCheck(mon);
                checkFigureOnAir(mon);
                mon.sendUpdateToClient();
            }
        }
    }
}
