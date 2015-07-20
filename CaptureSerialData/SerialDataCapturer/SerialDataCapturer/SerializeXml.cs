using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;


namespace SerialDataCapturer
{

    class SerializeXml
    {
        private static string _xmlPath = Environment.CurrentDirectory + @"\SerializeXml.xml";
        public static SerialDataCapture sdc = new SerialDataCapture();
        public static string XmlPath { get { return _xmlPath; } }
        public static bool isDeSerial = false;
        public static void ResetSerializeData()
        {
            List<FileName> fn0 = new List<FileName>{
                new FileName(){name="三轴G-Sensor", type="int16", cycle=3},
                new FileName(){name="三轴陀螺仪", type="int16", cycle=3},
            };
            List<FileName> fn1 = new List<FileName>{
                new FileName(){name="三轴G-Sensor", type="int16", cycle=3}
            };
            List<FileName> fn2 = new List<FileName>{
                new FileName(){name="旧三轴G-Sensor", type="int16", cycle=3},
            };
            List<FileName> fn3 = new List<FileName>{
                new FileName(){name="心率", type="int32", cycle=1}
            };
            List<FileName> fn4 = new List<FileName>{
                new FileName(){name="高度", type="int16", cycle=1},
                new FileName(){name="气压", type="int32", cycle=1},
            };
            List<SaveFile> sfList = new List<SaveFile>(){
                new SaveFile(){sgmt="Cycle"},
                new SaveFile(){sgmt="No"},
                new SaveFile(){sgmt="No"},
                new SaveFile(){sgmt="No"},
                new SaveFile(){sgmt="Byte"}
            };
            sfList[0].sfn = fn0;
            sfList[1].sfn = fn1;
            sfList[2].sfn = fn2;
            sfList[3].sfn = fn3;
            sfList[4].sfn = fn4;
            List<DataNode> dnList = new List<DataNode>(){
                new DataNode(){DataName="GetAccelerate6",desc="三轴G-Sensor+三轴陀螺仪",cmd="0x6e, 0x01, 0x08, 0x01, 0x8f"},
                new DataNode(){DataName="GetAccelerate3",desc="三轴G-Sensor",cmd="0x6e, 0x01, 0x07, 0x01, 0x8f"},
                new DataNode(){DataName="GetAccelerate",desc="旧三轴G-Sensor",cmd="0x24, 0x24, 0x08, 0x26, 0x26"},
                new DataNode(){DataName="GetHeartrate",desc="心率",cmd="0x6e, 0x01, 0x06, 0x01, 0x8f"},
                new DataNode(){DataName="GetAirPressure",desc="高度+气压",cmd="0x6e, 0x01, 0x05, 0x01, 0x8f"}
            };
            for (int i = 0; i < 5; i++)
            {
                dnList[i].sf = sfList[i];
            }
            List<SavePathNode> spnList = new List<SavePathNode>() {
                new SavePathNode() {name= "桌面"}, new SavePathNode() {name= "工具目录" }, new SavePathNode() {name= "浏览..." }};
            sdc.lsp = @"F:\新建文件夹";
            sdc.ldt = "GetAccelerate";
            sdc.br = "115200";
            sdc.dnList = dnList;
            sdc.spnList = spnList;
            SerializeData();
        }
        public static void SerializeData()
        {
            string xml = XmlHelper.XmlSerialize(sdc, Encoding.UTF8);
            File.WriteAllText(_xmlPath, xml);
        }
        public static void DeserializeData()
        {
            sdc = XmlHelper.XmlDeserializeFromFile<SerialDataCapture>(_xmlPath, Encoding.UTF8);
            isDeSerial = true;
        }
        public static List<string> GetallDataType()
        {
            List<string> list = new List<string>();
            if (!isDeSerial)
            {
                return list;
            }
            foreach (var node in sdc.dnList)
            {
                list.Add(node.DataName);
            }
            return list;
        }
        public static string GetDescription(string name)
        {
            if (!isDeSerial)
            {
                return string.Empty;
            }
            foreach (var node in sdc.dnList)
            {
                if (node.DataName==name)
                {
                    return node.desc;
                }
            }
            return string.Empty;
        }
        public static List<string> GetSavePath()
        {
            List<string> list = new List<string>();
            if (!isDeSerial)
            {
                return list;
            }
            foreach (var node in sdc.spnList)
            {
                list.Add(node.name);
            }
            return list;
        }
        public static string GetLastSavePath()
        {
            if (!isDeSerial)
            {
                return string.Empty;
            }
            return sdc.lsp;
        }
        public static bool SetLastSavePath(string path)
        {
            if (!isDeSerial)
            {
                return false;
            }
            sdc.lsp = path;
            SerializeData();
            return true;
        }
        public static string GetLastDataType()
        {
            if (!isDeSerial)
            {
                return string.Empty;
            }
            return sdc.ldt;
        }
        public static bool SetLastDataType(string type)
        {
            if (!isDeSerial)
            {
                return false;
            }
            sdc.ldt = type;
            SerializeData();
            return true;
        }
        public static Byte[] GetCommandByte(string commandName)
        {
            Byte[] command = new Byte[5];
            if (!isDeSerial)
            {
                return command;
            }
            foreach (var node in sdc.dnList)
            {
                if (node.DataName == commandName)
                {
                    string name = node.cmd;
                    name = name.Replace(" ", string.Empty);
                    name = name.Replace(",", string.Empty);
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
            if (!isDeSerial)
            {
                return baudrate;
            }
            return Convert.ToInt32(sdc.br);
        }
        public static bool SetBaudRate(int rate)
        {
            if (!isDeSerial)
            {
                return false;
            }
            sdc.br = rate.ToString();
            SerializeData();
            return true;
        }
        public static SegmentMethod GetSegentType(string dataType)
        {
            SegmentMethod method = new SegmentMethod();
            if (!isDeSerial)
            {
                return method;
            }
            foreach(var node in sdc.dnList)
            {
                if (node.DataName==dataType)
                {
                    string name = node.sf.sgmt;
                    if (name=="No")
                    {
                        method.type = SegmentType.No;
                        method.SegmentPartNum = 1;
                        method.filenames[0] = node.sf.sfn[0].name;
                        method.cycle[0] = node.sf.sfn[0].cycle;
                        method.dataWidth[0] = GetDataWidth(node.sf.sfn[0].type);
                    }
                    else 
                    {
                        if(name=="Cycle")
                        {
                            method.type = SegmentType.Cycle;
                        }
                        else
                        {
                            method.type = SegmentType.Byte;
                        }
                        method.SegmentPartNum = node.sf.sfn.Count;
                        for (int i = 0; i < node.sf.sfn.Count; i++)
                        {
                            method.filenames[i] = node.sf.sfn[i].name;
                            if (i>0)
                            {
                                method.cycle[i] = node.sf.sfn[i].cycle+node.sf.sfn[i-1].cycle;
                            }
                            else
                            {
                                method.cycle[0] = node.sf.sfn[0].cycle;
                            }
                            method.dataWidth[i] = GetDataWidth(node.sf.sfn[i].type);
                        }
                    }
                }
            }
            return method;
        }
        public static DataWidth GetDataWidth(string str)
        {
            DataWidth dtw = new DataWidth();
            switch (str)
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

    [XmlRoot("SerialDataCapture")]
    public class SerialDataCapture
    {
        [XmlArrayItem("Node")]
        [XmlArray("DataType")]
        public List<DataNode> dnList { set; get; }
        [XmlArrayItem("Node")]
        [XmlArray("SavePath")]
        public List<SavePathNode> spnList { set; get; }

        [XmlElement("LastSavePath")]
        public string lsp {get;set;}
        [XmlElement("LastDataType")]
        public string ldt {get;set;}
        [XmlElement("BaudRate")]
        public string br {get;set;}
    }
    [XmlType("DataNode")]
    public class DataNode
    {
        [XmlAttribute("name")]
        public string DataName { set; get; }
        [XmlAttribute("description")]
        public string desc { set; get; }
        [XmlElement("Command")]
        public string cmd { set; get; }
        [XmlElement("SaveFile")]
        public SaveFile sf { set; get; }
    }
    [XmlType("SaveFile")]
    public class SaveFile
    {
        [XmlElement("Segment")]
        public string sgmt { set; get; }
        [XmlElement("FileName")]
        public List<FileName> sfn { set; get; }
    }
    [XmlType("FileName")]
    public class FileName
    {
        [XmlAttribute("name")]
        public string name { set; get; }
        [XmlAttribute("type")]
        public string type { set; get; }
        [XmlAttribute("cycle")]
        public int cycle { set; get; }
    }
    [XmlType("Node")]
    public class SavePathNode
    {
        [XmlAttribute("name")]
        public string name { set; get; }
    }


}
