using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace PicturePackager
{
    class PicturePackager
    {
        protected PicturePackager()
        {
        }

        //По завершенному изображению создает сегменты изображения
        public static PicturePackage[] GetPicturePackages(string name, int id, Image picture)
        {
            return GetPicturePackages(name, id, picture);
        }

        //Возвращает сегменты изображения для завершенного изображения
        public static PicturePackage[] GetPicturePackages(string name, int id, Image picture, int segmentSize)
        {
            //Сохраняем изображение в массиве байтов
            MemoryStream stream = new MemoryStream();
            picture.Save(stream, ImageFormat.Jpeg);

            //Вычисляем число сегментов, на которые разбивается изображение
            int numberSegments = (int)stream.Position / segmentSize + 1;

            PicturePackage[] packages = new PicturePackage[numberSegments];

            //Создаем сегменты изображения
            int sourceIndex = 0;
            for(int i = 0; i < numberSegments; i++)
            {
                //Вычисляем размер буфера сегмента
                int bytesToCopy = (int)stream.Position - sourceIndex; 
                if(bytesToCopy > segmentSize)
                {
                    bytesToCopy = segmentSize;
                }
                byte[] segmentBuffer = new byte[bytesToCopy];
                //копируем stream с позиции sourceIndex в segmentBuffer с позиции 0 с длинной bytesToCopy
                Array.Copy(stream.GetBuffer(), sourceIndex, segmentBuffer, 0, bytesToCopy);
                packages[i] = new PicturePackage(name, id, i + 1, numberSegments, segmentBuffer);
                sourceIndex += bytesToCopy;
            }
            return packages;
        }

        //Метод возвращает завершенное изображение из переданных ему сегментов
        public static Image GetPicture(PicturePackage[] packages)
        {
            int fullSizeNeeded = 0;
            int numberPackages = packages[0].NumberOfSegments;
            int pictureId = packages[0].Id;

            //Вычисляем размер данных изображения и проверяем согласованность
            //идентификаторов изображения
            for(int i = 0; i < numberPackages; i++)
            {
                fullSizeNeeded += packages[i].SegmentBuffer.Length;
                if(packages[i].Id != pictureId)
                {
                    throw new ArgumentException("Inconsistent picture ids passed", "packages");
                }
            }

            //Объединяем сегменты в двоичный массив
            byte[] buffer = new byte[fullSizeNeeded];
            int destinationIndex = 0; 
            for(int i =0; i < numberPackages; i++)
            {
                int length = packages[i].SegmentBuffer.Length;
                Array.Copy(packages[i].SegmentBuffer, 0, buffer, destinationIndex, length);
                destinationIndex += length;
            }

            //Создаем объект image из двоичных данных
            MemoryStream stream = new MemoryStream(buffer);
            Image image = Image.FromStream(stream);

            return image;
        }
    }
}
