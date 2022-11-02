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
            useSkill
        }
        public enum ServerToClient
        {
            newLocationOfFigure,
            FigureSkill,
            newLifeOfFigure,
        }
        public static byte[] moveRequest(int side) //1-right 2-left 3-jump
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ClientToServer.moveRequest);
            buffer.writeInteger(side);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillMoveRequest(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ClientToServer.moveRequest;
            container.integers.Add(buffer.readInteger());
            buffer.Dispose();
            return container;
        }
        public static byte[] skillRequest(int skillID , int side)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.writeInteger((int)ClientToServer.useSkill);
            buffer.writeInteger(skillID);
            buffer.writeInteger(side);
            byte[] array = buffer.ToArray();
            buffer.Dispose();
            return array;
        }
        private static DataContainer fillSkillRequest(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ClientToServer.useSkill;
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
                case (int)ClientToServer.useSkill:
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
        private static DataContainer fillLocationOrder(ByteBuffer buffer)
        {
            DataContainer container = new DataContainer();
            container.requestType = (int)ServerToClient.newLocationOfFigure;
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
            if (cmd == (int)ServerToClient.newLocationOfFigure)
                return fillLocationOrder(buffer);
            return null;
        }
    }
}
