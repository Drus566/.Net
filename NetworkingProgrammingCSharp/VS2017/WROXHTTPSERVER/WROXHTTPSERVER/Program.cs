using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Xml;

namespace WROXHTTPSERVER
{
    public class Host : MarshalByRefObject
    {
        public string HandleRequest(string fileName)
        {
            StringWriter wr = new StringWriter();
            Console.WriteLine("The output from the {0} file", fileName);

            //Создаем Worker для выполнения файла aspx
            HttpWorkerRequest worker = new SimpleWorkerRequest(fileName, "", wr);

            //Выполняем страницу
            HttpRuntime.ProcessRequest(worker);
            return wr.ToString();
        }
        
    }

    public class ASPXHosting
    {
        public enum HostInfo { VirtualDirectory, Port}
        public string CreateHost(string fileName)
        {
            Host myHost = (Host)ApplicationHost.CreateApplicationHost(typeof(Host), "/", GetHostingInfo(ASPXHosting.HostInfo.VirtualDirectory));
            return myHost.HandleRequest(fileName);
        }

        public string GetHostingInfo(HostInfo InfoType)
        {
            string retVal = "";
            string xPath = "";

            try
            {
                //Установим выражение XPath для поиска узла VDir или Port
                //в зависимости от параметра, переданного в методе
                if (InfoType.Equals(HostInfo.VirtualDirectory))
                    xPath = "HostLocation/VDir";
                else if (InfoType.Equals(HostInfo.Port))
                    xPath = "HostLocation/Port";
                else
                    return "";

                //Загрузка XML файла 
                XmlDataDocument xDHost = new XmlDataDocument();
                xDHost.Load("data\\HostInfo.xml");

                //Выбираем соответствующий узел
                XmlNode node = xDHost.SelectSingleNode(xPath);
                //Получаем текстовое значение элемента
                retVal = node.InnerText.Trim();
            }
            catch (XmlException eXML)
            {
                Console.WriteLine("An ConfigFile Exception Occured : " + eXML.ToString());
            }

            return retVal;
        }
    }

    class WroxServer : MarshalByRefObject
    {
        //перечисление для HostInfo.xml
        public enum HostInfo { VirtualDirectory, Port }
        private TcpListener myListener;


        static void Main(string[] args)
        {
            WroxServer server = new WroxServer();
        }
        
        //Конструктор запускающий TcpListener на данном порту
        //И вызывает поток на методе StartListen()]
        public WroxServer()
        {
            try
            {
                //Начинаем слушать на данном порту
                myListener = new TcpListener(Int32.Parse(GetHostingInfo(WROXHTTPSERVER.WroxServer.HostInfo.Port)));

                myListener.Start();
                Console.WriteLine("Web Server Running... Press C to Stop...");

                //Начинаем поток, вызывающий метод 'StartListen'
                Thread thread = new Thread(new ThreadStart(StartListen));
                thread.Start();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Accept failed. Another process might be " + "bound to port " + WROXHTTPSERVER.WroxServer.HostInfo.Port.ToString());
            }
        }

        //Получаем информацию о порте из файла HostInfo.xml 
        //в этом методе
        public string GetHostingInfo(HostInfo InfoType)
        {
            string retVal = "";
            string xPath = "";

            try
            {
                //Установим выражение XPath для поиска узла VDir или Port
                //в зависимости от параметра, переданного в методе
                if (InfoType.Equals(HostInfo.VirtualDirectory))
                    xPath = "HostLocation/VDir";
                else if (InfoType.Equals(HostInfo.Port))
                    xPath = "HostLocation/Port";
                else
                    return "";

                //Загрузка XML файла 
                XmlDataDocument xDHost = new XmlDataDocument();
                xDHost.Load("data\\HostInfo.xml");

                //Выбираем соответствующий узел
                XmlNode node = xDHost.SelectSingleNode(xPath);
                //Получаем текстовое значение элемента
                retVal = node.InnerText.Trim();
            }
            catch (XmlException eXML)
            {
                Console.WriteLine("An ConfigFile Exception Occured : " + eXML.ToString());
            }

            return retVal;
        }

