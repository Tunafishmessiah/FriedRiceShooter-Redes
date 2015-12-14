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
    class ClientManager
    {
        //Definir o cliente
        bool Registered = false;
        UdpClient Me;

        //Definir variaveis e respetivos Locks
        public Dictionary<IPEndPoint, Ship> enemies = new Dictionary<IPEndPoint, Ship>();
        private List<Message> MessagesToSend();
        
        //Locks
        private Object ShipsLock = new Object();
        private Object MessageLock = new Object();

        public ClientManager()
        {
            Me = new UdpClient(9999);

            //Making it able to listen
            Me.Client.EnableBroadcast = true;
            Me.Client.Ttl = 2;

            //Giving it some time do do his thing
            Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 150000);
            Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 150000);
        }

        public void Update(GameTime gameTime)
        {
 
        }


    }
}