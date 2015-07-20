using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SerialDataCapturer
{
    class XmlService
    {
        private static string _xmlPath = Environment.CurrentDirectory + @"\SerialDataCapture.xml";
        public static XmlDocument doc = new XmlDocument();
        private static bool _isXmlLoaded = false;
        public static string XmlPath { get { return _xmlPath; } }
        public static bool IsXmlLoaded
        { 
            get 
            {
                return _isXmlLoaded;
            } 
        }
        public static bool LoadXml()
        {
            try
            {
                doc.Load(_xmlPath);
                _isXmlLoaded = true;
                return true;
            }
            catch 
            {
                _isXmlLoaded = false;
                return false; 
            }
        }
        public static List<string> GetallDataType()
        {
            List<string> list = new List<string>();
            if (!IsXmlLoaded)
            {
                return list;
            }
            XmlElement root = doc.DocumentElement;
            foreach (var node in root.ChildNodes)
            {
                XmlElement xe = (XmlElement)node;//将子节点类型转换为XmlElement类型      
                if (xe.Name=="DataType")
                {
                    XmlElement dtRoot = (XmlElement)node;
                    foreach (XmlElement nd in dtRoot.ChildNodes)
                    {
                        list.Add(nd.GetAttribute("name"));
                    }
                }
            }
            return list;
        }
        public static string GetDescription(string name)
        {
            if (!IsXmlLoaded)
            {
                return string.Empty;
            }
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {
                XmlElement xe = (XmlElement)node;//将子节点类型转换为XmlElement类型                 
                if (xe.Name == "DataType")
                {
                    foreach(XmlElement nd in xe.ChildNodes)
                    {
                        if (nd.GetAttribute("name") == name)
                        {
                            return nd.GetAttribute("description");
                        }
                    }
                }
            }
            return string.Empty;
        }
        public static List<string> GetSavePath()
        {
            List<string> list = new List<string>();
            if (!IsXmlLoaded)
            {
                return list;
            }
            XmlElement root = doc.DocumentElement;
            foreach (var node in root.ChildNodes)
            {
                XmlElement xe = (XmlElement)node;//将子节点类型转换为XmlElement类型      
                if (xe.Name == "SavePath")
                {
                    XmlElement dtRoot = (XmlElement)node;
                    foreach (XmlElement nd in dtRoot.ChildNodes)
                    {
                        list.Add(nd.GetAttribute("name"));
                    }
                }
            }
            return list;
        }
        public static string GetLastSavePath()
        {
            XmlElement root = doc.DocumentElement;
            foreach (XmlElement node in root.ChildNodes)
            {
                if (node.Name == "LastSavePath")
                {
                    return node.InnerText;
                }
            }
            return string.Empty;
        }
        public static bool SetLastSavePath(string path)
        {
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name=="LastSavePath")
                {
                    node.InnerText = path;
                    doc.Save(_xmlPath);
                }
            }
            return true;
        }
        public static string GetLastDataType()
        {
            XmlElement root = doc.DocumentElement;
            foreach (XmlElement node in root.ChildNodes)
            {
                if (node.Name == "LastDataType")
                {
                    return node.InnerText;
                }
            }
            return string.Empty;
        }
        public static bool SetLastDataType(string type)
        {
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "LastDataType")
                {
                    node.InnerText = type;
                    doc.Save(_xmlPath);
                }
            }
            return true;
        }
        public static Byte[] GetCommandByte(string commandName)
        {
            Byte[] command = new Byte[5];
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("Command");
            foreach (XmlNode node in nodes)
            {
                XmlElement xe = (XmlElement)node.ParentNode;
                if (xe.GetAttribute("name") == commandName)
                {
                    string name = node.InnerText;
                    name = name.Replace(" ", string.Empty);
                    name = name.Replace(",",string.Empty);
                    //command = System.Text.Encoding.ASCII.GetBytes(name,);
                    for (int i = 0; i < 5; i++)
                    {
                        command[i] = Convert.ToByte(name.Substring(i * 4, 4), 16);
                    }
                }
            }
            return command;
        }
        public static int GetBaudRate()
        {
            int baudrate = 0;
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("BaudRate");
            foreach (XmlNode node in nodes)
            {
                baudrate = Convert.ToInt32(node.InnerText);
            }
            return baudrate;
        }
        public static bool SetBaudRate(int rate)
        {
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("BaudRate");
            foreach (XmlNode node in nodes)
            {
                node.InnerText = rate.ToString();
                doc.Save(_xmlPath);
            }
            return true;
        }
        public static SegmentMethod GetSegentType(string dataType)
        {
            SegmentMethod method = new SegmentMethod();

            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("SaveFile");
            foreach (XmlNode node in nodes)
            {
                XmlElement xep = (XmlElement)node.ParentNode;
                if (xep.GetAttribute("name") == dataType)
                {
                    string name = node.InnerText;
                    if (name=="No")
                    {
                        method.type = SegmentType.No;
                        method.SegmentPartNum = 1;
                        XmlElement xe = (XmlElement)node;
                        XmlElement xet = (XmlElement)xe.ChildNodes[1];
                        method.filenames[0] = xet.GetAttribute("name");
                        method.cycle[0] = Convert.ToInt32(xet.GetAttribute("cycle"));
                    }
                    if (name=="Cycle")
                    {
                        method.type = SegmentType.Cycle;
                        XmlElement xe = (XmlElement)node;
                        int sgmt = xe.ChildNodes.Count-1;
                        method.SegmentPartNum = sgmt;
                        for (int i=0; i<sgmt; i++)
                        {
                            XmlElement xet = (XmlElement)xe.ChildNodes[i+1];
                            method.filenames[i] = xet.GetAttribute("name");
                            if (i>0)
                            {
                                method.cycle[i] = Convert.ToInt32(xet.GetAttribute("cycle")) + method.cycle[i - 1];
                            }
                            else
                            {
                                method.cycle[i] = Convert.ToInt32(xet.GetAttribute("cycle"));
                            }
                            method.dataWidth[i] = GetDataWidth(xet.GetAttribute("type"));
                        }
                    }
                    if (name == "Byte")
                    {
                        method.type = SegmentType.Byte;
                        XmlElement xe = (XmlElement)node;
                        int sgmt = xe.ChildNodes.Count - 1;
                        method.SegmentPartNum = sgmt;
                        for (int i = 0; i < sgmt; i++)
                        {
                            XmlElement xet = (XmlElement)xe.ChildNodes[i + 1];
                            method.filenames[i] = xet.GetAttribute("name");
                            method.dataWidth[i] = GetDataWidth(xet.GetAttribute("type"));
                        }
                    }
                }
            }
            return method;
        }
        public static DataWidth GetDataWidth(string str)
        {
            DataWidth dtw = new DataWidth();
            switch(str)
            {
                case "int16":
                    dtw = DataWidth._int16;
                    break;
                case "int32":
                    dtw = DataWidth._int32;
                    break;
            }
            return dtw;
        }
    }


}
