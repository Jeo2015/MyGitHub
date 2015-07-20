using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace SerialDataCapturer
{
    public partial class Form1 : Form
    {
        private string _savePath = Environment.CurrentDirectory;
        private string _lastSavePath;
        private Byte[] _sendCommand = new Byte[5];
        private string _sendCommandString = string.Empty;
        private int _baudRate;
        private byte[] receivedData = new byte[4096];
        List<byte> listData = new List<byte>();
        private string _serialType;
        private string _description = string.Empty;
        private volatile bool _shouldStop;
        //Thread thread_status = null;
        string _lastDataType = string.Empty;
        private static bool g_ConnectedFlag = false;
        public Form1()
        {
            try
            {
                InitializeComponent();
                //SerializeXml.LoadXml();
                SerializeXml.DeserializeData();

                _baudRate = SerializeXml.GetBaudRate();
                this.cbb_BandRate.Items.Add(_baudRate.ToString());
                this.cbb_BandRate.SelectedIndex = 0;

                List<string> dataType = SerializeXml.GetallDataType();
                this.cbb_DataType.Items.AddRange(dataType.ToArray());
                _lastDataType = SerializeXml.GetLastDataType();
                if (_lastDataType == "")
                {
                    this.cbb_DataType.SelectedIndex = 0;
                }
                else
                {
                    _serialType = _lastDataType;
                    this.cbb_DataType.SelectedItem = _serialType;
                }
                List<string> paths = SerializeXml.GetSavePath();
                this.cbb_SavePath.Items.AddRange(paths.ToArray());
                _lastSavePath = SerializeXml.GetLastSavePath();
                if (!System.IO.Directory.Exists(_lastSavePath))
                {
                    _lastSavePath = string.Empty;
                    SerializeXml.SetLastSavePath(_lastSavePath);
                    this.cbb_SavePath.SelectedIndex = 0;
                }
                else
                {
                    _savePath = _lastSavePath;
                    if (_lastSavePath == Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                    {
                        this.cbb_SavePath.SelectedIndex = 0;
                    }
                    else if (_lastSavePath == Environment.CurrentDirectory)
                    {

                        this.cbb_SavePath.SelectedIndex = 1;
                    }
                    else
                    {
                        this.cbb_SavePath.Items.Add(_lastSavePath);
                        this.cbb_SavePath.SelectedIndex = this.cbb_SavePath.Items.Count - 1;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        private void ShowMsg(string msg)
        {
            this.tb_TipInfo.Invoke(new Action(delegate()
                { tb_TipInfo.Text = msg; }));
            //Task task = new Task(new Action(() => { tb_TipInfo.Text = msg; }));
        }
        private void cbb_DataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _description = SerializeXml.GetDescription(this.cbb_DataType.Text);
                this.lb_DataTypeLabel.Text = "描述：" + _description;
                _sendCommand = SerializeXml.GetCommandByte(this.cbb_DataType.Text);
                _sendCommandString = BitConverter.ToString(_sendCommand);//.Replace("-", string.Empty);
                this.lb_CommandVerify.Text = "发送命令：" + _sendCommandString;
                _lastDataType = this.cbb_DataType.SelectedItem.ToString();
                SerializeXml.SetLastDataType(_lastDataType);
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void cbb_BandRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _baudRate = Convert.ToInt32(this.cbb_BandRate.Text);
                SerializeXml.SetBaudRate(_baudRate);
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void cbb_SavePath_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.cbb_SavePath.Text)
                {
                    case "桌面":
                        _savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        break;
                    case "工具目录":
                        _savePath = Environment.CurrentDirectory;
                        break;
                    case "浏览...":
                        FolderBrowserDialog fb = new FolderBrowserDialog();
                        if (fb.ShowDialog() == DialogResult.OK)
                        {
                            _savePath = fb.SelectedPath;
                            if (_savePath == Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                            {
                                this.cbb_SavePath.SelectedIndex = 0;
                            }
                            else if (_savePath == Environment.CurrentDirectory)
                            {

                                this.cbb_SavePath.SelectedIndex = 1;
                            }
                            else
                            {
                                this.cbb_SavePath.Items.Add(_savePath);
                                this.cbb_SavePath.SelectedIndex = this.cbb_SavePath.Items.Count - 1;
                            }
                        }
                        else
                        {
                            _savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            this.cbb_SavePath.SelectedIndex = 0;
                        }
                        break;
                    default:
                        _savePath = this.cbb_SavePath.Text;
                        break;
                }
                if (_lastSavePath != _savePath)
                {
                    _lastSavePath = _savePath;
                    SerializeXml.SetLastSavePath(_lastSavePath);
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void btn_GetAllSerialPort_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                cbb_SerialPort.Items.Clear();
                foreach (string port in ports)
                {
                    cbb_SerialPort.Items.Add(port);
                }
                if (cbb_SerialPort.Items.Count > 0)
                {
                    cbb_SerialPort.SelectedIndex = 0;
                    ShowMsg("端口号获取成功！");
                    if (cbb_SerialPort.Items.Count > 1)
                    {
                        cbb_SerialPort.SelectedIndex = 1;
                    }
                }
                //重新连接
                if (g_ConnectedFlag == true)
                {
                    g_ConnectedFlag = false;
                    this.btn_ConnectPort.Text = "连接端口";
                    EnableAllButton(false);
                    this.cbb_BandRate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void btn_ConnectPort_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cbb_SerialPort.Text.ToLower().Contains("com")
                    && this.cbb_SerialPort.Text.ToLower() != "com1")
                {
                    if (g_ConnectedFlag)
                    {
                        this.btn_ConnectPort.Text = "连接端口";
                        g_ConnectedFlag = false;
                        EnableAllButton(false);
                        serialPort1.Close();
                        ShowMsg("端口关闭成功！");
                        this.cbb_BandRate.Enabled = true;
                    }
                    else
                    {
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.Close();
                        }
                        serialPort1.PortName = this.cbb_SerialPort.Text.Trim();
                        serialPort1.BaudRate = Convert.ToInt32(this.cbb_BandRate.Text);
                        this.btn_ConnectPort.Text = "关闭端口";
                        g_ConnectedFlag = true;
                        EnableAllButton(true);
                        serialPort1.Open();
                        ShowMsg("端口连接成功！");
                        this.cbb_BandRate.Enabled = false;
                    }
                }
                else
                {
                    ShowMsg("请先获取端口，且不能连接com1口！");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void EnableAllButton(bool flag)
        {
            try
            {
                this.btn_SerialPortRead.Enabled = flag;
                this.btn_SaveData.Enabled = flag;
                this.cbb_BandRate.Enabled = flag;
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
#region 接收数据
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    return;
                }
                int length = serialPort1.BytesToRead;
                receivedData = new byte[length];
                serialPort1.Read(receivedData, 0, length);                
                HandlerData(receivedData);
            }
            catch(Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        public void HandlerData(byte[] data)
        {
            try
            {
                this.Invoke(new Action(delegate()
                {                    
                    listData.AddRange(data);
                }));
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
#endregion
        private void btn_SerialPortRead_Click(object sender, EventArgs e)
        {
            try
            {
                _serialType = this.cbb_DataType.Text;
                if (!serialPort1.IsOpen || _serialType == "")
                {
                    ShowMsg("串口没打开，或者读取的数据类型选择错误");
                    return;
                }
                else
                {
                    SerialWrite();
                    ShowMsg("数据读取中...");
                }
                _shouldStop = false;
                //thread_status = new Thread(new ThreadStart(ThreadMonitorComPort));
                //thread_status.Start();
                Task task = new Task(new Action(() =>
                {
                    int count = listData.Count;
                    int equalCount = 0;
                    while (!_shouldStop)
                    {
                        count = listData.Count;
                        Thread.Sleep(100);
                        int nowcount = listData.Count;

                        if (count != nowcount)
                        {
                            ShowMsg(string.Format("数据读取中...已读取{0}B数据！", listData.Count));
                            continue;
                        }
                        else
                        {
                            equalCount++;
                        }
                        if (equalCount >= 20)
                        {
                            _shouldStop = true;
                        }
                    }
                    Thread.Sleep(200);
                    ShowMsg(string.Format("读取完成！共读取{0}B数据！", listData.Count));
                }));
                task.Start();
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        public void ThreadMonitorComPort()
        {
            try
            {
                int count = listData.Count;
                int equalCount = 0;
                while (true)
                {
                    count = listData.Count;
                    Thread.Sleep(100);
                    int nowcount = listData.Count;

                    if (count != nowcount)
                    {
                        continue;
                    }
                    else
                    {
                        equalCount++;
                    }
                    if (equalCount >= 20)
                    {
                        _shouldStop = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void SerialWrite(params int[] timeout)
        {
            try
            {
                int time = 2000;
                if (timeout != null && timeout.Length > 0)
                {
                    time = timeout[0];
                }
                serialPort1.WriteTimeout = time;

                if (serialPort1.IsOpen)
                {
                    try
                    {
                        listData.Clear();
                        serialPort1.Write(_sendCommand, 0, _sendCommand.Length);
                        ShowMsg("串口写入命令成功！");
                    }
                    catch (Exception ex)
                    {
                        ShowMsg(ex.Message);
                        return;
                    }
                }
                else
                {
                    ShowMsg("串口没有打开！");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        private void btn_SaveData_Click(object sender, EventArgs e)
        {
            SaveDataToFile();
            //switch (_serialType)
            //{
            //    case "GetAccelerate6":
            //        SaveData_Accelerate6();
            //        break;
            //    case "GetAccelerate3":
            //        SaveData_Accelerate3();
            //        break;
            //    case "GetHeartrate":
            //        Save_Heartrate();
            //        break;
            //    case "GetAirPressure":
            //        SaveData_AirPressure();
            //        break;
            //    default:
            //        ShowMsg("类型选择错误，请重选！");
            //        return;
            //}
            ShowMsg(string.Format("保存成功！\r\n位置：{0}", _savePath));
        }
        private void btn_OpenSavePath_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.IO.Directory.Exists(_lastSavePath))
                {
                    ShowMsg("文件路径不存在...");
                    return;
                }
                else
                {
                    System.Diagnostics.Process.Start(_savePath);
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        #region 保存数据
        private static bool FindBytes(Byte bt)
        {
            if (bt == 'r')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SaveDataToFile()
        {
            try
            {
                if (!System.IO.Directory.Exists(_savePath))
                {
                    ShowMsg("保存的路径可能不存在，请检查后重新保存！");
                    return;
                }
                SegmentMethod saveMethod = new SegmentMethod();
                saveMethod = SerializeXml.GetSegentType(_serialType);
                if (saveMethod.type == SegmentType.No)
                {
                    saveMethod.filenames[0] = _savePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + saveMethod.filenames[0] + ".txt";
                }
                else//(saveMethod.type==SegmentType.Cycle)
                {
                    for (int i = 0; i < saveMethod.SegmentPartNum; i++)
                    {
                        saveMethod.filenames[i] = _savePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + saveMethod.filenames[i] + ".txt";
                    }
                }

                int dataLength = listData.Count;
                if (dataLength < 1)
                {
                    ShowMsg("没有读到有效数据！");
                    return;
                }
                if (_serialType == "GetAccelerate")
                {
                    string writeStr = string.Empty;
                    string tempName = saveMethod.filenames[0];
                    writeStr = Encoding.Default.GetString(listData.ToArray());
                    writeStr = writeStr.Replace("$$9,", string.Empty);
                    writeStr = writeStr.Replace("&&", string.Empty);
                    string[] split = new string[] { "start" };
                    string[] subStrs = writeStr.Split(split, StringSplitOptions.None);
                    List<string> sublist = subStrs.ToList();
                    sublist.Remove("");
                    int num = sublist.Count;
                    if (num > 1)
                    {
                        for (int i = 0; i < num; i++)
                        {
                            tempName = saveMethod.filenames[0].Remove(saveMethod.filenames[0].IndexOf(".txt")) + string.Format("_第{0}条.txt", i+1);
                            File.WriteAllText(tempName, sublist[i].Substring(1));
                        }
                    }
                    else
                    {
                        File.WriteAllText(tempName, sublist[0].Substring(1));
                    }
                }
                else
                {
                    // rule
                    int cycle;
                    int begin = 0;
                    while (listData[begin] != 0x6e)
                    {
                        begin++;
                    }
                    for (cycle = 1; cycle < dataLength; cycle++)
                    {
                        if (listData[begin] == listData[begin + cycle])
                        {
                            break;
                        }
                    }
                    //逐条读取
                    int loops = dataLength / cycle;
                    int temp;
                    for (int i = 0; i < loops; i++)
                    {
                        if (saveMethod.type == SegmentType.No)
                        {
                            temp = 0;
                            if (saveMethod.dataWidth[0] == DataWidth._int16)
                            {
                                byte[] vl = { listData[i * cycle + begin + 2], listData[i * cycle + begin + 1] };
                                temp = BitConverter.ToInt16(vl, 0);
                            }
                            else
                            {
                                byte[] vl = { listData[i * cycle + begin + 4], listData[i * cycle + begin + 3], listData[i * cycle + begin + 2], listData[i * cycle + begin + 1] };
                                temp = BitConverter.ToInt32(vl, 0);
                            }
                            if ((i + 1) % saveMethod.cycle[0] == 0)
                            {
                                saveMethod.writeStr[0] += temp.ToString() + "\n";
                            }
                            else
                            {
                                saveMethod.writeStr[0] += temp.ToString() + ",";
                            }
                        }
                        else if (saveMethod.type == SegmentType.Cycle)
                        {
                            temp = 0;
                            int pos = i % saveMethod.cycle[saveMethod.SegmentPartNum - 1];
                            int part = 0;
                            while (pos >= saveMethod.cycle[part])
                            {
                                part++;
                            }
                            if (saveMethod.dataWidth[part] == DataWidth._int16)
                            {
                                byte[] vl = { listData[i * cycle + begin + 2], listData[i * cycle + begin + 1] };
                                temp = BitConverter.ToInt16(vl, 0);
                            }
                            else
                            {
                                byte[] vl = { listData[i * cycle + begin + 4], listData[i * cycle + begin + 3], listData[i * cycle + begin + 2], listData[i * cycle + begin + 1] };
                                temp = BitConverter.ToInt32(vl, 0);
                            }
                            if ((i + 1) % saveMethod.cycle[part] == 0)
                            {
                                saveMethod.writeStr[part] += temp.ToString() + "\n";
                            }
                            else
                            {
                                saveMethod.writeStr[part] += temp.ToString() + ",";
                            }
                        }
                        else
                        {
                            temp = 0;
                            for (int u = 0; u < saveMethod.SegmentPartNum; u++)
                            {
                                if (saveMethod.dataWidth[u] == DataWidth._int16)
                                {
                                    byte[] vl = { listData[i * cycle + begin + 2], listData[i * cycle + begin + 1] };
                                    temp = BitConverter.ToInt16(vl, 0);
                                }
                                else
                                {
                                    byte[] vl = { listData[i * cycle + begin + 4], listData[i * cycle + begin + 3], listData[i * cycle + begin + 2], listData[i * cycle + begin + 1] };
                                    temp = BitConverter.ToInt32(vl, 0);
                                }
                                if ((i + 1) % saveMethod.cycle[u] == 0)
                                {
                                    saveMethod.writeStr[u] += temp.ToString() + "\n";
                                }
                                else
                                {
                                    saveMethod.writeStr[u] += temp.ToString() + ",";
                                }
                            }
                        }
                    }
                    for (int k = 0; k < saveMethod.SegmentPartNum; k++)
                    {
                        File.WriteAllText(saveMethod.filenames[k], saveMethod.writeStr[k]);
                    }
                }
                listData.Clear();
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
                return;
            }
        }
        public void Save_Heartrate()
        {
            try
            {
                int len = listData.Count;
                if (len == 0) return;
                int RecordCount = len;
                int pageSize = 6;
                int PageCount = (int)Math.Ceiling((float)RecordCount / pageSize);
                int position = 0;
                StringBuilder sb = new StringBuilder();
                StringBuilder primitivedata = new StringBuilder();
                for (int i = 1; i <= PageCount; i++)
                {
                    if (position == (len - 1)) break;
                    byte a6e = listData[position++];
                    byte d1 = listData[position++];
                    byte d2 = listData[position++];
                    byte d3 = listData[position++];
                    byte d4 = listData[position++];
                    byte a8f = listData[position++];
                    int data = d2 * 65536 + d3 * 256 + d4;
                    primitivedata.AppendLine(string.Format("{0} {1} {2} {3} {4} {5}", HexString(a6e), HexString(d1), HexString(d2), HexString(d3), HexString(d4), HexString(a8f)));
                    sb.AppendLine(data.ToString());
                }
                string dir = _savePath;
                if (!Directory.Exists(_savePath))
                {
                    ShowMsg("路径不存在！");
                    return;
                }
                //2015-05-29 09-03-59   第1次
                string HandlerSavedFilePath = _savePath + "\\处理" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(HandlerSavedFilePath, sb.ToString().TrimEnd(','));

                string PrimitiveSavedFilePath = _savePath + "\\原始" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(PrimitiveSavedFilePath, primitivedata.ToString().TrimEnd(','));
                listData.Clear();
            }
            catch
            {

            }
        }
        public void SaveData_Accelerate6()
        {
            try
            {
                int len = listData.Count;
                if (len == 0) 
                    return;
                int RecordCount = len;
                int pageSize = 5;
                int PageCount = (int)Math.Ceiling((float)RecordCount / pageSize);
                int position = 0;
                StringBuilder SBGroup1 = new StringBuilder();
                StringBuilder SBGroup2 = new StringBuilder();
                List<short> G1X = new List<short>();
                List<short> G1Y = new List<short>();
                List<short> G1Z = new List<short>();

                List<short> G2X = new List<short>();
                List<short> G2Y = new List<short>();
                List<short> G2Z = new List<short>();

                for (int i = 1; i <= PageCount; i++)
                {
                    if (position == (len - 1)) break;
                    byte a6e = listData[position++];
                    byte d1 = listData[position++];
                    byte d2 = listData[position++];
                    byte d3 = listData[position++];
                    byte a8f = listData[position++];
                    byte[] d = { d2, d1 };
                    short data = BitConverter.ToInt16(d, 0);
                    switch (d3)
                    {
                        case 0x01: G1X.Add(data); break;
                        case 0x02: G1Y.Add(data); break;
                        case 0x03: G1Z.Add(data); break;
                        case 0x04: G2X.Add(data); break;
                        case 0x05: G2Y.Add(data); break;
                        case 0x06: G2Z.Add(data); break;
                    }

                }
                if (!Directory.Exists(_savePath))
                {
                    ShowMsg("路径不存在！");
                    return;
                }
                //合并数据1
                for (int i = 0; i < G1X.Count; i++)
                {
                    try
                    {
                        SBGroup1.AppendLine(string.Format("{0} {1} {2}", G1X[i], G1Y[i], G1Z[i]));
                    }
                    catch
                    {
                        continue;
                    }
                }
                //合并数据2
                for (int i = 0; i < G2X.Count; i++)
                {
                    try
                    {
                        SBGroup2.AppendLine(string.Format("{0} {1} {2}", G2X[i], G2Y[i], G2Z[i]));
                    }
                    catch
                    {
                        continue;
                    }
                }
                string SBGroup1FilePath = _savePath + "\\加速度" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(SBGroup1FilePath, SBGroup1.ToString().TrimEnd(','));

                string SBGroup2FilePath = _savePath + "\\陀螺仪" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(SBGroup2FilePath, SBGroup2.ToString().TrimEnd(','));
                listData.Clear();
            }
            catch
            {

            }
        }
        public void SaveData_Accelerate3()
        {
            try
            {
                int len = listData.Count;
                if (len == 0) return;
                int RecordCount = len;
                int pageSize = 5;
                int PageCount = (int)Math.Ceiling((float)RecordCount / pageSize);
                int position = 0;
                StringBuilder SBGroup1 = new StringBuilder();
                List<short> G1X = new List<short>();
                List<short> G1Y = new List<short>();
                List<short> G1Z = new List<short>();

                for (int i = 1; i <= PageCount; i++)
                {
                    if (position == (len - 1)) break;
                    byte a6e = listData[position++];
                    byte d1 = listData[position++];
                    byte d2 = listData[position++];
                    byte d3 = listData[position++];
                    byte a8f = listData[position++];
                    byte[] d = { d2, d1 };
                    short data = BitConverter.ToInt16(d, 0);
                    switch (d3)
                    {
                        case 0x01: G1X.Add(data); break;
                        case 0x02: G1Y.Add(data); break;
                        case 0x03: G1Z.Add(data); break;
                    }
                }
                string dir = _savePath;
                if (!Directory.Exists(_savePath))
                {
                    ShowMsg("路径不存在！");
                    return;
                }
                //合并数据1
                for (int i = 0; i < G1X.Count; i++)
                {
                    try
                    {
                        SBGroup1.AppendLine(string.Format("{0} {1} {2}", G1X[i], G1Y[i], G1Z[i]));
                    }
                    catch
                    {
                        continue;
                    }
                }
                string SBGroup1FilePath = _savePath + "\\" + this.lb_DataTypeLabel.Text + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(SBGroup1FilePath, SBGroup1.ToString().TrimEnd(','));

                listData.Clear();

                System.Diagnostics.Process.Start(dir);
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }
        public void SaveData_AirPressure()
        {
            try
            {
                /*                 6e b1 b2  高度/10b3 b4  b5 b6    大气压力8F                 */
                int len = listData.Count;
                if (len == 0 || len < 10) return;
                int RecordCount = len;
                int pageSize = 6;
                int PageCount = (int)Math.Ceiling((float)RecordCount / pageSize);
                int position = 0;

                StringBuilder sbHeight = new StringBuilder();
                StringBuilder sbAir = new StringBuilder();
                for (int i = 1; i <= PageCount; i++)
                {
                    if (len - position < 6) break;
                    byte a6e = listData[position++];
                    //2字节 高度
                    byte d1 = listData[position++];
                    byte d2 = listData[position++];
                    //四个字节 气压
                    byte d3 = listData[position++];
                    byte d4 = listData[position++];
                    byte d5 = listData[position++];
                    byte d6 = listData[position++];
                    byte a8f = listData[position++];
                    //高度 除以10保存
                    byte[] heightByte = { d2, d1 };
                    short height = BitConverter.ToInt16(heightByte, 0);
                    sbHeight.AppendLine(string.Format("{0}", (((float)height / 10.0f)).ToString("f1")));

                    //气压直接保存
                    byte[] airPreByte = { d6, d5, d4, d3 };
                    uint airPre = BitConverter.ToUInt32(airPreByte, 0);
                    sbAir.AppendLine(string.Format("{0}", airPre));
                }

                string dir = _savePath;
                if (!Directory.Exists(_savePath))
                {
                    ShowMsg("路径不存在！");
                    return;
                }
                string SBGroup1FilePath = _savePath + "\\" + this.lb_DataTypeLabel.Text + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(SBGroup1FilePath, sbHeight.ToString().TrimEnd(','));

                string SBGroup2FilePath = _savePath + "\\" + this.lb_DataTypeLabel.Text + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   第1次.txt";
                File.WriteAllText(SBGroup2FilePath, sbAir.ToString().TrimEnd(','));
                listData.Clear();

                System.Diagnostics.Process.Start(dir);
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }
        #endregion
        public string HexString(byte[] b)
        {
            string orders = "";
            foreach (var r in b)
            {
                string s = String.Format("{0:X}", r);
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                orders += " " + s;
            }
            return orders;
        }
        public string HexString(byte b)
        {

            string s = String.Format("{0:X}", b);
            if (s.Length == 1)
            {
                s = "0" + s;
            }

            return s;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string info = "版本 v2.0Beta： 该版本是在kuangzm开发的版本基础上改进而成，由qiaolj在2015-7-18开发完成。\r\n改进主要内容： 增加配置文件，将于工具本身无关的参数采用配置文件来外置，增强工具的通用性。";
            info += "\r\n版本 v2.0： 该版本解决了串口读取时丢失数据的问题。2015-7-20 23:58 qiaolj";
            MessageBox.Show(info);
        }

        private void tsbtn_OpenXml_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(SerializeXml.XmlPath))
            {
                ShowMsg("配置文件不存在...");
                return;
            }
            else
            {
                System.Diagnostics.Process.Start(SerializeXml.XmlPath);
            }
        }

        private void tsbtn_OpenFolder_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(_lastSavePath))
            {
                ShowMsg("文件路径不存在...");
                return;
            }
            else
            {
                System.Diagnostics.Process.Start(_savePath);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要覆盖原有配置文件？","Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SerializeXml.ResetSerializeData();

                MessageBox.Show("创建成功！");
            }
        }
    }

}
