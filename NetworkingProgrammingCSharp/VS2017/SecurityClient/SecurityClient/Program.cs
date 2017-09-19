using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;

namespace SecurityClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Из командной строки можно передать опцию assert или deny
            //если опция первая, то успешное выполнение, иначе порождается исключение
            String option = null;

            if (args.Length > 0)
            {
                option = args[0];
            }
            else
            {
                option = "assert";
            }
            Console.WriteLine("option: " + option);
            MethodA(option);
        }

        public static void MethodA(String option)
        {
            Console.WriteLine("MethodA");

            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketPermission permitSocket = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, "127.0.0.1", SocketPermission.AllPorts);

            //на основе переданного параметра выбираем Assert или Deny
            if (option.Equals("deny"))
            {
                permitSocket.Deny();
            }
            else
            {
                permitSocket.Assert();
            }

            try
            {
                // соединяем сокет с удаленной endPoint, перехватываем все ошибки
                sender.Connect(ipEndPoint);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                byte[] bytes = new byte[1024];
                byte[] msg = Encoding.ASCII.GetBytes("This is a test <EOF>");

                // отправляем данные через сокет
                int bytesSent = sender.Receive(bytes);
                Console.WriteLine(bytesSent);
                Console.WriteLine(bytes);

                // получаем ответ от удаленного сервера
                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("Enchoed Test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
            }
            catch(SocketException se)
            {
                Console.WriteLine("Socket exception:" + se.ToString());
            }
            catch(SecurityException se)
            {
                Console.WriteLine("Socket Exception:" + se.ToString());
            }
            finally
            {
                if (sender.Connected)
                {
                    //освобождаем сокет
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
            Console.WriteLine("Closing MethodA");
            Console.ReadLine();
        }
    }
}