        //Если пользователь не предоставил имя файла, то этот метод
        //получает из файла default.xml имя файла по умолчанию. Принимает
        //путь каталога и ищет в этом каталоге. Если найден файл по умолчанию
        //то возвращает имя, иначе пустую строку
        public string GetTheDefaultFileName(string sLocalDirectory)
        {
            string sLine = "";
            try
            {
                //Загружаем XML документ
                XmlDataDocument xDFile = new XmlDataDocument();
                xDFile.Load("data\\Default.xml");

                //Выбираем все элементы <File>
                XmlNodeList fileNodes = xDFile.SelectNodes("Document/File");

                //Выполняем цикл по выбранным узлам, пока не найдем 
                //один из файлов по умолчанию
                foreach (XmlNode node in fileNodes)
                {
                    if (File.Exists(sLocalDirectory + node.InnerText.Trim()))
                    {
                        sLine = node.InnerText.Trim();
                        break;
                    }
                }
            }
            catch (XmlException eXML)
            {
                Console.WriteLine("An ConfigFile Exception Occured : " + eXML.ToString());
            }

            //Возвращаем имя файла, если существует файл по умолчанию
            //иначе возвращаем пустую строку
            if (File.Exists(sLocalDirectory + sLine))
                return sLine;
            else
                return "";
        }

        //проверяет расширение по информации о MIME типах в файле Mime.xml
        public string GetMimeType(string sRequestedFile)
        {
            string sMimeType = "";
            string sFileExt = "";
            string sMimeExt = "";

            //Преобразуем в строчные буквы
            sRequestedFile = sRequestedFile.ToLower();
            int iStartPos = sRequestedFile.IndexOf(".");
            sFileExt = sRequestedFile.Substring(iStartPos);

            try
            {
                //Загружаем файл Mime.xml для определения Mime-типа
                XmlDataDocument xDMime = new XmlDataDocument();
                xDMime.Load("data\\Mime.xml");

                //Выбираем элемент <Type>, имеющий родственный элемент <Ext>
                //с тем же значением, что и расширение файла
                string xPath = "Mime/Values/Type[../Ext='" + sFileExt + "']";
                XmlNode mimeNode = xDMime.SelectSingleNode(xPath);

                if (mimeNode != null)
                {
                    sMimeType = mimeNode.InnerText.Trim();

                    //Получаем значение предыдущего элемента <Ext>
                    sMimeExt = mimeNode.PreviousSibling.InnerText.Trim();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Exception Occurred : " + e.ToString());
            }

            if (sMimeExt == sFileExt)
                return sMimeType;
            else
                return "";
        }

        //отправка информации HTTP заголовка браузеру
        public void WriteHeader(string sHttpVersion, string sMIMEHeader, int iTotalBytes, string sStatusCode, ref Socket mySocket)
        {
            string sBuffer = "";

            //Если Mime тип не предоставлен, устанавливаем по умолчанию тип text/html
            if (sMIMEHeader.Length == 0)
            {
                sMIMEHeader = "text/html";
            }

            sBuffer = sBuffer + sHttpVersion + sStatusCode + "r\n";
            sBuffer = sBuffer + "Server: WroxServer\r\n";
            sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";
            sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
            sBuffer = sBuffer + "Content-Length: " + iTotalBytes + "\r\n\r\n";

            byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);
            SendToBrowser(bSendData, ref mySocket);
            Console.WriteLine("Total Bytes: " + iTotalBytes.ToString());
        }

