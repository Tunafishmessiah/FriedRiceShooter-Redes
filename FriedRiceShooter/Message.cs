using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;

namespace FriedRiceShooter
{
    public class Message
    {
        public byte[] message;
        public IPEndPoint receiver;

        public Message(byte[] ToSend, IPEndPoint ToReceive)
        {
            this.message = ToSend;
            this.receiver = ToReceive;
        }
        public Message(string ToSend, IPEndPoint ToReceive)
        {
            this.message = Encoding.ASCII.GetBytes(ToSend);
            this.receiver = ToReceive;
        }
    }
}
