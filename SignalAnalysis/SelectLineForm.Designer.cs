namespace SignalAnalysis
{
    partial class SelectLineForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectLineForm));
            this.dgv_SelectLine = new System.Windows.Forms.DataGridView();
            this.Col1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SelectLine)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_SelectLine
            // 
            this.dgv_SelectLine.AllowUserToAddRows = false;
            this.dgv_SelectLine.AllowUserToDeleteRows = false;
            this.dgv_SelectLine.AllowUserToResizeColumns = false;
            this.dgv_SelectLine.AllowUserToResizeRows = false;
            this.dgv_SelectLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SelectLine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2,
            this.Col3});
            this.dgv_SelectLine.Location = new System.Drawing.Point(0, 0);
            this.dgv_SelectLine.Name = "dgv_SelectLine";
            this.dgv_SelectLine.RowHeadersVisible = false;
            this.dgv_SelectLine.RowTemplate.Height = 23;
            this.dgv_SelectLine.Size = new System.Drawing.Size(341, 91);
            this.dgv_SelectLine.TabIndex = 0;
            // 
            // Col1
            // 
            this.Col1.Frozen = true;
            this.Col1.HeaderText = "是否显示";
            this.Col1.Name = "Col1";
            // 
            // Col2
            // 
            this.Col2.Frozen = true;
            this.Col2.HeaderText = "序号";
            this.Col2.Name = "Col2";
            this.Col2.ReadOnly = true;
            this.Col2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col3
            // 
            this.Col3.Frozen = true;
            this.Col3.HeaderText = "线型";
            this.Col3.Name = "Col3";
            this.Col3.ReadOnly = true;
            this.Col3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Col3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col3.Width = 150;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(133, 108);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 1;
            this.OK.Text = "确定";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // SelectLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 141);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.dgv_SelectLine);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectLineForm";
            this.Text = "选择显示数据";
            this.Load += new System.EventHandler(this.SelectLineForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SelectLine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_SelectLine;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col3;
    }
}