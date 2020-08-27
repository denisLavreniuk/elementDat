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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.unit_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.release_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contract_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.concluded_with = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dir_decision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warranty_stored_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warranty_exploit_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.warranty_hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.before_first_repair_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.before_first_repair_hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.between_repairs_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.between_repairs_hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assigned_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assigned_hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refurbished_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refurbished_hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operating_hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.change_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.unit_id,
            this.unit_num,
            this.product_code,
            this.release_date,
            this.contract_number,
            this.date,
            this.concluded_with,
            this.dir_decision,
            this.warranty_stored_period,
            this.warranty_exploit_period,
            this.warranty_hours,
            this.before_first_repair_period,
            this.before_first_repair_hours,
            this.between_repairs_period,
            this.between_repairs_hours,
            this.assigned_period,
            this.assigned_hours,
            this.refurbished_period,
            this.refurbished_hours,
            this.operating_hours,
            this.change_name});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(2, 2);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(842, 378);
            this.dataGridView1.TabIndex = 4;
            // 
            // unit_id
            // 
            this.unit_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.unit_id.FillWeight = 5F;
            this.unit_id.HeaderText = "#";
            this.unit_id.MinimumWidth = 6;
            this.unit_id.Name = "unit_id";
            this.unit_id.ReadOnly = true;
            this.unit_id.Visible = false;
            this.unit_id.Width = 55;
            // 
            // unit_num
            // 
            this.unit_num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.unit_num.FillWeight = 45F;
            this.unit_num.HeaderText = "Номер";
            this.unit_num.MinimumWidth = 45;
            this.unit_num.Name = "unit_num";
            this.unit_num.ReadOnly = true;
            this.unit_num.Width = 45;
            // 
            // product_code
            // 
            this.product_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.product_code.FillWeight = 120F;
            this.product_code.HeaderText = "Шифр, исполнение";
            this.product_code.MinimumWidth = 150;
            this.product_code.Name = "product_code";
            this.product_code.ReadOnly = true;
            this.product_code.Width = 150;
            // 
            // release_date
            // 
            this.release_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.release_date.HeaderText = "Выпуск";
            this.release_date.MinimumWidth = 70;
            this.release_date.Name = "release_date";
            this.release_date.ReadOnly = true;
            this.release_date.Width = 70;
            // 
            // contract_number
            // 
            this.contract_number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.contract_number.HeaderText = "№ Договора";
            this.contract_number.MinimumWidth = 100;
            this.contract_number.Name = "contract_number";
            this.contract_number.ReadOnly = true;
            // 
            // date
            // 
            this.date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.date.HeaderText = "Дата";
            this.date.MinimumWidth = 70;
            this.date.Name = "date";
            this.date.ReadOnly = true;
            this.date.Width = 70;
            // 
            // concluded_with
            // 
            this.concluded_with.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.concluded_with.HeaderText = "С кем заключен";
            this.concluded_with.MinimumWidth = 40;
            this.concluded_with.Name = "concluded_with";
            this.concluded_with.ReadOnly = true;
            // 
            // dir_decision
            // 
            this.dir_decision.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dir_decision.HeaderText = "Распоряжение директора";
            this.dir_decision.MinimumWidth = 50;
            this.dir_decision.Name = "dir_decision";
            this.dir_decision.ReadOnly = true;
            // 
            // warranty_stored_period
            // 
            this.warranty_stored_period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.warranty_stored_period.HeaderText = "Гарантийный срок хранения";
            this.warranty_stored_period.MinimumWidth = 50;
            this.warranty_stored_period.Name = "warranty_stored_period";
            this.warranty_stored_period.ReadOnly = true;
            this.warranty_stored_period.Width = 125;
            // 
            // warranty_exploit_period
            // 
            this.warranty_exploit_period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.warranty_exploit_period.HeaderText = "Гарантийный срок эксплуатации";
            this.warranty_exploit_period.MinimumWidth = 6;
            this.warranty_exploit_period.Name = "warranty_exploit_period";
            this.warranty_exploit_period.ReadOnly = true;
            this.warranty_exploit_period.Width = 125;
            // 
            // warranty_hours
            // 
            this.warranty_hours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.warranty_hours.HeaderText = "Гарантийная наработка";
            this.warranty_hours.MinimumWidth = 6;
            this.warranty_hours.Name = "warranty_hours";
            this.warranty_hours.ReadOnly = true;
            this.warranty_hours.Width = 125;
            // 
            // before_first_repair_period
            // 
            this.before_first_repair_period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.before_first_repair_period.HeaderText = "Срок до 1-го ремонта";
            this.before_first_repair_period.MinimumWidth = 6;
            this.before_first_repair_period.Name = "before_first_repair_period";
            this.before_first_repair_period.ReadOnly = true;
            this.before_first_repair_period.Width = 90;
            // 
            // before_first_repair_hours
            // 
            this.before_first_repair_hours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.before_first_repair_hours.HeaderText = "Наработка до 1-го ремонта ";
            this.before_first_repair_hours.MinimumWidth = 6;
            this.before_first_repair_hours.Name = "before_first_repair_hours";
            this.before_first_repair_hours.ReadOnly = true;
            this.before_first_repair_hours.Width = 125;
            // 
            // between_repairs_period
            // 
            this.between_repairs_period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.between_repairs_period.HeaderText = "Межремонтный срок";
            this.between_repairs_period.MinimumWidth = 6;
            this.between_repairs_period.Name = "between_repairs_period";
            this.between_repairs_period.ReadOnly = true;
            this.between_repairs_period.Width = 125;
            // 
            // between_repairs_hours
            // 
            this.between_repairs_hours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.between_repairs_hours.HeaderText = "Межремонтная наработка";
            this.between_repairs_hours.MinimumWidth = 6;
            this.between_repairs_hours.Name = "between_repairs_hours";
            this.between_repairs_hours.ReadOnly = true;
            this.between_repairs_hours.Width = 125;
            // 
            // assigned_period
            // 
            this.assigned_period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.assigned_period.HeaderText = "Назначенный срок";
            this.assigned_period.MinimumWidth = 6;
            this.assigned_period.Name = "assigned_period";
            this.assigned_period.ReadOnly = true;
            this.assigned_period.Width = 125;
            // 
            // assigned_hours
            // 
            this.assigned_hours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.assigned_hours.HeaderText = "Назначенная наработка";
            this.assigned_hours.MinimumWidth = 6;
            this.assigned_hours.Name = "assigned_hours";
            this.assigned_hours.ReadOnly = true;
            this.assigned_hours.Width = 125;
            // 
            // refurbished_period
            // 
            this.refurbished_period.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.refurbished_period.HeaderText = "Срок по тех. состоянию";
            this.refurbished_period.MinimumWidth = 6;
            this.refurbished_period.Name = "refurbished_period";
            this.refurbished_period.ReadOnly = true;
            this.refurbished_period.Width = 125;
            // 
            // refurbished_hours
            // 
            this.refurbished_hours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.refurbished_hours.HeaderText = "Наработка по тех. состоянию";
            this.refurbished_hours.MinimumWidth = 6;
            this.refurbished_hours.Name = "refurbished_hours";
            this.refurbished_hours.ReadOnly = true;
            this.refurbished_hours.Width = 125;
            // 
            // operating_hours
            // 
            this.operating_hours.HeaderText = "operating_hours";
            this.operating_hours.MinimumWidth = 6;
            this.operating_hours.Name = "operating_hours";
            this.operating_hours.ReadOnly = true;
            this.operating_hours.Visible = false;
            this.operating_hours.Width = 125;
            // 
            // change_name
            // 
            this.change_name.HeaderText = "change_name";
            this.change_name.MinimumWidth = 6;
            this.change_name.Name = "change_name";
            this.change_name.ReadOnly = true;
            this.change_name.Visible = false;
            this.change_name.Width = 125;
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
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn release_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn contract_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn concluded_with;
        private System.Windows.Forms.DataGridViewTextBoxColumn dir_decision;
        private System.Windows.Forms.DataGridViewTextBoxColumn warranty_stored_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn warranty_exploit_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn warranty_hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn before_first_repair_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn before_first_repair_hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn between_repairs_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn between_repairs_hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn assigned_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn assigned_hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn refurbished_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn refurbished_hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn operating_hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn change_name;
    }
}