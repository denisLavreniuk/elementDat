using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Org.BouncyCastle.Crypto.Tls;
using System.Drawing.Text;

using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace elementDB
{
    public partial class Form11 : Form
    {

        public string Filepath = @"D:\Отчёт.txt";
        public string strtowrite = "";
        public string total = "";
        public float rdc1 = 0;
        public float rdc2 = 0;
        public float brt3 = 0;
        public float bzg1 = 0;
        public float kpa = 0;
        public float kpa2 = 0;
        public float bzg2 = 0;
        public float brt = 0;
        public float brt2 = 0;
        public float kpto = 0;
        public float kpto2 = 0;
        public float sid = 0;
        public float sid2 = 0;

        public float nrdc = 0;
        public float nbzg = 0;
        public float nkpa = 0;
        public float nbrt = 0;
        public float nkpto = 0;
        public float nrdc2 = 0;
        public float nbzg2 = 0;
        public float nkpa2 = 0;
        public float nbrt2 = 0;
        public float nkpto2 = 0;
        public float nsid = 0;
        public float nsid2 = 0;

        public int cb = 0;
        public int size = 8;//размер шрифта


        public int id = -1;
        private Form12 resourceForm;

        public string m_title;
        private string m_filter;
        private enum Resources { WARRANTY = 1, BEFORE, BETWEEN, ASSIGNED };

        private struct ResourcesData
        {
            public int periodMonths;
            public int operatingHours;
            public int unitOperatingHours;
            public double difference;
            public Resources type;
        };

        public Form11(string filter)
        {
            WindowState = FormWindowState.Maximized;
            m_filter = filter;
            //BackColor = Color.PowderBlue;
            BackColor = Color.LightSteelBlue;

            InitializeComponent();

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, this.dataGridView1, new object[] { true });

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ReadOnly = false;

            // cb = int.Parse(textBox6.Text);

            collectUnits();
        }

        public void collectUnits()
        {
            dataGridView1.Rows.Clear();

            string sql = string.Format("SELECT unit_info.unit_id, unit_info.unit_num, " +
                "unit_info.product_code, unit_info.release_date, " +
                "contracts.contract_num, contracts.contract_date, " +
                "contracts.associate, contracts.director_decision, " +
                "unit_info.var_stor_period, unit_info.operating_hours," +
                "varranty_res.period_value varranty_period, " +
                "varranty_res.operating_hours varranty_hours, " +
                "bef_first_repair_res.period_value first_repair_period," +
                "bef_first_repair_res.operating_hours first_repair_hours, " +
                "between_repairs_res.period_value between_period, " +
                "between_repairs_res.operating_hours between_hours, " +
                "assigned_res.period_value assigned_period, " +
                "assigned_res.operating_hours assigned_hours, " +
                "refurbished_res.period_value refurbished_period, " +
                "refurbished_res.operating_hours refurbished_hours, " +
                "product_status_changes.change_name " +
                "from unit_info " +
                "left join contracts on unit_info.unit_id = contracts.unit_id " +
                "left join varranty_res on unit_info.unit_id = varranty_res.unit_id " +
                "left join bef_first_repair_res on unit_info.unit_id = bef_first_repair_res.unit_id " +
                "left join between_repairs_res on unit_info.unit_id = between_repairs_res.unit_id " +
                "left join assigned_res on unit_info.unit_id = assigned_res.unit_id " +
                "left join refurbished_res on unit_info.unit_id = refurbished_res.unit_id " +
                "left join product_status_changes on unit_info.unit_id = product_status_changes.unit_id " +
                "WHERE product_code like '%{0}%' " +
                "ORDER BY unit_num;", m_filter);

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

            foreach (DataRow dr in dt.Rows)
            {
                dataGridView1.Rows.Add(dr["unit_id"],
                                       dr["unit_num"],
                                       dr["product_code"],
                                       String.Format("{0:yyyy-MM-dd}",
                                       dr["release_date"]),
                                       dr["contract_num"],
                                       String.Format("{0:yyyy-MM-dd}",
                                       dr["contract_date"]),
                                       dr["associate"],
                                       dr["director_decision"],
                                       dr["var_stor_period"],
                                       dr["varranty_period"],
                                       dr["varranty_hours"],
                                       dr["first_repair_period"],
                                       dr["first_repair_hours"],
                                       dr["between_period"],
                                       dr["between_hours"],
                                       dr["assigned_period"],
                                       dr["assigned_hours"],
                                       dr["refurbished_period"],
                                       dr["refurbished_hours"],
                                       dr["operating_hours"],
                                       dr["change_name"]);
            }
            if (dt.Rows.Count > 0)
            {
                setRowNumber(dataGridView1);
                setCalculatedDate();
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                dataGridView1.ClearSelection();
            }
        }

        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Lavender;
                }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;

                try
                {
                    id = Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (FormatException)
                {
                    return;
                }

                if (resourceForm == null || resourceForm.IsDisposed)
                {
                    m_title = dataGridView1.Rows[e.RowIndex].Cells["unit_num"].
                        Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].
                        Cells["product_code"].Value.ToString();

                    resourceForm = new Form12(id, this);
                    resourceForm.Show();
                }
                else
                {
                    resourceForm.WindowState = FormWindowState.Normal;
                    resourceForm.Activate();
                    resourceForm.BringToFront();
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            /*if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked &&
                !checkBox4.Checked && !checkBox5.Checked)*/
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked &&
                !checkBox4.Checked && !checkBox5.Checked && !checkBox6.Checked)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Не выбран параметр поиска. Обновить список блоков?",
                    "Предупреждение", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    collectUnits();
                }
                return;
            }
            else
            {
                String sql = "SELECT unit_info.unit_id, unit_info.unit_num, " +
                "unit_info.product_code, unit_info.release_date, " +
                "contracts.contract_num, contracts.contract_date, " +
                "contracts.associate, contracts.director_decision, " +
                "unit_info.var_stor_period, unit_info.operating_hours, " +
                "varranty_res.period_value varranty_period, " +
                "varranty_res.operating_hours varranty_hours, " +
                "bef_first_repair_res.period_value first_repair_period," +
                "bef_first_repair_res.operating_hours first_repair_hours, " +
                "between_repairs_res.period_value between_period, " +
                "between_repairs_res.operating_hours between_hours, " +
                "assigned_res.period_value assigned_period, " +
                "assigned_res.operating_hours assigned_hours, " +
                "refurbished_res.period_value refurbished_period, " +
                "refurbished_res.operating_hours refurbished_hours, " +
                "product_status_changes.change_name " +
                "from unit_info " +
                "left join contracts on unit_info.unit_id = contracts.unit_id " +
                "left join varranty_res on unit_info.unit_id = varranty_res.unit_id " +
                "left join bef_first_repair_res on unit_info.unit_id = bef_first_repair_res.unit_id " +
                "left join between_repairs_res on unit_info.unit_id = between_repairs_res.unit_id " +
                "left join assigned_res on unit_info.unit_id = assigned_res.unit_id " +
                "left join refurbished_res on unit_info.unit_id = refurbished_res.unit_id " +
                "left join product_status_changes on unit_info.unit_id = product_status_changes.unit_id " +
                "WHERE ";

                if (checkBox1.Checked)
                {
                    if (textBox3.Text == "") textBox3.Text = 9999.ToString();
                    sql += string.Format("unit_num BETWEEN '{0}' and '{1}' ",
                        textBox1.Text, textBox3.Text);
                }

                if (checkBox2.Checked)
                {
                    if (checkBox1.Checked)
                    {
                        sql += "AND ";
                    }
                    sql += string.Format("release_date BETWEEN '{0}' and '{1}' ",
                        dateTimePicker2.Value.Date.ToString("yyyy-MM-dd"),
                        dateTimePicker3.Value.Date.ToString("yyyy-MM-dd"));
                }

                if (checkBox3.Checked)
                {
                    if (checkBox1.Checked || checkBox2.Checked)
                    {
                        sql += "AND ";
                    }
                    sql += string.Format("unit_info.unit_id IN " +
                                         "(SELECT unit_id FROM contracts " +
                                         "WHERE associate like '%{0}%')",
                                         textBox4.Text);
                }

                if (checkBox4.Checked)
                {
                    if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked)
                    {
                        sql += "AND ";
                    }
                    sql += string.Format("unit_info.unit_id IN " +
                                         "(SELECT unit_id FROM contracts " +
                                         "WHERE contract_num like '%{0}%')",
                                         textBox2.Text);
                }

                if (checkBox5.Checked)
                {
                    if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked ||
                        checkBox4.Checked)
                    {
                        sql += "AND ";
                    }
                    sql += string.Format("unit_info.unit_id IN " +
                                         "(SELECT unit_id FROM contracts " +
                                         "WHERE director_decision like '%{0}%')",
                                         textBox5.Text);
                }

                /* if (checkBox6.Checked)
                 {
                     if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked)
                     {
                         sql += "AND";
                     }
                     sql += string.Format("unit_info.unit_id IN " +
                      "(SELECT unit_id FROM contracts " +
                      "WHERE product_code like '%{0}%')",
                      textBox7.Text);
                 }*/

                if (checkBox6.Checked)
                {
                    if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked)
                    {
                        sql += "AND";
                    }
                    if (metroRadioButton.Checked)
                    {
                        sql += string.Format("product_code='{0}'", textBox7.Text);
                    }
                    else if (metroRadioButton2.Checked)
                    {
                        sql += string.Format("unit_info.unit_id IN " +
                         "(SELECT unit_id FROM unit_info " +
                         "WHERE product_code like '%{0}%')",
                         textBox7.Text);
                    }
                    if (!metroRadioButton.Checked && !metroRadioButton2.Checked) MessageBox.Show("Выберите способ поиска блока (точно/грубо)!");
                }

                DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

                if (dt.Rows.Count == 0)
                {
                    dataGridView1.Rows.Clear();
                    return;
                }

                dataGridView1.Rows.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    dataGridView1.Rows.Add(dr["unit_id"],
                                           dr["unit_num"],
                                           dr["product_code"],
                                           String.Format("{0:yyyy-MM-dd}",
                                           dr["release_date"]),
                                           dr["contract_num"],
                                           String.Format("{0:yyyy-MM-dd}",
                                           dr["contract_date"]),
                                           dr["associate"],
                                           dr["director_decision"],
                                           dr["var_stor_period"],
                                           dr["varranty_period"],
                                           dr["varranty_hours"],
                                           dr["first_repair_period"],
                                           dr["first_repair_hours"],
                                           dr["between_period"],
                                           dr["between_hours"],
                                           dr["assigned_period"],
                                           dr["assigned_hours"],
                                           dr["refurbished_period"],
                                           dr["refurbished_hours"],
                                           dr["operating_hours"],
                                           dr["change_name"]);
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    setRowNumber(dataGridView1);
                    setCalculatedDate();
                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                    dataGridView1.ClearSelection();
                }
            }
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.CellValue1.GetType() == typeof(int) &&
                e.CellValue2.GetType() == typeof(int))
            {
                int value1 = Convert.ToInt32(e.CellValue1);
                int value2 = Convert.ToInt32(e.CellValue2);

                if (value1 > value2)
                {
                    e.SortResult = 1;
                }
                else if (value1 < value2)
                {
                    e.SortResult = -1;
                }
                else if (value1 == value2)
                {
                    e.SortResult = 0;
                }
            }
            else
            {
                e.SortResult = System.String.Compare(e.CellValue1.ToString(),
                    e.CellValue2.ToString());
            }

            if (e.SortResult == 0 && e.Column.Name != "ID")
            {
                e.SortResult = System.String.Compare(
                    dataGridView1.Rows[e.RowIndex1].Cells["unit_num"].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex2].Cells["unit_num"].Value.ToString());
            }
            e.Handled = true;
        }


        private void setCalculatedDate()
        {
            ResourcesData assignedResource;
            ResourcesData beforeResource;
            ResourcesData betweenResource;
            ResourcesData warrantyResource;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int unit_num = (int)row.Cells["unit_num"].Value;

                if (row.Cells["warranty_hours"].Value.ToString() == "")
                {
                    continue;
                }

                warrantyResource.difference = (DateTime.Today - Convert.
                    ToDateTime(row.Cells["release_date"].Value)).TotalDays;
                warrantyResource.unitOperatingHours = (int)row.
                    Cells["operating_hours"].Value;
                warrantyResource.operatingHours = (int)row.
                    Cells["warranty_hours"].Value;
                warrantyResource.periodMonths = (int)row.
                    Cells["warranty_exploit_period"].Value;
                warrantyResource.type = Resources.WARRANTY;

                if (row.Cells["change_name"].Value.ToString() != "для эксплуатации" && row.Cells["change_name"].Value.ToString() == "для эксплуатации")//для отсечения всех, блоков, кроме тех, которые предназначены для эксплуатации исправить на if (row.Cells["change_name"].Value.ToString() != "для эксплуатации")
                {
                    if (isUnderWarranty(warrantyResource, row))
                    {
                        row.Cells["unit_num"].Style.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        row.Cells["unit_num"].Style.BackColor = Color.Yellow;
                    }

                    continue;
                }

                assignedResource.difference = warrantyResource.difference;
                assignedResource.unitOperatingHours = warrantyResource.unitOperatingHours;
                assignedResource.operatingHours = (int)row.Cells["assigned_hours"].Value;
                assignedResource.periodMonths = (int)row.Cells["assigned_period"].Value;
                assignedResource.type = Resources.ASSIGNED;

                beforeResource.difference = assignedResource.difference;
                beforeResource.unitOperatingHours = assignedResource.unitOperatingHours;
                beforeResource.operatingHours = (int)row.Cells["before_first_repair_hours"].Value;
                beforeResource.periodMonths = (int)row.Cells["before_first_repair_period"].Value;
                beforeResource.type = Resources.BEFORE;

                betweenResource.difference = assignedResource.difference;
                betweenResource.unitOperatingHours = assignedResource.unitOperatingHours;
                betweenResource.operatingHours = (int)row.Cells["between_repairs_hours"].Value;
                betweenResource.periodMonths = (int)row.Cells["between_repairs_period"].Value;
                betweenResource.type = Resources.BETWEEN;

                int counter = 1;

                if (isAssignedResourceExpire(assignedResource, row))
                {
                    row.Cells["unit_num"].Style.BackColor = Color.OrangeRed;
                }
                else
                {
                    if (!calculateResource(beforeResource, warrantyResource, counter, row))
                    {
                        if (betweenResource.periodMonths == 0 &&
                            betweenResource.operatingHours == 0 &&
                            assignedResource.periodMonths == 0 &&
                            assignedResource.operatingHours == 0)
                        {
                            continue;
                        }

                        while (!calculateResource(betweenResource, warrantyResource, counter, row))
                        {
                            counter++;
                        }
                    }
                }
            }
        }


        private void TotalBlocksRDC(DataGridViewRow row)
        {
            cb = int.Parse(textBox6.Text);
            if (row.Cells["product_code"].Value.ToString().Contains("РДЦ"))
            {
                nrdc++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    rdc1++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("БРТ"))
            {
                nbrt++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    brt++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("БЗГ"))
            {
                nbzg++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    bzg1++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("КПА"))
            {
                nkpa++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    kpa++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("КПТО"))
            {
                nkpto++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    kpto++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("СИД"))
            {
                nsid++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    sid++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("РДЦ"))
            {
                nrdc2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    rdc2++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("БРТ"))
            {
                nbrt2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {

                    brt2++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("БЗГ"))
            {
                nbzg2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    bzg2++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("КПА"))
            {
                nkpa2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    kpa2++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("КПТО"))
            {
                nkpto2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    kpto2++;
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("СИД"))
            {
                nsid2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    sid2++;
                }
            }
        }
        private bool isUnderWarranty(ResourcesData data, DataGridViewRow row)
        {
            int days = data.periodMonths * 30 + data.periodMonths / 2;
            if (data.difference > days)
            {
                row.Cells["warranty_exploit_period"].Style.BackColor = Color.Yellow;
                return false;
            }

            else if (data.unitOperatingHours > data.operatingHours)
            {
                row.Cells["warranty_hours"].Style.BackColor = Color.Yellow;
                return false;
            }
            else
            {
                isResourceExpireSoon(data, 1, row);
                return true;
            }
        }
        /// ////////////////////////////////////////////////////////////////↓↓↓↓↓↓↓
        private int WorkResourse(DataGridViewRow row)
        {
            int abc = (int)row.Cells["before_first_repair_period"].Value * 30;
            if (abc <= (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays && abc != 0)//пропускаем 0, для того, чтоб не отвлекало при просмотре, так как не может быть такого, что блок только выпустили, он проработал 0 месяцев и уже нужно ремонтировать
            {
                //row.Cells["before_first_repair_period"].Style.BackColor = Color.DarkSlateBlue;
                return 10;
            }
            else if (abc <= (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays + 61 && abc != 0)
            {
                //row.Cells["before_first_repair_period"].Style.BackColor = Color.OrangeRed;
                return 20;
            }
            else
            {
                //row.Cells["before_first_repair_period"].Style.BackColor = Color.LightGreen;
                return 30;
            }
        }
        private int assignedPeriod(DataGridViewRow row)
        {
            int days = (int)row.Cells["assigned_period"].Value * 30;
            if (days <= (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays && days != 0)//пропускаем 0, для того, чтоб не отвлекало при просмотре, так как не может быть такого, что блок только выпустили, он проработал 0 месяцев и уже нужно ремонтировать
            {
                //row.Cells["before_first_repair_period"].Style.BackColor = Color.DarkSlateBlue;
                return 100;
            }
            else if (days <= (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays + 61 && days != 0)
            {
                //row.Cells["before_first_repair_period"].Style.BackColor = Color.OrangeRed;
                return 200;
            }
            else
            {
                //row.Cells["before_first_repair_period"].Style.BackColor = Color.LightGreen;
                return 300;
            }
            //row.Cells["assigned_period"].Style.BackColor = Color.Orange;
        }

        private bool warrantyWork(DataGridViewRow row)//гарантийная наработка
        {
            int workPeriod = (int)row.Cells["operating_hours"].Value;
            if (workPeriod > (int)row.Cells["warranty_hours"].Value) return true;
            else return false;
        }

        private short firstRepairWork(DataGridViewRow row)
        {
            int workPeriod = (int)row.Cells["operating_hours"].Value;
            //MessageBox.Show(workPeriod.ToString());
            if (workPeriod > (int)row.Cells["before_first_repair_hours"].Value) return 1;
            else if ((workPeriod + 50) >= (int)row.Cells["before_first_repair_hours"].Value && workPeriod <= (int)row.Cells["before_first_repair_hours"].Value && workPeriod != 0) return 2;
            else return 0;
        }

        private short betweenRepairPeriod(DataGridViewRow row)
        {
            int workHours = (int)row.Cells["operating_hours"].Value;
            if (workHours > (int)row.Cells["between_repairs_period"].Value) return 1;
            return 0;
        }

        //private short AssignedHours(DataGridViewRow row)
        //{
        //    int workPeriod = (int)row.Cells["operating_hours"].Value;
        //    if (workPeriod > (int)row.Cells["assigned_hours"].Value) return 1;
        //    else if ((workPeriod + 60) >= (int)row.Cells["assigned_hours"].Value && workPeriod <= (int)row.Cells["assigned_hours"].Value && workPeriod != 0) return 2;
        //    else return 0;
        //}
        /// ///////////////////////////////////////////////////////////////////↑↑↑↑↑

        private bool calculateResource(ResourcesData data,
            ResourcesData warranty, int counter, DataGridViewRow row)
        {
            int days = data.periodMonths * 30 + data.periodMonths / 2;

            if ((data.difference > counter * days && data.periodMonths != 0) ||
                    (data.unitOperatingHours > counter * data.operatingHours &&
                data.operatingHours != 0))
            {
                return false;
            }

            isResourceExpireSoon(data, counter, row);

            if (isUnderWarranty(warranty, row))
            {
                row.Cells["unit_num"].Style.BackColor = Color.PaleGreen;
            }
            else
            {
                row.Cells["unit_num"].Style.BackColor = Color.Yellow;
            }

            //////////////////////////////////////////////////////////////////↓↓↓↓↓↓↓
            if (assignedPeriod(row) == 100)
            {
                row.Cells["unit_num"].Style.BackColor = Color.Red;
                row.Cells["assigned_period"].Style.BackColor = Color.Red;
            }
            else if (assignedPeriod(row) == 200)
            {
                row.Cells["unit_num"].Style.BackColor = Color.Orange;
                row.Cells["assigned_period"].Style.BackColor = Color.Orange;
            }
            else if (assignedPeriod(row) == 300)
            {
                //row.Cells["unit_num"].Style.BackColor = Color.Green;
                //row.Cells["assigned_period"].Style.BackColor = Color.Green;
            }

            if (WorkResourse(row) == 10)
            {
                row.Cells["unit_num"].Style.BackColor = Color.Red;
                row.Cells["before_first_repair_period"].Style.BackColor = Color.Red;
            }
            else if (WorkResourse(row) == 20)
            {
                row.Cells["unit_num"].Style.BackColor = Color.Orange;
                row.Cells["before_first_repair_period"].Style.BackColor = Color.Orange;
            }
            //else row.Cells["before_first_repair_period"].Style.BackColor = Color.LightGreen;
            if (warrantyWork(row) == true)
            {
                row.Cells["warranty_hours"].Style.BackColor = Color.Yellow;
            }
            /*else  
            { 
                //row.Cells["warranty_hours"].Style.BackColor = Color.PaleGreen; 
            }*/
            if (firstRepairWork(row) == 1)
            {
                row.Cells["before_first_repair_hours"].Style.BackColor = Color.Red;
                //row.Cells["before_first_repair_hours"].Style.BackColor = Color.Purple;

                row.Cells["unit_num"].Style.BackColor = Color.Red;
            }
            else if (firstRepairWork(row) == 2)
            {
                row.Cells["before_first_repair_hours"].Style.BackColor = Color.Orange;
                //row.Cells["before_first_repair_hours"].Style.BackColor = Color.MediumPurple;

                row.Cells["unit_num"].Style.BackColor = Color.Orange;
            }











            //if (Ass(ignedHours(row) == 1)
            //{
            //    row.Cells["assigned_hours"].Style.BackColor = Color.Red;
            //    //row.Cells["unit_num"].Style.BackColor = Color.Red;
            //}
            //else if (AssignedHours(row) == 2)
            //{
            //    row.Cells["assigned_hours"].Style.BackColor = Color.Orange;
            //    //row.Cells["unit_num"].Style.BackColor = Color.Orange;
            //}





            //else if (AssignedPeriod(row)==0) row.Cells["assigned_period"].Style.BackColor = Color.PaleGoldenrod;
            //TotalBlocksRDC(row);

            //else row.Cells["before_first_repair_hours"].Style.BackColor = Color.PaleGreen
            //////////////////////////////////////////////////////////////////↑↑↑↑
            return true;
        }

        private bool isAssignedResourceExpire(ResourcesData data,
            DataGridViewRow row)
        {
            if (data.operatingHours == 0 && data.periodMonths == 0)
            {
                return false;
            }

            int days = data.periodMonths * 30 + data.periodMonths / 2;

            if ((data.difference > days && data.periodMonths != 0) ||
                (data.unitOperatingHours > data.operatingHours &&
                data.operatingHours != 0))
            {
                row.Cells["unit_num"].Style.BackColor = Color.Red;
                //row.Cells["assigned_period"].Style.BackColor = Color.OrangeRed;
                row.Cells["assigned_hours"].Style.BackColor = Color.Red;

                return true;
            }
            isResourceExpireSoon(data, 1, row);
            return false;
        }

        private bool isResourceExpireSoon(ResourcesData data, int counter, DataGridViewRow row)
        {
            int days = data.periodMonths * 30 + data.periodMonths / 2;
            if (data.difference > (counter * days) - 61 &&
                data.difference < counter * days)
            {
                switch (data.type)
                {
                    case Resources.WARRANTY:
                        //row.Cells["warranty_exploit_period"].Style.BackColor = Color.Orange;
                        break;
                    case Resources.BEFORE:
                        row.Cells["before_first_repair_period"].Style.BackColor = Color.Orange;
                        break;
                    case Resources.BETWEEN:
                        row.Cells["between_repairs_period"].Style.BackColor = Color.OrangeRed;//межремонтный срок
                        break;
                    case Resources.ASSIGNED:
                        row.Cells["assigned_period"].Style.BackColor = Color.Orange;
                        break;
                }
                return true;
            }
            return false;
        }


        private void button1_Click(Object sender, EventArgs e)//создание отчета
        {
            SaveFileDialog savetxt = new SaveFileDialog();
            savetxt.Filter = "TXT Files |*.txt";
            if (savetxt.ShowDialog() == DialogResult.OK)
            {
                rdc1 = rdc2 = brt3 = bzg1 = kpa = kpa2 = bzg2 = brt = brt2 = kpto = kpto2 = sid = sid2 = nrdc = nbzg = nkpa = nbrt = nkpto = nrdc2 = nbzg2 = nkpa2 = nbrt2 = nkpto2 = nsid = nsid2 = 0;

                ResourcesData assignedResource;
                ResourcesData beforeResource;
                ResourcesData betweenResource;
                ResourcesData warrantyResource;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int unit_num = (int)row.Cells["unit_num"].Value;

                    if (row.Cells["warranty_hours"].Value.ToString() == "")
                    {
                        continue;
                    }

                    warrantyResource.difference = (DateTime.Today - Convert.
                        ToDateTime(row.Cells["release_date"].Value)).TotalDays;
                    warrantyResource.unitOperatingHours = (int)row.
                        Cells["operating_hours"].Value;
                    warrantyResource.operatingHours = (int)row.
                        Cells["warranty_hours"].Value;
                    warrantyResource.periodMonths = (int)row.
                        Cells["warranty_exploit_period"].Value;
                    warrantyResource.type = Resources.WARRANTY;

                    if (row.Cells["change_name"].Value.ToString() != "для эксплуатации" && row.Cells["change_name"].Value.ToString() == "для эксплуатации")//для отсечения всех, блоков, кроме тех, который предназначены для эксплуатации исправить на if (row.Cells["change_name"].Value.ToString() != "для эксплуатации")
                    {
                        if (isUnderWarranty(warrantyResource, row))
                        {
                            row.Cells["unit_num"].Style.BackColor = Color.PaleGreen;
                        }
                        else
                        {
                            row.Cells["unit_num"].Style.BackColor = Color.Yellow;
                        }

                        continue;
                    }

                    assignedResource.difference = warrantyResource.difference;
                    assignedResource.unitOperatingHours = warrantyResource.unitOperatingHours;
                    assignedResource.operatingHours = (int)row.Cells["assigned_hours"].Value;
                    assignedResource.periodMonths = (int)row.Cells["assigned_period"].Value;
                    assignedResource.type = Resources.ASSIGNED;

                    beforeResource.difference = assignedResource.difference;
                    beforeResource.unitOperatingHours = assignedResource.unitOperatingHours;
                    beforeResource.operatingHours = (int)row.Cells["before_first_repair_hours"].Value;
                    beforeResource.periodMonths = (int)row.Cells["before_first_repair_period"].Value;
                    beforeResource.type = Resources.BEFORE;

                    betweenResource.difference = assignedResource.difference;
                    betweenResource.unitOperatingHours = assignedResource.unitOperatingHours;
                    betweenResource.operatingHours = (int)row.Cells["between_repairs_hours"].Value;
                    betweenResource.periodMonths = (int)row.Cells["between_repairs_period"].Value;
                    betweenResource.type = Resources.BETWEEN;

                    int counter = 1;

                    if (isAssignedResourceExpire(assignedResource, row))
                    {
                        row.Cells["unit_num"].Style.BackColor = Color.OrangeRed;
                    }
                    else
                    {
                        if (!calculateResource(beforeResource, warrantyResource, counter, row))
                        {
                            if (betweenResource.periodMonths == 0 &&
                                betweenResource.operatingHours == 0 &&
                                assignedResource.periodMonths == 0 &&
                                assignedResource.operatingHours == 0)
                            {
                                continue;
                            }

                            while (!calculateResource(betweenResource, warrantyResource, counter, row))
                            {
                                counter++;
                            }
                        }
                    }
                    TotalBlocksRDC(row);
                }

                cb = int.Parse(textBox6.Text);
                float rdcProc = rdc1 / nrdc * 100;
                float brtProc = brt / nbrt * 100;
                float bzgProc = bzg1 / nbzg * 100;
                float kpaProc = kpa / nkpa * 100;
                float kptoProc = kpto / nkpto * 100;
                float sidProc = sid / nsid * 100;

                float rdcProc2 = rdc2 / nrdc * 100;
                float brtProc2 = brt2 / nbrt * 100;
                float bzgProc2 = bzg2 / nbzg * 100;
                float kpaProc2 = kpa2 / nkpa * 100;
                float kptoProc2 = kpto2 / nkpto * 100;
                float sidProc2 = sid2 / nsid * 100;
                total = "Отчет о гарантийном ресурсе блоков, выпускаемых АО Элемент по состоянию на " + DateTime.Today.ToShortDateString() + '\n' +
               "РДЦ-450  всего: " + nrdc + "   из них на гарантии сейчас: " + rdc1 + "(" + Math.Round(rdcProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + rdc2 + "(" + Math.Round(rdcProc2, 2) + " %)" + '\n' +
               "БРТ всего:  " + nbrt + " из них на гарантии сейчас: " + brt + "(" + Math.Round(brtProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + brt2 + "(" + Math.Round(brtProc2, 2) + " %)" + '\n' +
               "БЗГ всего:  " + nbzg + " из них на гарантии сейчас: " + bzg1 + "(" + Math.Round(bzgProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + bzg2 + "(" + Math.Round(bzgProc2, 2) + " %)" + '\n' +
               "КПА всего:  " + nkpa + " из них на гарантии сейчас:" + kpa + "(" + Math.Round(kpaProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + kpa2 + "(" + Math.Round(kpaProc2, 2) + " %)" + '\n' +
               "КПТО всего: " + nkpto + " из них на гарантии сейчас: " + kpto + "(" + Math.Round(kptoProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + kpto2 + "(" + Math.Round(kptoProc2, 2) + " %)" + '\n' +
               "СИД3 всего: " + nsid + " из них на гарантии сейчас: " + sid + "(" + Math.Round(sidProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + sid2 + "(" + Math.Round(sidProc2, 2) + " %)" + '\n';
                strtowrite = total;

                Excel.Application ObjExcel = new Excel.Application();
                Excel.Workbook objWorkBook;
                Excel.Worksheet objWorkSheet;
                objWorkBook = ObjExcel.Workbooks.Add(@"D:\отчет.xlsx");
                objWorkSheet = (Excel.Worksheet)objWorkBook.Sheets[1];


                Excel.Range _excelCells1 = objWorkSheet.get_Range("A1", "A3").Cells;
                _excelCells1.Merge(Type.Missing);
                objWorkSheet.Cells[1, 1] = "№";

                Excel.Range _excelCells2 = objWorkSheet.get_Range("B1", "B3").Cells;
                _excelCells2.Merge(Type.Missing);
                objWorkSheet.Cells[1, 2] = "Тип изделия";

                Excel.Range _excelCells3 = objWorkSheet.get_Range("C1", "C3").Cells;
                _excelCells3.Merge(Type.Missing);
                objWorkSheet.Cells[1, 3] = "Общее количество изделий";

                Excel.Range _excelCells4 = objWorkSheet.get_Range("D1", "D3").Cells;
                _excelCells4.Merge(Type.Missing);
                objWorkSheet.Cells[1, 4] = "Установленный гарантийный ресурс, год";

                Excel.Range _excelCells5 = objWorkSheet.get_Range("E1", "H1").Cells;
                _excelCells5.Merge(Type.Missing);
                objWorkSheet.Cells[1, 5] = "Количество гарантийных изделий";

                Excel.Range _excelCells6 = objWorkSheet.get_Range("I1", "I1").Cells;
                _excelCells6.Merge(Type.Missing);
                objWorkSheet.Cells[1, 6] = "Примечания";

                Excel.Range _excelCells7 = objWorkSheet.get_Range("E2", "F2").Cells;
                _excelCells7.Merge(Type.Missing);
                objWorkSheet.Cells[2, 5] = "На " + DateTime.Today.ToShortDateString();

                Excel.Range _excelCells8 = objWorkSheet.get_Range("G2", "H2").Cells;
                _excelCells8.Merge(Type.Missing);
                objWorkSheet.Cells[2, 7] = "Через " + cb + " мес.";

                objWorkSheet.Cells[3, 5] = "Ед.";
                objWorkSheet.Cells[3, 6] = "%";
                objWorkSheet.Cells[3, 7] = "Ед.";
                objWorkSheet.Cells[3, 8] = "%";

                int i = 0;
                objWorkSheet.Cells[4, 1] = ++i;
                objWorkSheet.Cells[4, 2] = "РДЦ-450";
                objWorkSheet.Cells[4, 3] = nrdc;
                objWorkSheet.Cells[4, 4] = "1-3";
                objWorkSheet.Cells[4, 5] = rdc1;
                objWorkSheet.Cells[4, 6] = Math.Round(rdcProc, 2);
                objWorkSheet.Cells[4, 7] = rdc2;
                objWorkSheet.Cells[4, 8] = Math.Round(rdcProc2, 2);

                objWorkSheet.Cells[5, 1] = ++i;
                objWorkSheet.Cells[5, 2] = "БРТ";
                objWorkSheet.Cells[5, 3] = nbrt;
                objWorkSheet.Cells[5, 4] = "2";
                objWorkSheet.Cells[5, 5] = brt;
                objWorkSheet.Cells[5, 6] = Math.Round(brtProc, 2);
                objWorkSheet.Cells[5, 7] = brt2;
                objWorkSheet.Cells[5, 8] = Math.Round(brtProc2, 2);

                objWorkSheet.Cells[6, 1] = ++i;
                objWorkSheet.Cells[6, 2] = "БЗГ-450";
                objWorkSheet.Cells[6, 3] = nbzg;
                objWorkSheet.Cells[6, 4] = "1-2";
                objWorkSheet.Cells[6, 5] = bzg1;
                objWorkSheet.Cells[6, 6] = Math.Round(bzgProc, 2);
                objWorkSheet.Cells[6, 7] = bzg2;
                objWorkSheet.Cells[6, 8] = Math.Round(bzgProc2, 2);

                objWorkSheet.Cells[7, 1] = ++i;
                objWorkSheet.Cells[7, 2] = "КПА-450";
                objWorkSheet.Cells[7, 3] = nkpa;
                objWorkSheet.Cells[7, 4] = "2";
                objWorkSheet.Cells[7, 5] = kpa;
                objWorkSheet.Cells[7, 6] = Math.Round(kpaProc, 2);
                objWorkSheet.Cells[7, 7] = kpa2;
                objWorkSheet.Cells[7, 8] = Math.Round(kpaProc2, 2);

                objWorkSheet.Cells[8, 1] = ++i;
                objWorkSheet.Cells[8, 2] = "КПТО";
                objWorkSheet.Cells[8, 3] = nkpto;
                objWorkSheet.Cells[8, 4] = "1-1,5";
                objWorkSheet.Cells[8, 5] = kpto;
                objWorkSheet.Cells[8, 6] = Math.Round(kptoProc, 2);
                objWorkSheet.Cells[8, 7] = kpto2;
                objWorkSheet.Cells[8, 8] = Math.Round(kptoProc2, 2);

                objWorkSheet.Cells[9, 1] = ++i;
                objWorkSheet.Cells[9, 2] = "СИД-3-148";
                objWorkSheet.Cells[9, 3] = nsid;
                objWorkSheet.Cells[9, 4] = "2";
                objWorkSheet.Cells[9, 5] = sid;
                objWorkSheet.Cells[9, 6] = Math.Round(sidProc, 2);
                objWorkSheet.Cells[9, 7] = sid2;
                objWorkSheet.Cells[9, 8] = Math.Round(sidProc2, 2);


                ObjExcel.Visible = true;
                ObjExcel.UserControl = true;
                //File.WriteAllLines(sfd.FileName, output, System.Text.Encoding.UTF8);
                File.WriteAllText(savetxt.FileName, strtowrite, Encoding.UTF8);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            button1.Text = "Отчет для " + textBox6.Text.ToString() + " месяцев";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "") checkBox1.Checked = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "") checkBox1.Checked = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Text != "") checkBox2.Checked = true;
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker3.Text != "") checkBox2.Checked = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "") checkBox3.Checked = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "") checkBox4.Checked = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != "") checkBox5.Checked = true;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "") checkBox6.Checked = true;
        }

        private void metroRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton.Checked) checkBox6.Checked = true;
        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton2.Checked) checkBox6.Checked = true;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            size++;
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", size);
        }


        private void metroButton2_Click_1(object sender, EventArgs e)
        {
            if (size > 1) size--;
            else size = 1;
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", size);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}