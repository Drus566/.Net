using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace DownloadData
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        static void DownloadData()
        {
            WebClient client = new WebClient();
            byte[] urlData = client.DownloadData("http://www.dotnetforce.com");
            string data = Encoding.ASCII.GetString(urlData);
            Console.WriteLine(data);
        }

        static void DownloadFile()
        {
            string siteURL = "http://www.dotnetforce.com/images/logo11.gif";
            string fileName = "E:\\ASP.gif";

            WebClient client = new WebClient();

            Console.WriteLine("Downloading File \"{0}\" from \"{1}\".....\n\n",fileName, siteURL);

            //Копируем Web-ресурс и сохраняем его в текущей
            //папке файловой системы
            client.DownloadFile(siteURL, fileName);
            Console.WriteLine("Successfully Downloaded file \"{0}\" from \"{1}\"", fileName, siteURL);
            Console.WriteLine("\nDownloaded file saved in the following " + "file system folder:\n\t" + fileName);
        }

        //Поток для чтения
        static void OpenRead()
        {
            string siteURL = "http://www.rediff.com";

            WebClient client = new WebClient();

            Console.WriteLine("Start Downloading Data From \"{0}\".....\n\n", siteURL);

            //Копируем Web ресурс из RemoteURL
            Stream stmData = client.OpenRead(siteURL);
            StreamReader srData = new StreamReader(stmData);

            //Создаем файл
            FileInfo fiData = new FileInfo("E:\\Default.htm");
            StreamWriter st = fiData.CreateText();
            Console.WriteLine("Writting to the file...");

            //Записываем данные в файл
            st.WriteLine(srData.ReadToEnd());

            st.Close();
            stmData.Close();
        }

        //Отправка данных указанному URL
        static void OpenWrite()
        {
            string siteURL = "http://localhost/postsample.aspx";

            string uploadData = "Posted=True&X=Value";

            byte[] uploadArray = Encoding.ASCII.GetBytes(uploadData);

            WebClient client = new WebClient();
            Console.WriteLine("Uploading data to {0}...", siteURL);

            Stream stmUpload = client.OpenWrite(siteURL, "POST");
            stmUpload.Write(uploadArray, 0, uploadArray.Length);

            stmUpload.Close();
            Console.WriteLine("Successfully posted the data");
        }

        //Отправляет на сервер данные(массив байтов) без их кодирования
        static void UploadData()
        {
            string siteURL = "http://localhost/postsample.aspx";
            WebClient client = new WebClient();
            client.Credentials = System.Net.CredentialCache.DefaultCredentials;
            string uploadingString = "Hello Force...";

            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            byte[] sendData = Encoding.ASCII.GetBytes(uploadingString);
            Console.WriteLine("Uploading to {0}...", siteURL);

            byte[] recData = client.UploadData(siteURL, "POST", sendData);

            Console.WriteLine("\nResponse received was {0}", Encoding.ASCII.GetString(recData));
        }

        //Копирование файла на сервер "PUT"
        //Файл содержит метаданные, поэтому на стороне сервера
        //его нужно привести в порядок
        static void UploadFile()
        {
            string siteURL = "http://localhost/images/http.txt";
            string remoteResponse;

            WebClient client = new WebClient();

            //Информация учетной записи для подтверждения ее сервером для успешного копирования
            NetworkCredential cred = new NetworkCredential("username", "password", "domain");

            string fileName = "C:\\http.txt";
            Console.WriteLine("Uploading {0} to {1}...", fileName, siteURL);

            byte[] responseArray = client.UploadFile(siteURL, "PUT", fileName);

            remoteResponse = Encoding.ASCII.GetString(responseArray);
            Console.WriteLine(remoteResponse);
        }

        //копирует на сервер коллекцию пар имя-значение
        static void UploadValues()
        {
            string siteUrl = "http://localhost/Force/SiteContent.aspx";
            string remoteResponse;

            WebClient client = new WebClient();
            NameValueCollection appendURL = new NameValueCollection();

            appendURL.Add("Type", "14");
            appendURL.Add("Keyword", "WebService");
            Console.WriteLine("Uploading the Value pair");

            byte[] responseArray = client.UploadValues(siteUrl, "POST", appendURL);
            remoteResponse = Encoding.ASCII.GetString(responseArray);
            Console.WriteLine(remoteResponse);
        }
    }
}
