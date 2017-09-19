using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace EchoClient
{
    class Program
    {
        const int ECHO_PORT = 8080;

        static void Main(string[] args)
        {
            Console.Write("Your UserName");
            string userName = Console.ReadLine();
            Console.WriteLine("--Logged In-->");

            try
            {
                // Создаем соединение с ChatServer;
                TcpClient eClient = new TcpClient("127.0.0.1", ECHO_PORT);

                //Создаем классы потоков
                StreamReader readerStream = new StreamReader(eClient.GetStream());
                NetworkStream writerStream = eClient.GetStream();

                string dataToSend;

                dataToSend = userName;
                dataToSend += "\r\n";

                //Отправляем имя пользователя на сервер
                byte[] data = Encoding.ASCII.GetBytes(dataToSend);

                writerStream.Write(data, 0, data.Length);

                while (true)
                {
                    Console.Write(userName + ":");

                    //Считываем строку с сервера
                    dataToSend = Console.ReadLine();
                    dataToSend += "\r\n";

                    data = Encoding.ASCII.GetBytes(dataToSend);
                    writerStream.Write(data, 0, data.Length);

                    //Если отправлена команда Quit, выйти из приложения
                    if (dataToSend.IndexOf("QUIT") > -1)
                        break;
                    string returnData;

                    //Получить ответ от сервера
                    returnData = readerStream.ReadLine();

                    Console.WriteLine("Server: " + returnData);
                }
                //Закрыть TcpClient
                eClient.Close();
            }
            catch(Exception exp)
            {
                Console.WriteLine("Exception: " + exp);
            }
        }
    }
}
