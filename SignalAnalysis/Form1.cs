using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalAnalysis
{
    public partial class SignalAnalysis : Form
    {
        public SignalAnalysis()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //this.reportViewer1.RefreshReport();
            //this.reportViewer1.RefreshReport();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        public static List<string> str = new List<string>{"类别1","类别2"};
        public static int count = 0;
        private void dataGridView1_DefaultCellStyleChanged(object sender, EventArgs e)
        {
            
        }
        private System.Windows.Forms.ComboBox comboBox14;
        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            count++;
            e.Row.Cells["dgv_Col1"].Value = count;
            this.comboBox14 = new System.Windows.Forms.ComboBox();
            //comboBox14.
            this.dataGridView1.Controls.Add(this.comboBox14);
        }

        private void cbb_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
           

            switch (cbb_Category.SelectedIndex)
            {
                case 0:////电缸运动
                    this.cbb_Function.Items.AddRange(new string[2] { "MoveAxis","GetAxisPos" });
                    this.cbb_Function.SelectedIndex = 0;
                    break;
                case 1://气缸运动
                    this.cbb_Function.Items.AddRange(new string[2] { "MoveAxis","GetAxisPos" });
                    this.cbb_Function.SelectedIndex = 0;
                    break;
                case 2://设备状态查询及改变
                    this.cbb_Function.Items.AddRange(new string[2] { "MoveAxis","GetAxisPos" });
                    break;
                case 3://手机端操作
                    this.cbb_Function.Items.AddRange(new string[2] { "MoveAxis","GetAxisPos" });
                    break;
                case 4://相机操作
                    this.cbb_Function.Items.AddRange(new string[2] { "MoveAxis","GetAxisPos" });
                    break;
                case 5://算法操作
                    break;
                case 6://流程操作 

                    break;
            }
        }

        private void btn_AddRow_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Add("1", this.cbb_Category.SelectedItem.ToString(),
                this.cbb_Function.SelectedItem.ToString(), "Axis1, 50", true);
        }
    }
}
