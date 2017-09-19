using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Буфер для входящих данных
            byte[] bytes = new byte[1024];

            //Соединяемся с удаленным устройством
            try
            {
                //Устанавливаем удаленную конечную точку для сокета
                IPHostEntry ipHost = Dns.Resolve("127.0.0.1");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Соединяем сокет с удаленной точкой
                sender.Connect(ipEndPoint);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                string theMessage = "This is a test";

                byte[] msg = Encoding.ASCII.GetBytes(theMessage + "<TheEnd>");

                //Отправляем данные через сокет
                int bytesSent = sender.Send(msg);

                //Получаем ответ от удаленного устройства
                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("The Server says: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                //Освобождаем сокет 
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
        }
    }
}
