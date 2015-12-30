using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FriedRiceShooter
{
    class ClientManager
    {
        //Definir o cliente
        bool Registered = false;
        UdpClient Me;
        Player playah;
        Socket clientConnectionSocket;
        Thread ConnectMe, ListenServer;

        //Definir variaveis e respetivos Locks
        public Dictionary<IPEndPoint, Ship> enemies = new Dictionary<IPEndPoint, Ship>();
        
        //Locks
        private Object ShipsLock = new Object();
        private Object MessageLock = new Object();
        

        public ClientManager(Player player)
        {
            this.playah = player;
            //this.Me = new UdpClient(9999);

            this.clientConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            byte[] message = new byte[1024];
            //ServidorEndPoint
            //IPEndPoint origin = new IPEndPoint(IPAddress.Parse("192.168.1.4"), 0);
            //Para usar como debugger
            IPEndPoint DoubleOrigin = new IPEndPoint(IPAddress.Parse("192.168.1.4"), 9919);

            
            ////Making it able to listen
            //Me.Client.EnableBroadcast = true;
            //Me.Client.Ttl = 2;

            ////Giving it some time do do his thing
            //Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 150000);
            //Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 150000);

            //Debuging part

            this.ConnectMe = new Thread(() => {
                //Inserir Comunicação com o servidor aqui
                //Por enquanto vai ser só para conectar a um novo player do qual eu sei o IP
                
                message = Encoding.ASCII.GetBytes("HiMate");
                while (true)
                {
                    Socket clientConnectionSocketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    clientConnectionSocketSend.SendTo(message, DoubleOrigin);
                    Console.WriteLine("Tried...");
                    Thread.Sleep(100);
                }
                /*
                 * 
                 */
            });

            this.ListenServer = new Thread(() =>
            {
                EndPoint Doubleorigin;
                try
                {
                 Doubleorigin = (EndPoint)DoubleOrigin;
                }
                catch (Exception)
                {
                    DoubleOrigin = new IPEndPoint(IPAddress.Parse("192.168.1.4"), 9918);
                    Doubleorigin = (EndPoint)DoubleOrigin;
                }
                clientConnectionSocket.Bind(Doubleorigin);

                while (true)
                {
                    int size = this.clientConnectionSocket.ReceiveFrom(message, ref Doubleorigin);

                    string answer = Encoding.ASCII.GetString(message);

                    if (message.ToString() != "")
                    {
                        Registered = true;
                        Console.WriteLine(answer + "Did it!!");
                    }
                }
            });

            this.ConnectMe.Start();
            this.ListenServer.Start();
        }

        public void Update(GameTime gameTime)
        {
            //if (Registered && ConnectMe.IsAlive)
            //{
            //    ConnectMe.Abort();
            //    ListenServer.Abort();
            //}

                      
        }

        private void terminateSend(IAsyncResult result)
        {
            UdpClient me = result.AsyncState as UdpClient;
            me.EndSend(result);
        }

        private void AddEnemy( string IP )
        {
            Ship enemy = new Player(Vector2.One, playah.graphics, playah.texture, playah.BulletTexture, playah.Spriter);
            IPEndPoint newIP = new IPEndPoint(IPAddress.Parse(IP), 1000);

            enemies.Add(newIP, enemy);
        }

    }
}