using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        static void SyncIO(){
           Console.WriteLine("Create or open file SyncDemo.txt");
           FileStream syncF = new FileStream("SyncDemo.txt",FileMode.OpenOrCreate);
           Console.WriteLine("Write char 'A'");
           syncF.WriteByte(Convert.ToByte('A'));
           Console.WriteLine("Write string ' is a first character.'");
           byte[] writeBytes = Encoding.ASCII.GetBytes(" is a first character.");
        // Write(Массив байтов для записи, позиция в массиве байтов с которой начнется запись, длина записываемых данных)
           syncF.Write(writeBytes,0,writeBytes.Length);
        // Seek(Устанавливаем указатель в начало файла) 
           syncF.Seek(0,SeekOrigin.Begin);
        // Читаем один байт syncF.ReadByte()
           Console.WriteLine("Read : First character is -> " + Convert.ToChar(syncF.ReadByte()));
        // Определяем массив для чтения
           byte[] readBuf = new byte[syncF.Length - 1];
        // Read(массив куда записываются байты для чтения, позиция чтения, длина данных для чтения)   
           syncF.Read(readBuf,0,(Convert.ToInt32(syncF.Length)) - 1);
        // Читаем записанный массив байтов  
           Console.WriteLine("The rest of the file is : " + Encoding.ASCII.GetString(readBuf));
        }

        //Чтение BufferedStream - промежуточная память
        static void readBufStream(Stream st){
            //Формируем BufferedStream
            BufferedStream bf = new BufferedStream(st);
            byte[] inData = new byte[st.Length];
            // Читаем и отображаем буферизованные данные
            bf.Read(inData,0,Convert.ToInt32(st.Length));
            Console.WriteLine(Encoding.ASCII.GetString(inData));
        }   

        // Для Сохранения данных в памяти используют MemoryStream
        static void memStreamDemo(){
            //Создаем пустой поток в памяти
            MemoryStream mS = new MemoryStream();
            byte[] memData = Encoding.ASCII.GetBytes("This will go in memory!");
            // Записываем данные
            mS.Write(memData,0,memData.Length);
            // Устанавливаем указатель в начало
            mS.Position = 0;
            byte[] inData = new byte[100];
            // Читаем из памяти 
            Console.WriteLine(mS.Read(inData, 0, 100));
            Console.WriteLine(Encoding.ASCII.GetString(inData));
            // WriteTo() нужен для записи всего содержимого потока в памяти в файловый поток
            Stream strm = new FileStream("C:\\Users\\Drus\\Desktop\\Test\\MemOutput.txt",FileMode.OpenOrCreate,FileAccess.Write);
            mS.WriteTo(strm);
        }
        // Чтение и запись двоичных данных Binary
        static void BinaryGetting(){
            double angle, sinAngle;
            FileStream fStream = new FileStream(@"D:\Develop\Projects\GitHub\Traning\NetworkingProgrammingCSharp\Sines.dat",FileMode.Create,FileAccess.Write);
            BinaryWriter bW = new BinaryWriter(fStream);
            for(int i = 0; i <= 90; i+=5){
                double angleRads = Math.PI * i / 180;
                sinAngle = Math.Sin(angleRads);
                bW.Write((double)i);
                bW.Write(sinAngle);
            }
            bW.Dispose();
            fStream.Dispose();
            FileStream frStream = new FileStream(@"D:\Develop\Projects\GitHub\Traning\NetworkingProgrammingCSharp\Sines.dat",FileMode.Open,FileAccess.Read);
            BinaryReader br = new BinaryReader(frStream);
            int endOfFile;
            do{
                endOfFile = br.PeekChar();
                // br.PeekChar возвращает -1 в конце файла
                if(endOfFile != -1){
                    angle = br.ReadDouble();
                    sinAngle = br.ReadDouble();
                    Console.WriteLine("{0} : {1}", angle, sinAngle);
                }
            }
            while(endOfFile != -1);
            br.Dispose();
            frStream.Dispose();
        }
        static void TextReader(){
            Stream fS = new FileStream("TextOut.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sReader = new StreamReader(fS);
            string data;
            int line = 0;
            while((data = sReader.ReadLine()) != null){
                Console.WriteLine("Line {0}: {1} : Position = {2}", ++line,data,sReader.BaseStream.Position);
            }
            // Устанавливаем позицию, используя поисковое свойство базового потока
            sReader.BaseStream.Seek(0,SeekOrigin.Begin);
            Console.WriteLine("* Reading entire file using ReadToEnd \n" + sReader.ReadToEnd());
            sReader.Dispose();
            fS.Dispose();
        }
        static void TextWriting(){
            Stream fS = new FileStream("TextOut1.txt", FileMode.CreateNew, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(fS);
            // Отображаем тип кодировки
            Console.WriteLine("Encoding type : " + sWriter.Encoding.ToString());
            // Отображаем провайдер формата
            Console.WriteLine("Format Provider : " + sWriter.FormatProvider.ToString());
            sWriter.WriteLine("Today is {0}.", DateTime.Today.DayOfWeek);
            sWriter.WriteLine("Today we will mostly be using StreamWritter");
            for(int i = 0; i < 5; i++){
                sWriter.WriteLine("Value {0}, its square is {1}",i,i*i);
            }
            sWriter.Write("Arrays can be written : ");
            char[] myArray = new char[] {'a','r','r','a','y'};

            sWriter.Write(myArray);
            sWriter.WriteLine("\r\nAnd parts of arrays can be written");
            sWriter.Write(myArray,0,3);

            sWriter.Dispose();
            fS.Dispose();
        }
    }
}
