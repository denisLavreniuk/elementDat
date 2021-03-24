using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace elementDB
{
    public partial class failures : Form
    {
        public int id = -1;
        //private Form12 resourceForm;

        public string m_title;
        private string m_filter;
        private enum Resources { WARRANTY = 1, BEFORE, BETWEEN, ASSIGNED };

        public failures(string filter)
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

            collectUnits();
        }

        public void collectUnits()
        {
            dataGridView1.Rows.Clear();

            string sql = string.Format("SELECT unit_info.unit_id, unit_info.unit_num, " +
                "unit_info.product_code, unit_info.release_date, " +
                "unit_info.failures_count," +
                "product_status_changes.change_name " +
                "from unit_info " +
                "left join refurbished_res on unit_info.unit_id = refurbished_res.unit_id " +
                "left join product_status_changes on unit_info.unit_id = product_status_changes.unit_id " +
                "WHERE product_code like '%{0}%' " +
                "ORDER BY unit_num;", m_filter);


            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

            foreach (DataRow dr in dt.Rows)
            {
                if ((int)dr["failures_count"] != 0)
                {
                    dataGridView1.Rows.Add(dr["unit_id"],
                                           dr["unit_num"],
                                           dr["product_code"],
                                               string.Format("{0:yyyy-MM-dd}",
                                           dr["release_date"]),
                                           dr["failures_count"]);
                    if (dt.Rows.Count > 0)
                    {
                        setRowNumber(dataGridView1);
                        dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                        dataGridView1.ClearSelection();
                    }
                }
            }
        }

        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Lavender;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
