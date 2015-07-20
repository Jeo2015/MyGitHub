using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SignalAnalysis
{
    public partial class Signal : Form
    {
        string _filePath = "";
        int _columnsCount = 0;
        static public List<LineData> dataList = new List<LineData>();
        DataTable dt = default(DataTable);
        public Signal()
        {
            InitializeComponent();
            //InitializeChart();
            this.chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart1_GetToolTipText);
            chart1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            //chart1.BackColor = Color.FromArgb(211, 223, 240);
            //this.BackColor = Color.White;
        }
        private void Signal_Load(object sender, EventArgs e)
        {
            //chart1.Series.
            
            dt = CreateDataTable();

            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea());
            //chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            //设置图表的数据源
            chart1.DataSource = dt;
            chart1.Series.Clear();
            chart1.PaletteCustomColors = new Color[] { Color.Black, Color.Red, Color.Blue, Color.Purple, Color.Yellow, Color.Green, Color.LightBlue, Color.Olive, Color.RosyBrown };
            //chart1.Palette = chart1.PaletteCustomColors.;
            int index = 0;
            while (index < _columnsCount)
            {
                chart1.Series.Add(string.Format("{0}", index));
                chart1.Series[index].ChartType = SeriesChartType.Line;
                chart1.Series[index].YValueMembers = string.Format("{0}", index);
                index++;
            }

            //设置图表Y轴对应项
            //chart1.Series[2].YValueMembers = "Volume3";
            //设置图表X轴对应项
            //chart1.Series[0].XValueMember = "Date";
            //绑定数据

            chart1.DataBind();

            //保存图片


            //chart1.SaveImage(@"e:\1.bmp ", ImageFormat.Bmp);


        }
        private void ReloadSignal()
        {
            //chart1.Series.
            DataTable dtPart = new DataTable();
            int i,j;
            // 一共几列
            int count = 0;
            for (i =0; i<dataList.Count; i++)
            {
                if (dataList[i].IsSelecte)
                {
                    dtPart.Columns.Add(string.Format("{0}",i));
                    count++;
                }
            }
            // 每列重新获取
            DataRow dr;
            DataRow ds;
            for (i = 0; i < dt.Rows.Count; i++ )
            {
                dr = dtPart.NewRow();
                ds = dt.Rows[i];
                for (j = 0; j<dataList.Count; j++)
                {
                    if (dataList[j].IsSelecte)
                    {
                        dr[string.Format("{0}", j)] = ds[string.Format("{0}", j)];
                    }
                }
                dtPart.Rows.Add(dr);
            }


            chart1.DataSource = dtPart;
            chart1.Series.Clear();
            //chart1.PaletteCustomColors = new Color[] { Color.Black, Color.Red, Color.Blue, Color.Purple, Color.Yellow, Color.Green, Color.LightBlue, Color.Olive, Color.RosyBrown };
            //chart1.Palette = chart1.PaletteCustomColors.;
            int index = 0;
            i = 0;
            while (index < _columnsCount)
            {
                if (dataList[index].IsSelecte)
                { 
                    chart1.Series.Add(string.Format("{0}", index));
                    chart1.Series[i].ChartType = SeriesChartType.Line;
                    chart1.Series[i].YValueMembers = string.Format("{0}", index);
                    chart1.Series[i].Color = chart1.PaletteCustomColors[index];
                    i++;
                }
                index++;
            }

            chart1.DataBind();

        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();

            if (!File.Exists(_filePath))
            {
                return dt;
            }
            //File.Open(_filePath, FileMode.Open);
            string[] info = File.ReadAllLines(_filePath);
            List<string> list = new List<string>(info);
            //找第一行
            while (list[0].IndexOf(',')<0)
            {
                list.RemoveAt(0);
            }
            //看一共几列
            string oneLine = list[0];
            int count = 0;
            while (oneLine.Length>0)
            {
                if (oneLine[0]==',')
                {
                    count++;
                }
                oneLine = oneLine.Substring(1);
            }
            count++;
            _columnsCount = count;
            dataList.Clear();
            int index = 0;
            while(index<count)
            {
                dt.Columns.Add(string.Format("{0}",index));
                dataList.Add(new LineData() { IsSelecte = true, Serier = string.Format("{0}",index), clr = this.chart1.PaletteCustomColors[index]});
                index++;
            }
            DataRow dr;
            //逐行添加
            int data = 0;
            string sData = "";
            while (list.Count>0)
            {
                dr = dt.NewRow();
                if (list[0].IndexOf(',')<0)
                {
                    break;
                }
                oneLine = list[0];
                oneLine = oneLine.Trim();
                oneLine = oneLine.Replace(" ","");
                oneLine += ",";
                index = 0;
                while(index<count)
                {
                    sData = oneLine.Substring(0, oneLine.IndexOf(','));
                    data = Convert.ToInt32(sData);
                    dr[string.Format("{0}",index)] = data;
                    oneLine = oneLine.Substring(data.ToString().Length+1);
                    index++;
                }
                dt.Rows.Add(dr);
                list.RemoveAt(0);
            }
            //dr = dt.NewRow();
            //dr["Date"] = "Apr";
            //dr["Volume1"] = 4466;
            //dr["Volume2"] = 5644;
            //dr["Volume3"] = 2000;
            //dt.Rows.Add(dr);

            return dt;
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // Zoom into the X axis

                chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(3, 4);

                //将滚动内嵌到坐标轴中
                //chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

                // 设置滚动条的大小
                //chart1.ChartAreas[0].AxisX.ScrollBar.Size = 10;

                // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
                //chart1.ChartAreas["Default"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyle.All;

                // 设置自动放大与缩小的最小量
                //chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
                //chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 2;
            }
            else
            {
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
            }
        }

        private void chart1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            //string s = DataFormats.FileDrop;
            _filePath = s[0];
            if (s.Count<string>() > 1)
            {
                MessageBox.Show(string.Format("只显示文件{0}，请打开多个程序拖入文件！",s[0]));
            }
            Signal_Load(null, null);
        }

        private void chart1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void InitializeChart()
        {
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();

            #region 设置图表的属性
            //图表的背景色
            chart1.BackColor = Color.FromArgb(211, 223, 240);
            //图表背景色的渐变方式
            chart1.BackGradientStyle = GradientStyle.TopBottom;
            //图表的边框颜色、
            chart1.BorderlineColor = Color.FromArgb(26, 59, 105);
            //图表的边框线条样式
            chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            //图表边框线条的宽度
            chart1.BorderlineWidth = 2;
            //图表边框的皮肤
            chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            #endregion

            #region 设置图表的标题
            Title title = new Title();
            //标题内容
            title.Text = "曲线图";
            //标题的字体
            title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            //标题字体颜色
            title.ForeColor = Color.FromArgb(26, 59, 105);
            //标题阴影颜色
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            //标题阴影偏移量
            title.ShadowOffset = 3;
            chart1.Titles.Add(title);
            #endregion

            #region 设置图例的属性
            //注意，需要把原来控件自带的图例删除掉
            this.chart1.Legends.Clear();

            Legend legend = new Legend("Default");
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Bottom;
            legend.LegendStyle = LegendStyle.Column;
            this.chart1.Legends.Add(legend);

            // Add header separator of type line
            legend.HeaderSeparator = LegendSeparatorStyle.Line;
            legend.HeaderSeparatorColor = Color.Gray;

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends["Default"].CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Name";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends["Default"].CellColumns.Add(secondColumn);

            // Add AVG cell column
            LegendCellColumn avgColumn = new LegendCellColumn();
            avgColumn.Text = "#AVG{N2}";
            avgColumn.HeaderText = "Avg";
            avgColumn.Name = "AvgColumn";
            avgColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends["Default"].CellColumns.Add(avgColumn);

            // Add Total cell column
            LegendCellColumn totalColumn = new LegendCellColumn();
            totalColumn.Text = "#TOTAL{N1}";
            totalColumn.HeaderText = "Total";
            totalColumn.Name = "TotalColumn";
            totalColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends["Default"].CellColumns.Add(totalColumn);

            // Set Min cell column attributes
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Text = "#MIN{N1}";
            minColumn.HeaderText = "Min";
            minColumn.Name = "MinColumn";
            minColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends["Default"].CellColumns.Add(minColumn);

            // Set Max cell column attributes
            LegendCellColumn maxColumn = new LegendCellColumn();
            maxColumn.Text = "#MAX{N1}";
            maxColumn.HeaderText = "Max";
            maxColumn.Name = "MaxColumn";
            maxColumn.HeaderBackColor = Color.WhiteSmoke;
            chart1.Legends["Default"].CellColumns.Add(maxColumn);

            #endregion

            #region 设置图表区属性
            ChartArea chartArea = new ChartArea("Default");
            //设置Y轴刻度间隔大小
            chartArea.AxisY.Interval = 5;
            //设置Y轴的数据类型格式
            //chartArea.AxisY.LabelStyle.Format = "C";
            //设置背景色
            chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            //设置背景渐变方式
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            //设置渐变和阴影的辅助背景色
            chartArea.BackSecondaryColor = Color.White;
            //设置边框颜色
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            //设置阴影颜色
            chartArea.ShadowColor = Color.Transparent;
            //设置X轴和Y轴线条的颜色
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            //设置X轴和Y轴线条的宽度
            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            //设置X轴和Y轴的标题
            chartArea.AxisX.Title = "时间";
            chartArea.AxisY.Title = "数值";
            //设置图表区网格横纵线条的颜色
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            //设置图表区网格横纵线条的宽度
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            //设置坐标轴刻度线不延长出来
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            //开启下面两句能够隐藏网格线条
            //chartArea.AxisX.MajorGrid.Enabled = false;
            //chartArea.AxisY.MajorGrid.Enabled = false;
            //设置X轴的显示类型及显示方式
            chartArea.AxisX.Interval = 0; //设置为0表示由控件自动分配
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;
            chartArea.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            //设置文本角度
            //chartArea.AxisX.LabelStyle.Angle = 45;
            //设置文本自适应
            chartArea.AxisX.IsLabelAutoFit = true;
            //设置X轴允许拖动放大
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0;
            chartArea.CursorX.IntervalOffset = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Minutes;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScrollBar.IsPositionedInside = false;

            //设置中短线（还没看到效果）
            //chartArea.AxisY.ScaleBreakStyle.Enabled = true;
            //chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            //chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            //chartArea.AxisY.ScaleBreakStyle.Spacing = 2;
            //chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
            //chartArea.AxisY.ScaleBreakStyle.LineWidth = 10;

            chart1.ChartAreas.Add(chartArea);
            #endregion

            //线条2：主要曲线
            Series series = new Series("Default");
            //设置线条类型
            series.ChartType = SeriesChartType.Line;
            //线条宽度
            series.BorderWidth = 1;
            //阴影宽度
            series.ShadowOffset = 0;
            //是否显示在图例集合Legends
            series.IsVisibleInLegend = true;
            //线条上数据点上是否有数据显示
            series.IsValueShownAsLabel = true;
            //线条颜色
            series.Color = Color.MediumPurple;
            //设置曲线X轴的显示类型
            series.XValueType = ChartValueType.DateTime;
            //设置数据点的类型
            series.MarkerStyle = MarkerStyle.Circle;
            //线条数据点的大小
            series.MarkerSize = 5;
            chart1.Series.Add(series);

            //手动构造横坐标数据
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TheTime", typeof(DateTime)); //注意typeof
            dataTable.Columns.Add("TheValue", typeof(double)); //注意typeof
            Random random = new Random(); //随机数
            DateTime dateTime = System.DateTime.Now;
            for (int n = 0; n < 3; n++)
            {
                dateTime = dateTime.AddSeconds(10);
                DataRow dr = dataTable.NewRow();
                dr["TheTime"] = dateTime;
                dr["TheValue"] = random.Next(0, 101);
                dataTable.Rows.Add(dr);
            }
            for (int n = 3; n < 1000; n++)
            {
                dateTime = dateTime.AddSeconds(30);
                DataRow dr = dataTable.NewRow();
                dr["TheTime"] = dateTime;
                dr["TheValue"] = random.Next(0, 101);
                dataTable.Rows.Add(dr);
            }

            //线条1：下限横线
            Series seriesMin = new Series("Min");
            seriesMin.ChartType = SeriesChartType.Line;
            seriesMin.BorderWidth = 1;
            seriesMin.ShadowOffset = 0;
            seriesMin.IsVisibleInLegend = true;
            seriesMin.IsValueShownAsLabel = false;
            seriesMin.Color = Color.Red;
            seriesMin.XValueType = ChartValueType.DateTime;
            seriesMin.MarkerStyle = MarkerStyle.None;
            chart1.Series.Add(seriesMin);

            //线条3：上限横线
            Series seriesMax = new Series("Max");
            seriesMax.ChartType = SeriesChartType.Line;
            seriesMax.BorderWidth = 1;
            seriesMax.ShadowOffset = 0;
            seriesMax.IsVisibleInLegend = true;
            seriesMax.IsValueShownAsLabel = false;
            seriesMax.Color = Color.Red;
            seriesMax.XValueType = ChartValueType.DateTime;
            seriesMax.MarkerStyle = MarkerStyle.None;
            chart1.Series.Add(seriesMax);

            //设置X轴的最小值为第一个点的X坐标值
            chartArea.AxisX.Minimum = Convert.ToDateTime(dataTable.Rows[0]["TheTime"]).ToOADate();

            //开始画线
            foreach (DataRow dr in dataTable.Rows)
            {
                series.Points.AddXY(dr["TheTime"], dr["TheValue"]);

                seriesMin.Points.AddXY(dr["TheTime"], 15); //设置下线为15
                seriesMax.Points.AddXY(dr["TheTime"], 30); //设置上限为30
            }
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("t:{0}，v:{1} ", i+1, dp.YValues[0]);
            }
        }


        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button== System.Windows.Forms.MouseButtons.Left)
            {
                Point formPoint = this.PointToClient(Control.MousePosition);
                if (formPoint.X > chart1.Legends[0].Position.X * chart1.Width / 100 &&
                    formPoint.X < (chart1.Legends[0].Position.X + chart1.Legends[0].Position.Width) * chart1.Width / 100 &&
                    formPoint.Y > chart1.Legends[0].Position.Y * chart1.Height / 100 &&
                    formPoint.Y < (chart1.Legends[0].Position.Y + chart1.Legends[0].Position.Height) * chart1.Height / 100)
                {
                    SelectLineForm dlg = new SelectLineForm(dataList);
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ReloadSignal();
                    }
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                    chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
                }
            }
        }

        private void tsb_OpenFile_Click(object sender, EventArgs e)
        {
            //FileDialog dlg = new FileDialog();
            OpenFileDialog odlg = new OpenFileDialog();
            if (odlg.ShowDialog()== DialogResult.OK)
            {
                _filePath = odlg.FileName;
                Signal_Load(null, null);
            }

        }

        private void tsb_Info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该工具由qljun开发，2015-3-12完成V1.0版。\r\n");        
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataList.Count>0)
            { 
                SelectLineForm dlg = new SelectLineForm(dataList);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ReloadSignal();
                }
            }
        }

    }
}
