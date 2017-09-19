using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Threading;

namespace UdpFileSender
{
    class FileSender
    {
        private static FileDetails fileDet = new FileDetails();

        //Поля, связанные с UdpClient
        private static IPAddress remoteIPAddress;
        private const int remotePort = 5002;
        private static UdpClient sender = new UdpClient();
        private static IPEndPoint endPoint;

        //Объект FileStream
        private static FileStream fs;

        static void Main(string[] args)
        {
            try
            {
                //Получаем удаленный IP адрес и создаем IPEndPoint
                Console.WriteLine("Enter Remote IP address");
                remoteIPAddress = IPAddress.Parse(Console.ReadLine().ToString());
                endPoint = new IPEndPoint(remoteIPAddress, remotePort);

                //Получаем путь файла. Размер надо меньше 8к
                Console.WriteLine("Enter file path and name to send");
                fs = new FileStream(Console.ReadLine().ToString(), FileMode.Open, FileAccess.Read);

                if(fs.Length > 8192)
                {
                    Console.Write("This version transfers files with size < 8192 bytes");
                    sender.Close();
                    fs.Close();
                    return;
                }

                SendFileInfo();//Отправляем инфу о файле получателю
                Thread.Sleep(2000); //Wait 2 seconds
                SendFile(); // Sender the file
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void SendFileInfo()
        {
            // Get type and расширение
            fileDet.FILETYPE = fs.Name.Substring((int)fs.Name.Length - 3, 3);

            //Get length the file
            fileDet.FILESIZE = fs.Length;

            XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));

            MemoryStream stream = new MemoryStream();

            //Сериализуем объект 
            fileSerializer.Serialize(stream, fileDet);

            //Считываем поток в байты
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));

            Console.WriteLine("Sending file details...");

            //Отправляем информацию о файле
            sender.Send(bytes, bytes.Length, endPoint);
            stream.Close();
        }

        private static void SendFile()
        {
            //Создаем файловый поток
            byte[] bytes = new byte[fs.Length];

            //Поток переводим  в байты
            fs.Read(bytes, 0, bytes.Length);

            Console.WriteLine("Sending file... size = " + fs.Length + " bytes");
            try
            {
                sender.Send(bytes, bytes.Length, endPoint); //Посылаем файл
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                //Чистим объекты
                fs.Close();
                sender.Close();
            }
            Console.Read();
            Console.WriteLine("File sent successfully");
        }
    }

    [Serializable]
    public class FileDetails
    {
        public string FILETYPE = "";
        public long FILESIZE = 0;
    }
}
