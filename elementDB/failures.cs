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


namespace elementDB
{
    public partial class failures : Form
    {
        public failures()
        {
            InitializeComponent();
        }

        public void collectUnits()
        {
            dataGridView1.Rows.Clear();

            string sql = string.Format("SELECT unit_info.unit_id, unit_info.unit_num, " +
                "unit_info.product_code, unit_info.release_date, " +
                "failures.failure_type,"+
                "WHERE product_code like '%{0}%' " +
                "ORDER BY unit_num;");

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

            foreach (DataRow dr in dt.Rows)
            {
                dataGridView1.Rows.Add(dr["unit_id"],
                                       dr["unit_num"],
                                       dr["product_code"],
                                       String.Format("{0:yyyy-MM-dd}",
                                       dr["release_date"]),
                                       dr["failure_type"]);
            }
            if (dt.Rows.Count > 0)
            {
                //setRowNumber(dataGridView1);
                //setCalculatedDate();
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                dataGridView1.ClearSelection();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
