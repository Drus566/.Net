using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.Resolve("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            //Создаем сокет Tcp/Ip
            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                //Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидание подключения к порту {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();

                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться
                    while (true)
                    {
                        byte[] bytes = new byte[1024];
                        // Данные, полученные от клиента
                        int bytesRec = handler.Receive(bytes);
                        // Байты преобразуются в строку
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        // Проверка конца сообщения
                        if(data.IndexOf("<TheEnd>") > -1)
                        {
                            break;
                        }
                    }
                    // Показываем данные на консоле
                    Console.WriteLine("Text received: {0}", data);
                    string theReply = "Thank you for those " + data.Length.ToString() + " characters...";
                    byte[] msg = Encoding.ASCII.GetBytes(theReply);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
