using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace AsyncClient
{
    class Program
    {
        public static string theResponse = "";
        public static byte[] buffer = new byte[1024];

        // Создаем три сигнальные события для каждой задачи
        public static ManualResetEvent ConnectDone = new ManualResetEvent(false);
        public static ManualResetEvent SendDone = new ManualResetEvent(false);
        public static ManualResetEvent ReceiveDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            try
            {
                Thread thr = Thread.CurrentThread;
                Console.WriteLine("Main Thread State:" + thr.ThreadState);

                IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ipAddr, 11000);
                Socket sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Устанавливаем соединение с сервером
                sClient.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), sClient);

                // Ждем установления ConnectDone.Set()
                ConnectDone.WaitOne();
                
                //Создаем сообщение
                string data = "This is a test";
                for(int i = 0; i < 72; i++)
                {
                    data += i.ToString() + ":" + (new string('=',i)); 
                }
                byte[] byteData = Encoding.ASCII.GetBytes(data + "<TheEnd>");

                //Асинхронно запускаем отправку данных на сервер
                sClient.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), sClient);

                //Имитируем вычисления, которые пожирают процессорное время на практике
                for(int i = 0; i < 5; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(10);
                }

                //Ждем SendDone.Set()
                SendDone.WaitOne();

                //Асинхронное получение данных от сервера
                sClient.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sClient);

                ReceiveDone.WaitOne();

                Console.WriteLine("Response received: {0} ", theResponse);
                Console.ReadLine();

                sClient.Shutdown(SocketShutdown.Both);
                sClient.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine("ReceiveCallback Thread State:" + thr.ThreadState);

            Socket sClient = (Socket)ar.AsyncState;

            //Возвращает число полученных байтов
            int bytesRead = sClient.EndReceive(ar);

            //Проверяем остались ли еще данные в очереди
            if(bytesRead > 0)
            {
                theResponse += Encoding.ASCII.GetString(buffer, 0, bytesRead);
                sClient.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), sClient);
            }
            else
            {
                ReceiveDone.Set();
            }
        }

        public static void SendCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine("SendCallback Thread State:" + thr.ThreadState);

            Socket sClient = (Socket)ar.AsyncState;

            // Заканчивает асинхронный запрос на отправку данных
            int bytesSent = sClient.EndSend(ar);

            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            SendDone.Set();
        }

        //Обратный вызов метода соединения с сервером
        public static void ConnectCallback(IAsyncResult ar)
        {
            Thread thr = Thread.CurrentThread;
            Console.WriteLine("ConnectCallback Thread State:" + thr.ThreadState);

            // Получаем сокет и приравниваем его к третьему параметру из метода BeginConnect()
            Socket sClient = (Socket)ar.AsyncState;

            // Вызываем EndConnect чтобы завершить асинхронный запрос
            sClient.EndConnect(ar);
            Console.WriteLine("Socket connected to {0}", sClient.RemoteEndPoint.ToString());
            ConnectDone.Set();
        }

    }
}
