using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;


namespace DecSecurity
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DNS RESOLVE: " + Dns.Resolve(Dns.GetHostName()));
            LegalMethod();
            IllegalMethod();
        }
        [SocketPermission(SecurityAction.Assert, Access = "Connect", Host = "192.168.1.214", Port = "All", Transport = "Tcp")]
        
        public static void LegalMethod()
        {
            Console.WriteLine("LegalMethod");

            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Соединяем сокет с удаленной endPoint и перехватываем все ошибки 
                sender.Connect(ipEndPoint);
                Console.WriteLine("Socket conntected to {0}", sender.RemoteEndPoint.ToString());
            }
            catch(SecurityException se)
            {
                Console.WriteLine("Security Exception:" + se);
            }
            catch(SocketException se)
            {
                Console.WriteLine("Socket Exception:" + se);
            }
            finally
            {
                if (sender.Connected)
                {
                    //Освобождаем сокет
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
        }

        [SocketPermission(SecurityAction.Deny, Access = "Connect", Host = "192.168.1.214", Port = "All", Transport = "Tcp")]
        public static void IllegalMethod()
        {
            Console.WriteLine("IllegalMethod " + Dns.Resolve(Dns.GetHostName()));

            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Соединяем сокет с удаленной endPoint и перехватываем все ошибки 
                sender.Connect(ipEndPoint);
                Console.WriteLine("Socket conntected to {0}", sender.RemoteEndPoint.ToString());
            }
            catch (SecurityException se)
            {
                Console.WriteLine("Security Exception:" + se);
            }
            catch (SocketException se)
            {
                Console.WriteLine("Socket Exception:" + se);
            }
            finally
            {
                if (sender.Connected)
                {
                    //Освобождаем сокет
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
            Console.ReadLine();
        }
    }
}
