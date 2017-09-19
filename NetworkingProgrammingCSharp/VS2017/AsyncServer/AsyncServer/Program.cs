using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AsyncServer
{
    class Program
    {
        //Буфер для получения данных
        public static byte[] buffer = new byte[1024];

        //Класс события для поддержки синхронизаций
        public static ManualResetEvent socketEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            //Определяем Айди текущего Потока
            Console.WriteLine("Main Thread ID:" + AppDomain.GetCurrentThreadId());

            byte[] bytes = new byte[1024];

            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];

            IPEndPoint localEnd = new IPEndPoint(ipAddr, 11000);

            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            //Связываем сокет
            sListener.Bind(localEnd);

            //Начинаем слушать 10 подключений
            sListener.Listen(10);

            Console.WriteLine("Waiting for a connection...");

            //Экземпляр делегата асинхронного класса для указания на функцию AcceptCallback
            AsyncCallback aCallBack = new AsyncCallback(AcceptCallback);

            //асинхронная функция, дающая согласие на соединения
            sListener.BeginAccept(aCallBack, sListener);

            //ждем, пока другие потоки закончат работу
            socketEvent.WaitOne();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("AcceptCallback Thread ID:" + AppDomain.GetCurrentThreadId());

            //сокет для получения запросов
            Socket listener = (Socket)ar.AsyncState;

            //Новый сокет
            Socket handler = listener.EndAccept(ar);

            handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), handler);
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            Console.WriteLine("ReceiveCallback Thread ID:" + AppDomain.GetCurrentThreadId());

            string content = String.Empty;

            Socket handler = (Socket)ar.AsyncState;

            int bytesRead = handler.EndReceive(ar);

            //Если есть данные...
            if(bytesRead > 0)
            {
                //Присоединяем их к основной строке
                content += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //Если мы находим символ конца сообщения ...
                if(content.IndexOf(".") > -1)
                {
                    Console.WriteLine("Read {0} bytes from socket. \n Data: {1}", content.Length, content);
                    byte[] byteData = Encoding.ASCII.GetBytes(content);

                    //Отправляем данные обратно клиенту
                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
                }
                else
                {
                    //иначе получаем оставшиеся данные
                    handler.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), handler);
                }
            }
        }
        public static void SendCallback(IAsyncResult ar)
        {
            Console.WriteLine("SendCallback Thread ID:" + AppDomain.GetCurrentThreadId());

            Socket handler = (Socket)ar.AsyncState;

            //Отправляем данные обратно клиенту
            int bytesSent = handler.EndSend(ar);

            Console.WriteLine("Sent {0} bytes to Client.", bytesSent);

            //Закрываем сокет
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            //устанавливаем событие для основного потока
            socketEvent.Set();
        }
    }
}
