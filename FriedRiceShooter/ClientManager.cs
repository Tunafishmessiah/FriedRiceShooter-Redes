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
        Player playah;


        //Definir variaveis e respetivos Locks
        public Dictionary<IPEndPoint, Ship> enemies = new Dictionary<IPEndPoint, Ship>();
        private List<Message> MessagesToSend;
        
        //Locks
        private Object ShipsLock = new Object();
        private Object MessageLock = new Object();
        

        public ClientManager(Player player)
        {
            this.playah = player;
            this.Me = new UdpClient(9999);

            IPEndPoint origin = new IPEndPoint(IPAddress.Any, 0);

            //Making it able to listen
            Me.Client.EnableBroadcast = true;
            Me.Client.Ttl = 2;

            //Giving it some time do do his thing
            Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 150000);
            Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 150000);
        }

        public void Update(GameTime gameTime, Player player)
        {
          

            if (enemies.Count > 0)
            {

                foreach (IPEndPoint enemy in enemies.Keys)
                {
                    Message position = new Message("PX" + player.Position.X.ToString() + "Y" + player.Position.Y.ToString(), enemy);  
                }
                if (MessagesToSend.Count > 0)
                {
                    lock (MessageLock)
                    {
                        foreach (Message mess in MessagesToSend.ToArray())
                        {
                            Me.BeginSend(mess.message,
                                mess.message.Length,
                                mess.receiver,
                                terminateSend,
                                Me);
                        }
                    }
                }
                
            }

        }

        private void terminateSend(IAsyncResult result)
        {
            UdpClient me = result.AsyncState as UdpClient;
            me.EndSend(result);
        }


    }
}