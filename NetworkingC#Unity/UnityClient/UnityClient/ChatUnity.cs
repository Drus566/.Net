using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace UnityClient
{
    public class ChatUnity
    {
        public string UserName { get; set; }
        public string Host { get; set; }
        public string Message { get; set; }
        public string Logs { get; set; }
        public int Port { get; set; }
        public bool Relay{ get; set; }
        private static TcpClient client;
        private static NetworkStream stream;
        Thread receiveThread;

        public void Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(Host, Port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = UserName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                Debug.Log(String.Format("Добро пожаловать, {0}", UserName));
                Logs = String.Format("Добро пожаловать, {0}", UserName) + "\r\n";
            }
            catch (Exception ex)
            {
               Debug.Log(ex.Message);
               Logs = ex.Message + "\r\n";
            }
        }

        // отправка сообщений
        public void SendMessage()
        {
            try
            {
                Debug.Log(String.Format("Вы ввели сообщение: {0}", Message));
                Logs = String.Format("Вы ввели сообщение: {0}", Message) + "\r\n";
                string message = Message;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Logs = ex.Message + "\r\n";
            }
        }
        // получение сообщений
        public void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Debug.Log(message);//вывод сообщения
                    Logs = message + "\r\n";
                    Relay = true;
                }
                catch
                {
                    Debug.Log("Подключение прервано...");
                    Logs = "Подключение прервано..." + "\r\n";
                    Relay = true;
                    Disconnect();
                }
            }
        }

        public void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            receiveThread.Abort();
            receiveThread.Join(500);
            //Environment.Exit(0); //завершение процесса
        }
    }
}
