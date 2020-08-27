namespace elementDB
{
    partial class failures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(failures));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.unit_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.release_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.all_failures = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kpn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.use = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.not_confirmed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.unit_num,
            this.product_code,
            this.release_date,
            this.all_failures,
            this.kpn,
            this.use,
            this.not_confirmed});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(840, 376);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(846, 426);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // unit_num
            // 
            this.unit_num.HeaderText = "Номер";
            this.unit_num.Name = "unit_num";
            this.unit_num.ReadOnly = true;
            // 
            // product_code
            // 
            this.product_code.HeaderText = "Шифр, исполнение";
            this.product_code.Name = "product_code";
            this.product_code.ReadOnly = true;
            // 
            // release_date
            // 
            this.release_date.HeaderText = "Выпуск";
            this.release_date.Name = "release_date";
            this.release_date.ReadOnly = true;
            // 
            // all_failures
            // 
            this.all_failures.HeaderText = "Всего отказов";
            this.all_failures.Name = "all_failures";
            this.all_failures.ReadOnly = true;
            // 
            // kpn
            // 
            this.kpn.HeaderText = "КПН";
            this.kpn.Name = "kpn";
            this.kpn.ReadOnly = true;
            // 
            // use
            // 
            this.use.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.use.HeaderText = "Эксплуатационный";
            this.use.Name = "use";
            this.use.ReadOnly = true;
            this.use.Width = 129;
            // 
            // not_confirmed
            // 
            this.not_confirmed.HeaderText = "Не подтвердился";
            this.not_confirmed.Name = "not_confirmed";
            this.not_confirmed.ReadOnly = true;
            // 
            // failures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "failures";
            this.Text = "Список отказов";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn release_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn all_failures;
        private System.Windows.Forms.DataGridViewTextBoxColumn kpn;
        private System.Windows.Forms.DataGridViewTextBoxColumn use;
        private System.Windows.Forms.DataGridViewTextBoxColumn not_confirmed;
    }
}