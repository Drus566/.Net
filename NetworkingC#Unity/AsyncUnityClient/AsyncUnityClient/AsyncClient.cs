using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using UnityEngine;
using UnityEditor;

namespace AsyncUnityClient
{
    class Program
    {
        public string UserName { get; set; }
        public string Host { get; set; }
        public string Message { get; set; }
        public string Logs { get; set; }
        public int Port { get; set; }
        public bool Relay { get; set; }
        private static TcpClient client;
        private static NetworkStream stream;
        //Thread receiveThread;

        public static string theResponse = "";
        public static byte[] buffer = new byte[1024];

        // Создаем три сигнальные события для каждой задачи
        public static ManualResetEvent ConnectDone = new ManualResetEvent(false);
        public static ManualResetEvent SendDone = new ManualResetEvent(false);
        public static ManualResetEvent ReceiveDone = new ManualResetEvent(false);

        public void Connect()
        {
            try
            {
                client = new TcpClient();
      
                client.BeginConnect(Host, Port, new AsyncCallback(ConnectCallback),client);
                ConnectDone.WaitOne();

                stream = client.GetStream();

                string message = UserName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                Debug.Log(String.Format("Добро пожаловать, {0}", UserName));
                Logs = String.Format("Добро пожаловать, {0}", UserName) + "\r\n";

                ReceiveDone.WaitOne();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                Logs = ex.ToString();
            }
        }

        public void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
        }

        public void SendMessage()
        {
            try
            {
                Debug.Log(String.Format("Вы ввели сообщение: {0}", Message));
                Logs = String.Format("Вы ввели сообщение: {0}", Message) + "\r\n";
                string message = Message;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.BeginWrite(data, 0, data.Length, new AsyncCallback(SendCallback), client);
                SendDone.WaitOne();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Logs = ex.Message + "\r\n";
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            Socket sClient = (Socket)ar.AsyncState;

            //Возвращает число полученных байтов
            int bytesRead = sClient.EndReceive(ar);

            //Проверяем остались ли еще данные в очереди
            if (bytesRead > 0)
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
            // Заканчивает асинхронный запрос на отправку данных
            SendDone.Set();
        }

        //Обратный вызов метода соединения с сервером
        public static void ConnectCallback(IAsyncResult ar)
        {
            // Получаем сокет и приравниваем его к третьему параметру из метода BeginConnect()
            TcpClient client = (TcpClient)ar.AsyncState;

            // Вызываем EndConnect чтобы завершить асинхронный запрос
            client.EndConnect(ar);
            ConnectDone.Set();
        }
    }
}
