namespace SerialDataCapturer
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_GetAllSerialPort = new System.Windows.Forms.Button();
            this.cbb_SerialPort = new System.Windows.Forms.ComboBox();
            this.btn_ConnectPort = new System.Windows.Forms.Button();
            this.cbb_BandRate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SaveData = new System.Windows.Forms.Button();
            this.cbb_DataType = new System.Windows.Forms.ComboBox();
            this.btn_SerialPortRead = new System.Windows.Forms.Button();
            this.cbb_SavePath = new System.Windows.Forms.ComboBox();
            this.tb_TipInfo = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.lb_DataTypeLabel = new System.Windows.Forms.Label();
            this.lb_CommandVerify = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_OpenXml = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_OpenFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_GetAllSerialPort
            // 
            this.btn_GetAllSerialPort.Location = new System.Drawing.Point(12, 36);
            this.btn_GetAllSerialPort.Name = "btn_GetAllSerialPort";
            this.btn_GetAllSerialPort.Size = new System.Drawing.Size(75, 23);
            this.btn_GetAllSerialPort.TabIndex = 0;
            this.btn_GetAllSerialPort.Text = "获取端口";
            this.btn_GetAllSerialPort.UseVisualStyleBackColor = true;
            this.btn_GetAllSerialPort.Click += new System.EventHandler(this.btn_GetAllSerialPort_Click);
            // 
            // cbb_SerialPort
            // 
            this.cbb_SerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_SerialPort.FormattingEnabled = true;
            this.cbb_SerialPort.Location = new System.Drawing.Point(106, 39);
            this.cbb_SerialPort.Name = "cbb_SerialPort";
            this.cbb_SerialPort.Size = new System.Drawing.Size(121, 20);
            this.cbb_SerialPort.TabIndex = 1;
            // 
            // btn_ConnectPort
            // 
            this.btn_ConnectPort.Location = new System.Drawing.Point(359, 39);
            this.btn_ConnectPort.Name = "btn_ConnectPort";
            this.btn_ConnectPort.Size = new System.Drawing.Size(74, 62);
            this.btn_ConnectPort.TabIndex = 2;
            this.btn_ConnectPort.Text = "连接端口";
            this.btn_ConnectPort.UseVisualStyleBackColor = true;
            this.btn_ConnectPort.Click += new System.EventHandler(this.btn_ConnectPort_Click);
            // 
            // cbb_BandRate
            // 
            this.cbb_BandRate.FormattingEnabled = true;
            this.cbb_BandRate.Location = new System.Drawing.Point(106, 81);
            this.cbb_BandRate.Name = "cbb_BandRate";
            this.cbb_BandRate.Size = new System.Drawing.Size(121, 20);
            this.cbb_BandRate.TabIndex = 3;
            this.cbb_BandRate.TextChanged += new System.EventHandler(this.cbb_BandRate_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "选择波特率：";
            // 
            // btn_SaveData
            // 
            this.btn_SaveData.Enabled = false;
            this.btn_SaveData.Location = new System.Drawing.Point(12, 192);
            this.btn_SaveData.Name = "btn_SaveData";
            this.btn_SaveData.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveData.TabIndex = 5;
            this.btn_SaveData.Text = "数据保存";
            this.btn_SaveData.UseVisualStyleBackColor = true;
            this.btn_SaveData.Click += new System.EventHandler(this.btn_SaveData_Click);
            // 
            // cbb_DataType
            // 
            this.cbb_DataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_DataType.FormattingEnabled = true;
            this.cbb_DataType.Location = new System.Drawing.Point(109, 131);
            this.cbb_DataType.Name = "cbb_DataType";
            this.cbb_DataType.Size = new System.Drawing.Size(118, 20);
            this.cbb_DataType.TabIndex = 6;
            this.cbb_DataType.SelectedIndexChanged += new System.EventHandler(this.cbb_DataType_SelectedIndexChanged);
            // 
            // btn_SerialPortRead
            // 
            this.btn_SerialPortRead.Enabled = false;
            this.btn_SerialPortRead.Location = new System.Drawing.Point(12, 131);
            this.btn_SerialPortRead.Name = "btn_SerialPortRead";
            this.btn_SerialPortRead.Size = new System.Drawing.Size(75, 23);
            this.btn_SerialPortRead.TabIndex = 7;
            this.btn_SerialPortRead.Text = "串口读取";
            this.btn_SerialPortRead.UseVisualStyleBackColor = true;
            this.btn_SerialPortRead.Click += new System.EventHandler(this.btn_SerialPortRead_Click);
            // 
            // cbb_SavePath
            // 
            this.cbb_SavePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_SavePath.FormattingEnabled = true;
            this.cbb_SavePath.Location = new System.Drawing.Point(109, 194);
            this.cbb_SavePath.Name = "cbb_SavePath";
            this.cbb_SavePath.Size = new System.Drawing.Size(324, 20);
            this.cbb_SavePath.TabIndex = 1;
            this.cbb_SavePath.SelectedIndexChanged += new System.EventHandler(this.cbb_SavePath_SelectedIndexChanged);
            // 
            // tb_TipInfo
            // 
            this.tb_TipInfo.Location = new System.Drawing.Point(14, 225);
            this.tb_TipInfo.Multiline = true;
            this.tb_TipInfo.Name = "tb_TipInfo";
            this.tb_TipInfo.Size = new System.Drawing.Size(419, 64);
            this.tb_TipInfo.TabIndex = 8;
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // lb_DataTypeLabel
            // 
            this.lb_DataTypeLabel.AutoSize = true;
            this.lb_DataTypeLabel.Location = new System.Drawing.Point(233, 136);
            this.lb_DataTypeLabel.Name = "lb_DataTypeLabel";
            this.lb_DataTypeLabel.Size = new System.Drawing.Size(41, 12);
            this.lb_DataTypeLabel.TabIndex = 9;
            this.lb_DataTypeLabel.Text = "label2";
            // 
            // lb_CommandVerify
            // 
            this.lb_CommandVerify.AutoSize = true;
            this.lb_CommandVerify.Location = new System.Drawing.Point(233, 164);
            this.lb_CommandVerify.Name = "lb_CommandVerify";
            this.lb_CommandVerify.Size = new System.Drawing.Size(41, 12);
            this.lb_CommandVerify.TabIndex = 9;
            this.lb_CommandVerify.Text = "label2";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.tsbtn_OpenXml,
            this.tsbtn_OpenFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(445, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "tsbtn_Info";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // tsbtn_OpenXml
            // 
            this.tsbtn_OpenXml.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_OpenXml.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_OpenXml.Image")));
            this.tsbtn_OpenXml.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_OpenXml.Name = "tsbtn_OpenXml";
            this.tsbtn_OpenXml.Size = new System.Drawing.Size(23, 22);
            this.tsbtn_OpenXml.Text = "toolStripButton1";
            this.tsbtn_OpenXml.Click += new System.EventHandler(this.tsbtn_OpenXml_Click);
            // 
            // tsbtn_OpenFolder
            // 
            this.tsbtn_OpenFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_OpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_OpenFolder.Image")));
            this.tsbtn_OpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_OpenFolder.Name = "tsbtn_OpenFolder";
            this.tsbtn_OpenFolder.Size = new System.Drawing.Size(23, 22);
            this.tsbtn_OpenFolder.Text = "toolStripButton1";
            this.tsbtn_OpenFolder.Click += new System.EventHandler(this.tsbtn_OpenFolder_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 306);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lb_CommandVerify);
            this.Controls.Add(this.lb_DataTypeLabel);
            this.Controls.Add(this.tb_TipInfo);
            this.Controls.Add(this.btn_SerialPortRead);
            this.Controls.Add(this.cbb_DataType);
            this.Controls.Add(this.btn_SaveData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbb_BandRate);
            this.Controls.Add(this.btn_ConnectPort);
            this.Controls.Add(this.cbb_SavePath);
            this.Controls.Add(this.cbb_SerialPort);
            this.Controls.Add(this.btn_GetAllSerialPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "串口数据抓取工具";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_GetAllSerialPort;
        private System.Windows.Forms.ComboBox cbb_SerialPort;
        private System.Windows.Forms.Button btn_ConnectPort;
        private System.Windows.Forms.ComboBox cbb_BandRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SaveData;
        private System.Windows.Forms.ComboBox cbb_DataType;
        private System.Windows.Forms.Button btn_SerialPortRead;
        private System.Windows.Forms.ComboBox cbb_SavePath;
        private System.Windows.Forms.TextBox tb_TipInfo;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label lb_DataTypeLabel;
        private System.Windows.Forms.Label lb_CommandVerify;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton tsbtn_OpenXml;
        private System.Windows.Forms.ToolStripButton tsbtn_OpenFolder;
    }
}

