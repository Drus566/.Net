using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            memStreamDemo();
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
    }
}
