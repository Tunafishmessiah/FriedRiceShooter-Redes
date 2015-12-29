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


        //Definir variaveis e respetivos Locks
        public Dictionary<IPEndPoint, Ship> enemies = new Dictionary<IPEndPoint, Ship>();
        
        //Locks
        private Object ShipsLock = new Object();
        private Object MessageLock = new Object();
        

        public ClientManager(Player player)
        {
            this.playah = player;
            this.Me = new UdpClient(9999);

            byte[] message = new byte[1024];
            //ServidorEndPoint
            IPEndPoint origin = new IPEndPoint(IPAddress.Any, 0);
            //Para usar como debugger
            IPEndPoint DoubleOrigin = new IPEndPoint(IPAddress.Parse("127.0.0.1"),9919);

            Thread ConnectMe = new Thread(() => {
                //Inserir Comunicação com o servidor aqui
                //Por enquanto vai ser só para adicionar um novo player do qual eu sei o IP
                
                message = Encoding.ASCII.GetBytes("HiMate");



                Socket clientConnectionSocketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                clientConnectionSocketSend.SendTo(message, DoubleOrigin);
                Thread.Sleep(100);
                /*
                 * 
                 */

            });

            //Making it able to listen
            Me.Client.EnableBroadcast = true;
            Me.Client.Ttl = 2;

            //Giving it some time do do his thing
            Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 150000);
            Me.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 150000);

            //Debuging part
        }

        public void Update(GameTime gameTime, Player player)
        {
          

            if (enemies.Count > 0)
            {
                
            }

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