        //отправляет информацию клиенту 
        public void SendToBrowser(string data, ref Socket socket)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(data), ref socket);
        }

        public void SendToBrowser(Byte[] bSendData, ref Socket socket)
        {
            int iNumByte = 0;
            try
            {
                if (socket.Connected)
                {
                    if ((iNumByte = socket.Send(bSendData, bSendData.Length, 0)) == -1)
                    {
                        Console.WriteLine("Socket error: cannot send packet");
                    }
                    else
                    {
                        Console.WriteLine("No of bytes send {0}", iNumByte);
                    }
                }
                else
                {
                    Console.WriteLine("Connected Dropped...");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred : {0} ", e);
            }
        }

        //дает согласие на установление соединения 
        //обрабатывает запрос от клиента и отпарвляет
        //ему ответ на основании запроса
        public void StartListen()
        {
            int iStartPos = 0;
            string sRequest;
            string sDirName;
            string sRequestedFile;
            string sErrorMessage;
            string sLocalDir;

            //получаем информацию о Virtual Dir
            string sWebServerRoot = GetHostingInfo(WROXHTTPSERVER.WroxServer.HostInfo.VirtualDirectory);
            string sPhysicalFilePath = "";
            string sFormattedMessage = "";
            string sResponse = "";

            while (true)
            {
                //Принимаем новое соединение
                Socket socket = myListener.AcceptSocket();
                Console.WriteLine("Socket Type " + socket.SocketType);
                if (socket.Connected)
                {
                    Console.WriteLine("\nClient Connected!\n" + "========\nClient IP {0}\n", socket.RemoteEndPoint);
                    //Make a byte array and receive data from the client
                    byte[] bReceive = new byte[1024];
                    int i = socket.Receive(bReceive, bReceive.Length, 0);

                    //Преобразуем байты в строку
                    string sBuffer = Encoding.ASCII.GetString(bReceive);

                    //Убедимся что используем HTTP
                    iStartPos = sBuffer.IndexOf("HTTP", 1);

                    //Получаем текст "HTTP" и версию, например "HTTP/1.1"
                    string sHttpVersion = sBuffer.Substring(iStartPos, 8);

                    sRequest = sBuffer.Substring(0, iStartPos - 1);
                    //Заменяем обратные слеши
                    sRequest.Replace("\\", "/");

                    //Если имя файла не указано, добавляем прямой слеш,
                    //чтобы указать , что это каталог, а затем поищем
                    //имя файла, используемого по умолчанию
                    if ((sRequest.IndexOf(".") < 1) && (!sRequest.EndsWith("/")))
                        sRequest = sRequest + "/";

                    //Извлекаем имя запрошенного файла
                    iStartPos = sRequest.LastIndexOf("/") + 1;
                    sRequestedFile = sRequest.Substring(iStartPos);

                    //Извлекаем имя каталога
                    sDirName = sRequest.Substring(sRequest.IndexOf("/"), sRequest.LastIndexOf("/") - 3);

                    //определяем физический каталог
                    if (sDirName == "/")
                    {
                        sLocalDir = sWebServerRoot;
                    }
                    else
                    {
                        //Получаем виртуальный каталог
                        sDirName = sDirName.Replace(@"/", @"\");
                        sLocalDir = sWebServerRoot + sDirName;
                    }
                    Console.WriteLine("Directory Requested : " + sLocalDir);

                    //Определяем имя файла, если имя файла не представлено
                    //ищем в списке файлов по умолчанию
                    if(sRequestedFile.Length == 0)
                    {
                        //Получаем имя файла, используемого по умолчанию
                        sRequestedFile = GetTheDefaultFileName(sLocalDir);
                        if(sRequestedFile == "")
                        {
                            sErrorMessage = "<H2>Error!! No Default File Name " + "Specified</H2>";
                            WriteHeader(sHttpVersion, "", sErrorMessage.Length, "404 Not Found", ref socket);
                            SendToBrowser(sErrorMessage, ref socket);
                            socket.Close();
                            return;
                        }
                    }

                    //Получаем Mime тип
                    string sMimeType = GetMimeType(sRequestedFile);

                    //Строим физический путь
                    sPhysicalFilePath = sLocalDir + "\\" + sRequestedFile;
                    Console.WriteLine("File Requested : " + sPhysicalFilePath);
                    if(File.Exists(sPhysicalFilePath) == false)
                    {
                        sErrorMessage = "<H2>404 Error! File Does Not Exists...</H2>";
                        WriteHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref socket);
                        Console.WriteLine(sFormattedMessage);
                    }
                    else
                    {
                        string ucReqFile = sRequestedFile.ToUpper();

                        //Если запрошенный файл - страница ASP.NET
                        if(ucReqFile.Substring(ucReqFile.Length - 4).Equals("ASPX"))
                        {
                            //Создаем экземпляр класса ASPXGHosting
                            ASPXHosting host = new ASPXHosting();

                            //передаем файл ASPX, чтобы получить вывод HTML
                            string HTMLOut = host.CreateHost(sRequestedFile);
                            WriteHeader(sHttpVersion, sMimeType, HTMLOut.Length, " 200 OK", ref socket);
                            SendToBrowser(HTMLOut, ref socket);
                        }
                        else
                        {
                            int iToBytes = 0;
                            sResponse = "";
                            FileStream fs = new FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                            //Создаем reader, считывающий массив байтов
                            //из FileStream
                            BinaryReader reader = new BinaryReader(fs);
                            byte[] bytes = new byte[fs.Length];
                            int read;
                            while((read = reader.Read(bytes,0,bytes.Length)) != 0)
                            {
                                //читаем файл и записываем данные в сеть
                                sResponse = sResponse + Encoding.ASCII.GetString(bytes, 0, read);

                                iToBytes = iToBytes + read;
                            }
                            reader.Close();
                            fs.Close();
                            WriteHeader(sHttpVersion, sMimeType, iToBytes, "200 OK", ref socket);
                            SendToBrowser(bytes, ref socket);
                        }
                    }
                    socket.Close();
                }
            }
        }
    }
}
