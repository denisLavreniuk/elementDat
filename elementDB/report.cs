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
//using Org.BouncyCastle.Security;
//using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace elementDB
{
    public partial class report : Form
    {
        report rep;

        //public string Filepath = @"D:\Отчёт.txt";
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


        public int id = -1;
        private Form12 resourceForm;
        //Form18 frm18;
        /// ////////////////////////////////////////////////
        //private report reportForm;
        ////////////////////////////////////////////////////

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

        public report(string filter)
        {
            WindowState = FormWindowState.Maximized;
            m_filter = filter;
            BackColor = Color.PowderBlue;
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

        /*private void Button2_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked &&
                !checkBox4.Checked && !checkBox5.Checked)
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
        }*/

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



                //MessageBox.Show(row.Cells["warranty_hours"].Value.ToString());






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



                //MessageBox.Show(betweenResource.operatingHours.ToString());



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
            //int cb = 0;
            //cb = int.Parse(textBox6.Text);
            //cb = int.Parse(textBox6.Text);
            //int cb=int.Parse(textBox6.Text);
            if (row.Cells["product_code"].Value.ToString().Contains("РДЦ"))
            {
                nrdc++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {
                    rdc1++;
                    //total += row.Cells["unit_num"].Value.ToString() + "   " + row.Cells["product_code"].Value.ToString() + '\n';
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("БРТ"))
            {
                nbrt++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                //int b= (int)(DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays;
                if (abc > (DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {

                    brt++;

                    //total += row.Cells["unit_num"].Value.ToString() + "   " + row.Cells["product_code"].Value.ToString() +  '\n';

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
                    //total += row.Cells["unit_num"].Value.ToString() + "   " + row.Cells["product_code"].Value.ToString() + '\n';
                }
            }

            if (row.Cells["product_code"].Value.ToString().Contains("БРТ"))
            {
                nbrt2++;
                int abc = (int)row.Cells["warranty_exploit_period"].Value * 30;
                //int b= (int)(DateTime.Today - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays;
                if (abc > (DateTime.Today.AddMonths(cb) - Convert.ToDateTime(row.Cells["release_date"].Value)).TotalDays)
                {

                    brt2++;

                    //total += row.Cells["unit_num"].Value.ToString() + "   " + row.Cells["product_code"].Value.ToString() +  '\n';

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

            //    total = "РДЦ-450  всего: " + nrdc + "   из них на гарантии сейчас: " + rdc1 + "(" + (rdc1 / nrdc) + ")" + "  будет через  " + cb + " месяцев:  " + rdc2 + '\n' +
            //            "БРТ всего:  " + nbrt + " из них на гарантии сейчас: " + brt + "  будет через  " + cb + " месяцев:  " + brt2 + '\n' +
            //            "БЗГ всего:  " + nbzg + " из них на гарантии сейчас: " + bzg1 + "  будет через  " + cb + " месяцев:  " + bzg2 + '\n' +
            //            "КПА всего:  " + nkpa + " из них на гарантии сейчас:" + kpa + "  будет через  " + cb + " месяцев:  " + kpa2 + '\n' +
            //            "КПТО всего: " + nkpto + " из них на гарантиии сейчас: " + kpto + "  будет через  " + cb + " месяцев:  " + kpto2 + '\n';

            //strtowrite = total;
        }
        private bool isUnderWarranty(ResourcesData data, DataGridViewRow row)
        {
            int days = data.periodMonths * 30 + data.periodMonths / 2;
            /*//отчет (блоки не на гарантии)
            if (row.Cells["product_code"].Value.ToString().Contains("РДЦ-450М")) nrdc++;
            else if (row.Cells["product_code"].Value.ToString().Contains("БРТ")) nbrt++;
            else if (row.Cells["product_code"].Value.ToString().Contains("БЗГ")) nbzg++;
            else if (row.Cells["product_code"].Value.ToString().Contains("КПА-450-КПА-450М")) nkpa++;
            else if (row.Cells["product_code"].Value.ToString().Contains("КПТО-КПТО-450С-500")) nkpto++;
            //отчет*/
            //отчет↓↓↓↓↓↓↓
            if (data.difference > days)
            {
                row.Cells["warranty_exploit_period"].Style.BackColor = Color.Yellow;
                /* //отчет (блоки не на гарантии)
                 if (row.Cells["product_code"].Value.ToString().Contains("РДЦ-450М")) nrdc++;
                 else if (row.Cells["product_code"].Value.ToString().Contains("БРТ")) nbrt++;
                 else if (row.Cells["product_code"].Value.ToString().Contains("БЗГ")) nbzg++;
                 else if (row.Cells["product_code"].Value.ToString().Contains("КПА-450-КПА-450М")) nkpa++;
                 else if (row.Cells["product_code"].Value.ToString().Contains("КПТО-КПТО-450С-500")) nkpto++;
                 //отчет*/
                return false;
            }

            else if (data.unitOperatingHours > data.operatingHours)
            {
                row.Cells["warranty_hours"].Style.BackColor = Color.Yellow;
                return false;
            }
            /*//отчет (блоки на гарантии)↓↓↓↓↓↓↓
            if (data.difference<days)
            {

                //if (row.Cells["product_code"].Value.ToString() == "РДЦ-450М-нет") rdc1++;
                //else if (row.Cells["product_code"].Value.ToString() == "РДЦ-450М-В-430-Р") rdc2++;
               // else if (row.Cells["product_code"].Value.ToString() == "БРТ-02") brt++;
               // else if (row.Cells["product_code"].Value.ToString() == "БЗГ-450") bzg1++;
               // else if (row.Cells["product_code"].Value.ToString() == "РДЦ-450М-С-400МВРР-Р") rdc3++;
               // else if (row.Cells["product_code"].Value.ToString() == "КПА-450-КПА-450М") kpa++;
               // else if (row.Cells["product_code"].Value.ToString() == "БЗГ-450-2") bzg2++;
              //  else if (row.Cells["product_code"].Value.ToString() == "КПТО-КПТО-450С-500") kpto++;

                if (row.Cells["product_code"].Value.ToString().Contains("РДЦ-450М")) rdc1++;
                else if (row.Cells["product_code"].Value.ToString().Contains("БРТ")) brt++;
                else if (row.Cells["product_code"].Value.ToString().Contains("БЗГ")) bzg1++;
                else if (row.Cells["product_code"].Value.ToString().Contains("КПА-450-КПА-450М")) kpa++;
                else if (row.Cells["product_code"].Value.ToString().Contains("КПТО-КПТО-450С-500")) kpto++;

                //total = rdc1 + " " + rdc2 + " " + rdc3 + " " + bzg1 + " " + bzg2 + " " + brt + " " + kpto;
                //int rdc = rdc1 + nrdc;
                //float r = rdc1 / rdc;
                total = "РДЦ-450  всего: " + (rdc1 + nrdc) + "   из них на гарантии: " + rdc1 + '\n' +
                    "БРТ всего:  " + (brt + nbrt) + " из них на гарантии: " + brt + '\n' +
                    "БЗГ всего:  " + (bzg1 + nbzg) + " из них на гарантии: " + bzg1 + '\n' +
                    "КПА всего:  " + (kpa + nkpa) + " из них на гарантии:" + kpa + '\n' +
                    "КПТО всего: " + (kpto + nkpto) + " из них на гарантиии: " + kpto + '\n';
         


                strtowrite = total;

                //strtowrite += row.Cells["unit_num"].Value.ToString() + "   " + row.Cells["product_code"].Value.ToString() + total + '\n';
                return true;
            }*/
            //отчет↑↑↑↑↑
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

        private bool warrantyWork(DataGridViewRow row)//гарантийная наработка
        {
            int workPeriod = (int)row.Cells["operating_hours"].Value;
            if (workPeriod > (int)row.Cells["warranty_hours"].Value) return true;
            else return false;
        }

        private short firstRepairWork(DataGridViewRow row)
        {
            int workPeriod = (int)row.Cells["operating_hours"].Value;
            if (workPeriod > (int)row.Cells["before_first_repair_hours"].Value) return 1;
            else if (workPeriod + 50 >= (int)row.Cells["before_first_repair_hours"].Value && workPeriod != 0) return 2;
            else return 0;
        }

        private short betweenRepairPeriod(DataGridViewRow row)
        {
            int workHours = (int)row.Cells["operating_hours"].Value;
            if (workHours > (int)row.Cells["between_repairs_period"].Value) return 1;
            return 0;
        }
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
                row.Cells["unit_num"].Style.BackColor = Color.Red;
            }
            else if (firstRepairWork(row) == 2)
            {
                row.Cells["before_first_repair_hours"].Style.BackColor = Color.Orange;
                row.Cells["unit_num"].Style.BackColor = Color.Orange;
            }

            TotalBlocksRDC(row);

            //else row.Cells["before_first_repair_hours"].Style.BackColor = Color.PaleGreen;
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
                row.Cells["unit_num"].Style.BackColor = Color.OrangeRed;
                row.Cells["assigned_period"].Style.BackColor = Color.OrangeRed;

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


        private void button1_Click(Object sender, EventArgs e)
        {
            if (rep == null || rep.IsDisposed)
            {
                rep = new report(textBox6.Text);
                rep.Show();
            }
            else
            {
                rep.WindowState = FormWindowState.Maximized;
                rep.Activate();
                rep.BringToFront();
            }

            /*cb = int.Parse(textBox6.Text);
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
           "КПТО всего: " + nkpto + " из них на гарантии сейчас: " + kpto + "(" + Math.Round(kptoProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + kpto2 + "(" + Math.Round(kptoProc2, 2) + " %)" + '\n'+
           "СИД3 всего: " + nsid + " из них на гарантии сейчас: " + sid + "(" + Math.Round(sidProc, 2) + " %)" + "  будет через  " + cb + " месяцев:  " + sid2 + "(" + Math.Round(sidProc2, 2) + " %)" + '\n';
           strtowrite = total;
           File.WriteAllText(Filepath, strtowrite*/
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}