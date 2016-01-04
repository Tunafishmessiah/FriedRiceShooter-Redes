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
        Boolean Registred;
        Player playah;
        Socket ConnectionSocket;
        Thread ConnectMe, ListenServer;

        //Definir variaveis
        IPEndPoint Server;
        IPEndPoint SReceiver;
        public Dictionary<IPEndPoint, Ship> enemies = new Dictionary<IPEndPoint, Ship>();
        
        //Locks
        private Object ClientLock = new Object();
        private Object ShipsLock = new Object();
        private Object MessageLock = new Object();
        

        public ClientManager(Player player)
        {
            this.playah = player;

            this.ConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Byte[] message = new Byte[1024];
            this.Server = new IPEndPoint(IPAddress.Parse("25.9.152.191"), 904);
            this.SReceiver = new IPEndPoint(IPAddress.Parse("255.0.0.0"), 905);
            this.ConnectionSocket.Bind(SReceiver);

            this.ConnectMe = new Thread(() => {

                lock (MessageLock)
                {
                    message = Encoding.ASCII.GetBytes("Client Connected...");
                }

                Socket clientSocketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                while (true)
                {
                    clientSocketSend.SendTo(message, Server);
                    Console.WriteLine("Tried...");
                    Thread.Sleep(100);
                }
            });

            this.ListenServer = new Thread(() =>
            {

                EndPoint Doubleorigin;
                Doubleorigin = (EndPoint)this.SReceiver;
                ConnectionSocket.Bind(Doubleorigin);

                while (true)
                {
                    int size = this.ConnectionSocket.ReceiveFrom(message, ref Doubleorigin);
                    string answer = "";

                    if (message.ToString() != "")
                    {
                        if (message[0] == 'P' && !Registred)
                        {
                            for (int i = 1; i < size; i++)
                            {
                                answer += message[i].ToString();
                            }
                            lock (this.ClientLock)
                            {
                                this.Server = new IPEndPoint(Server.Address, int.Parse(answer));
                                this.Registred = true;
                            }

                            this.ConnectMe.Abort();
                        }
                        else
                        {
                            Boolean newClient = true;
                            for (int i = 2; i < size; i++)
                            {
                                answer += message[i].ToString();
                            }

                            int IP = int.Parse(answer);

                            foreach(IPEndPoint Adress in enemies.Keys )
                            {
                                if (int.Parse(Adress.Address.ToString()) == IP)
                                    newClient = false;
                            }

                            if (newClient)
                            {
                                AddEnemy(IP.ToString());
                            }
                        }
                        Console.WriteLine("Received: " + message.ToString() + "\nFrom " + this.SReceiver.Address.ToString());
                        this.ConnectionSocket.SendTo(Encoding.ASCII.GetBytes("00"), this.Server);
                    }
                }
            });


            this.ConnectMe.Start();
            this.ListenServer.Start();
        }

        public void Update(GameTime gameTime)
        {
            //Enviar a posição e rotação a todos os oponentes

            string message = "P";
            message += "X" + playah.Position.X + "Y" + playah.Position.Y + "R" + playah.rotation;

            byte[] messag = Encoding.ASCII.GetBytes(message);
            
            foreach (IPEndPoint ip in enemies.Keys)
            {
                this.ConnectionSocket.SendTo(messag, (EndPoint)ip);
            }

        }


        private void AddEnemy( string IP )
        {
            Ship enemy = new Player(Vector2.One, playah.graphics, playah.texture, playah.BulletTexture, playah.Spriter);
            IPEndPoint newIP = new IPEndPoint(IPAddress.Parse(IP), 1000);

            enemies.Add(newIP, enemy);
        }

    }
}
