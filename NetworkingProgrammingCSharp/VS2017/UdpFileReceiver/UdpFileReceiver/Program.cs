using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace UdpFileReceiver
{
    class FileReceiver
    {
        private static FileDetails fileDet;

        //UdpClient
        private static int localPort = 5002;
        private static UdpClient receivingUdpClient = new UdpClient(localPort);
        private static IPEndPoint remoteIpEndPoint = null;
        private static FileStream fs;
        private static byte[] receiveBytes = new byte[0];

        [STAThread]
        static void Main(string[] args)
        {
            //Получаем инфу о файле
            GetFileDetails();

            //Get the file
            ReceiveFile();
        }

        private static void GetFileDetails()
        {
            try
            {
                Console.WriteLine("Waiting to get File Details!!!");
                //Get info about the file
                receiveBytes = receivingUdpClient.Receive(ref remoteIpEndPoint);

                Console.WriteLine("Received File Details");

                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));

                MemoryStream stream1 = new MemoryStream();

                //Get bytes -> in stream
                stream1.Write(receiveBytes, 0, receiveBytes.Length);
                stream1.Position = 0; //Обязательно

                //Вызываем метод Deserialize и приводим к типу объекта
                fileDet = (FileDetails)fileSerializer.Deserialize(stream1);
                Console.WriteLine("Received file of type " + fileDet.FILETYPE + " whose size is " + fileDet.FILESIZE.ToString() + " bytes");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ReceiveFile()
        {
            try
            {
                Console.WriteLine("Waiting to get file");

                //Get the file
                receiveBytes = receivingUdpClient.Receive(ref remoteIpEndPoint);

                //Преобразуем и отображаем данные
                Console.WriteLine("File received...Saving...");

                //Создаем файл temp с полученным расширением
                fs = new FileStream("temp." + fileDet.FILETYPE, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(receiveBytes, 0, receiveBytes.Length);

                Console.WriteLine("File saved...");
                Console.WriteLine("Openning file with associated program");

                Process.Start(fs.Name); // Открываем файл связанной с ним программой
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                fs.Close();
                receivingUdpClient.Close();
            }
        }
    }

    [Serializable]
    public class FileDetails
    {
        public string FILETYPE = "";
        public long FILESIZE = 0;
    }
}
