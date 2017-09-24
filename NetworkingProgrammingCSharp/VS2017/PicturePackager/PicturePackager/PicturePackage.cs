using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace PicturePackager
{
    //Пакеты изображений
    public class PicturePackage
    {
        private string name;
        private int id;
        private int segmentNumber;
        private int numberOfSegments;
        private byte[] segmentBuffer;

        public string Name { get { return name; } }
        public int Id { get { return id; } }
        public int SegmentNumber { get { return segmentNumber; } }
        public int NumberOfSegments { get { return numberOfSegments; } }
        public byte[] SegmentBuffer { get { return segmentBuffer; } }

        //Конструктор создает сегмент изображения из типов данных 
        //Используется серверным приложением
        public PicturePackage(string name, int id, int segmentNumber, int numberOfSegments, byte[] segmentBuffer)
        {
            this.name = name;
            this.id = id;
            this.segmentNumber = segmentNumber;
            this.numberOfSegments = numberOfSegments;
            this.segmentBuffer = segmentBuffer;
        }

        //Создает сегмент изображения по XML коду
        //Используется клиентским приложением
        public PicturePackage(XmlDocument xml)
        {
            XmlNode rootNode = xml.SelectSingleNode("PicturePackage");
            id = int.Parse(rootNode.Attributes["Number"].Value);

            XmlNode nodeName = rootNode.SelectSingleNode("Name");
            this.name = nodeName.InnerXml;

            XmlNode nodeData = rootNode.SelectSingleNode("Data");
            numberOfSegments = int.Parse(nodeData.Attributes["SegmentNumber"].Value);

            int size = int.Parse(nodeData.Attributes["Size"].Value);
            segmentBuffer = Convert.FromBase64String(nodeData.InnerXml);

            //Пример XML документа
            // <PicturePackage Number = "4">
            //      <Name>hello.jpg</Name>
            //      <Data SegmentNumber = "2" LastSegmentNumber = "12" Size = "2400">
            //          <!- base-64 encoded picture data ->
            //      </Data>
            // </PicturePackage>
        }

        //Возвращаем XML код, представляющий сегмент изображения
        public string GetXml()
        {
            XmlDocument doc = new XmlDocument();

            //Корневой элемент <Picture Package>
            XmlElement picturePackage = doc.CreateElement("PicturePackage");

            //<PicturePackage.Number="number"></PicturePackage>
            XmlAttribute pictureNumber = doc.CreateAttribute("Number");
            pictureNumber.Value = id.ToString();
            picturePackage.Attributes.Append(pictureNumber);

            //<Name>pictureName</Name>
            XmlElement pictureName = doc.CreateElement("Name");
            pictureName.InnerText = name;
            picturePackage.AppendChild(pictureName);

            //<Data SegmentNumber = "" Size = ""> (фрагмент  в кодировке base-64)
            XmlElement data = doc.CreateElement("Data");
            XmlAttribute numberAttr = doc.CreateAttribute("SegmentNumber");
            numberAttr.Value = segmentNumber.ToString();
            data.Attributes.Append(numberAttr);

            XmlAttribute lastNumberAttr = doc.CreateAttribute("LastSegmentNumber");
            lastNumberAttr.Value = numberOfSegments.ToString();
            data.Attributes.Append(lastNumberAttr);

            data.InnerText = Convert.ToBase64String(segmentBuffer);
            XmlAttribute sizeAttr = doc.CreateAttribute("Size");
            sizeAttr.Value = segmentBuffer.Length.ToString();
            data.Attributes.Append(sizeAttr);

            picturePackage.AppendChild(data);

            doc.AppendChild(picturePackage);

            return doc.InnerXml;
        }
    }
}
