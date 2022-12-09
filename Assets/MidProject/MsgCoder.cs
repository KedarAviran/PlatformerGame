using System;
using System.Collections.Generic;
using System.Numerics;

namespace MidProject
{
    static class MsgCoder
    {
        public enum ClientToServer
        {
            moveRequest,
            SkillRequest
        }
        public enum ServerToClient
        {
            newLocationOfFigure,
            FigureSkill,
            newLifeOfFigure,
            newFigure,
            setBool,
            setTrigger
        }
        public enum Direction
        {
            Right,
            Left,
            Jump,
            Up,
            Down
        }
        public enum Figures
        {
            player,
            monster,
            npc
        }
        public static byte[] moveRequest(Direction dir , bool enable)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ClientToServer.moveRequest);
            buffer.writeInteger((int)dir);
            buffer.writeBool(enable);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillMoveRequest(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ClientToServer.moveRequest;
            container.integers.Add(buffer.readInteger());
            container.booleans.Add(buffer.readBool());
            buffer.Dispose();
            return container;
        }
        public static byte[] skillRequest(int skillID , Direction dir)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ClientToServer.SkillRequest);
            buffer.writeInteger(skillID);
            buffer.writeInteger((int)dir);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillSkillRequest(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ClientToServer.SkillRequest;
            container.integers.Add(buffer.readInteger());
            container.integers.Add(buffer.readInteger());
            buffer.Dispose();
            return container;
        }
        public static DataContainer getDataContainerServer(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeBytes(data);
            int cmd = buffer.readInteger();
            switch (cmd)
            {
                case (int)ClientToServer.moveRequest:
                    return fillMoveRequest(buffer);
                case (int)ClientToServer.SkillRequest:
                    return fillSkillRequest(buffer);
            }
            return null;
        }
        public static byte[] newLocationOrder(int figureID, Vector2 pos)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ServerToClient.newLocationOfFigure);
            buffer.writeInteger(figureID);
            buffer.writeFloat(pos.X);
            buffer.writeFloat(pos.Y);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillNewLocationOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.newLocationOfFigure;
            container.integers.Add(buffer.readInteger());
            container.floats.Add(buffer.readFloat());
            container.floats.Add(buffer.readFloat());
            buffer.Dispose();
            return container;
        }
        public static byte[] newFigureOrder(int figureID, int figureKind, int figureType, Vector2 pos)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ServerToClient.newFigure);
            buffer.writeInteger(figureID);
            buffer.writeInteger(figureKind);
            buffer.writeInteger(figureType);
            buffer.writeFloat(pos.X);
            buffer.writeFloat(pos.Y);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillNewFigureOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.newFigure;
            container.integers.Add(buffer.readInteger());
            container.integers.Add(buffer.readInteger());
            container.integers.Add(buffer.readInteger());
            container.floats.Add(buffer.readFloat());
            container.floats.Add(buffer.readFloat());
            buffer.Dispose();
            return container;
        }
        public static byte[] newLifeOfFigureOrder(int figureID, float life , float damageTaken)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ServerToClient.newLifeOfFigure);
            buffer.writeInteger(figureID);
            buffer.writeFloat(life);
            buffer.writeFloat(damageTaken);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillnewLifeOfFigureOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.newLifeOfFigure;
            container.integers.Add(buffer.readInteger());
            container.floats.Add(buffer.readFloat());
            container.floats.Add(buffer.readFloat());
            buffer.Dispose();
            return container;
        }
        public static byte[] setBoolOrder(int figureID, string order , bool onLadder)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ServerToClient.setBool);
            buffer.writeInteger(figureID);
            buffer.writeString(order);
            buffer.writeBool(onLadder);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillSetBoolOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.setBool;
            container.integers.Add(buffer.readInteger());
            container.strings.Add(buffer.readString());
            container.booleans.Add(buffer.readBool());
            buffer.Dispose();
            return container;
        }
        public static byte[] setTriggerOrder(int figureID, string trigger)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ServerToClient.setTrigger);
            buffer.writeInteger(figureID);
            buffer.writeString(trigger);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillSetTriggerOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.setTrigger;
            container.integers.Add(buffer.readInteger());
            container.strings.Add(buffer.readString());
            buffer.Dispose();
            return container;
        }
        public static byte[] figureSkillOrder(int figureID, int skillID,Vector2 pos)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ServerToClient.FigureSkill);
            buffer.writeInteger(figureID);
            buffer.writeInteger(skillID);
            buffer.writeFloat(pos.X);
            buffer.writeFloat(pos.Y);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillFigureSkillOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.FigureSkill;
            container.integers.Add(buffer.readInteger());
            container.integers.Add(buffer.readInteger());
            container.floats.Add(buffer.readFloat());
            container.floats.Add(buffer.readFloat());
            buffer.Dispose();
            return container;
        }
        public static DataContainer getDataContainerClient(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeBytes(data);
            int cmd = buffer.readInteger();
            switch (cmd)
            {
                case (int)ServerToClient.newLocationOfFigure:
                    return fillNewLocationOrder(buffer);
                case (int)ServerToClient.FigureSkill:
                    return fillFigureSkillOrder(buffer);
                case (int)ServerToClient.newLifeOfFigure:
                    return fillnewLifeOfFigureOrder(buffer);
                case (int)ServerToClient.newFigure:
                    return fillNewFigureOrder(buffer);
                case (int)ServerToClient.setBool:
                    return fillSetBoolOrder(buffer);
                case (int)ServerToClient.setTrigger:
                    return fillSetTriggerOrder(buffer);
            }
            return null;
        }
    }
}
