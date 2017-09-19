using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;
using System.Text;

namespace EchoServer
{
    public class ClientHandler
    {
        public TcpClient clientSocket;

        public void RunClient()
        {
            //Создаем классы потоков
            StreamReader readerStream = new StreamReader(clientSocket.GetStream());
            NetworkStream writerStream = clientSocket.GetStream();

            string returnData = readerStream.ReadLine();
            string userName = returnData;

            Console.WriteLine("Welcome " + userName + "to the Server");

            while (true)
            {
                returnData = readerStream.ReadLine();

                if(returnData.IndexOf("QUIT") > -1)
                {
                    Console.WriteLine("Bye Bye " + userName);
                    break;
                }
                Console.WriteLine(userName + " : " + returnData);
                returnData += "\r\n";

                byte[] dataWrite = Encoding.ASCII.GetBytes(returnData);
                writerStream.Write(dataWrite, 0, dataWrite.Length);
            }
            clientSocket.Close();
        }
       
    }

    public class EchoServer
    {
        const int ECHO_PORT = 8080;
        public static int nClients = 0;

        static void Main(string[] args)
        {
            try
            {
                //Связываем сервер с локальным портом
                TcpListener clientListener = new TcpListener(ECHO_PORT);

                //Начинаем слушать
                clientListener.Start();

                Console.WriteLine("Waiting for connections...");

                while (true)
                {
                    //Даем согласие на соединение
                    TcpClient client = clientListener.AcceptTcpClient();

                    ClientHandler cHandler = new ClientHandler();

                    //Передаем значение объекту ClientHandler
                    cHandler.clientSocket = client;

                    //Создаем новый поток для клиента
                    Thread clientThread = new Thread(new ThreadStart(cHandler.RunClient));
                    clientThread.Start();
                }
                clientListener.Stop();
            }
            catch(Exception exp)
            {
                Console.WriteLine("Exception: " + exp);
            }
        }
    }
}
