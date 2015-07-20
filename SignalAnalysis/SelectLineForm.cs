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
    public partial class SelectLineForm : Form
    {
        public SelectLineForm()
        {
            InitializeComponent();
        }
        public List<LineData> _dataList = new List<LineData>();
        public SelectLineForm(List<LineData> list)
        {
            _dataList = list;
            InitializeComponent();
        }

        private void SelectLineForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < _dataList.Count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewCheckBoxCell());
                row.Cells[0].Value = _dataList[i].IsSelecte;
                row.Cells.Add(new DataGridViewTextBoxCell());//(_dataList[i].Serier));
                row.Cells[1].Value = _dataList[i].Serier;
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[2].Value = _dataList[i].clr.ToString();

                this.dgv_SelectLine.Rows.Add(row);
            }
        }


        private void Confirm_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            for (int i = 0; i < _dataList.Count; i++)
            {
                row = this.dgv_SelectLine.Rows[i];
                _dataList[i].IsSelecte = Convert.ToBoolean(row.Cells[0].Value);
            }
            Signal.dataList = _dataList;
            //this.OK_Click(null, null);
            DialogResult = System.Windows.Forms.DialogResult.OK;

        }

    }
    public class LineData
    {
        public bool IsSelecte;
        public string Serier;
        public Color clr;
    }
}
