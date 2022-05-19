using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.tool.xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace elementDB
{
    public partial class Form2 : Form
    {
        private int _id;
        private bool isDeviationChanged = false;
        public string m_unitTitle;
        public string m_numCode;
        public string addInf;//Для добавления информации в разрешения применения (textBox6)

        private Form1 frm1;
        private Form3 frm3;
        private Form4 frm4;
        private Form5 frm5;
        private Form6 frm6;
        private Form7 frm7;
        private Form8 frm8;
        private Form10 frm10;


        public Form2(int id, Form form)
        {
            //BackColor = Color.PowderBlue;
            BackColor = Color.LightSteelBlue;

            InitializeComponent();
            frm1 = form as Form1;
            _id = id;
            makeRequest(_id);
            populateNotesBox();
            setAccessSettings();

            textBox12.Text = "Краткая информация о разрешениии";//подсказка
            //comboBox1.BackColor = Color.Black;
            //if (textBox5.ReadOnly == true) textBox5.BackColor = Color.DarkGray;
            //else if (textBox5.ReadOnly == false) textBox5.BackColor = Color.White;

            //checkBox1.Enabled = false;

            if (textBox1.ReadOnly == true || textBox2.ReadOnly == true || textBox3.ReadOnly == true ||
                textBox4.ReadOnly == true || textBox5.ReadOnly == true || textBox6.ReadOnly == true ||
                textBox7.ReadOnly == true || textBox8.Enabled == true || comboBox1.Enabled == false)
            {
                comboBox1.Enabled = false;
                button1.BackColor = Color.OrangeRed;

                //comboBox1.BackColor = Color.White;
                //textBox1.BackColor = Color.White;
                //textBox2.BackColor = Color.White;
                //KD.BackColor = Color.White;
                //textBox9.BackColor = Color.White;
                //textBox8.BackColor = Color.White;
                //textBox7.BackColor = Color.White;
                //textBox5.BackColor = Color.White;
                //textBox10.BackColor = Color.White;
                //textBox11.BackColor = Color.White;
                //textBox6.BackColor = Color.White;
                //equipmentText.BackColor = Color.White;
                ////checkBox1.Enabled = false;
                //maskedTextBox1.BackColor = Color.White;
                //textBox12.BackColor = Color.White;
                //textBox13.BackColor = Color.White;
                //textBox14.BackColor = Color.White;
                //textBox13.BackColor = Color.White;
                //textBox3.BackColor = Color.White;
                //textBox4.BackColor = Color.White;
                //revisionTB.BackColor = Color.White;






                //comboBox1.Enabled = false;
                //comboBox1.BackColor = Color.DarkGray;
                //textBox1.BackColor = Color.DarkGray;
                //textBox2.BackColor = Color.DarkGray;
                //KD.BackColor = Color.DarkGray;
                //textBox9.BackColor = Color.DarkGray;
                //textBox8.BackColor = Color.DarkGray;
                //textBox7.BackColor = Color.DarkGray;
                //textBox5.BackColor = Color.DarkGray;
                //textBox10.BackColor = Color.DarkGray;
                //textBox11.BackColor = Color.DarkGray;
                //textBox6.BackColor = Color.DarkGray;
                //equipmentText.BackColor = Color.DarkGray;
                ////checkBox1.Enabled = false;
                //maskedTextBox1.BackColor = Color.DarkGray;
                //textBox12.BackColor = Color.DarkGray;
                //textBox13.BackColor = Color.DarkGray;
                //textBox14.BackColor = Color.DarkGray;
                //textBox13.BackColor = Color.DarkGray;
                //textBox3.BackColor = Color.DarkGray;
                //textBox4.BackColor = Color.DarkGray;
                //revisionTB.BackColor = Color.DarkGray;
            }
        }

        public void makeRequest(int id)
        {
            String sql = "SELECT * FROM `unit_info` WHERE `unit_id` = " + id.ToString();

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt != null)
            {
                m_unitTitle = dt.Rows[0]["product_code"].ToString();
                this.Text = m_unitTitle + " Учетные данные";
                m_numCode = dt.Rows[0]["unit_num"].ToString() + " " +
                    dt.Rows[0]["product_code"].ToString();

                label9.Text = dt.Rows[0]["product_code"].ToString();
                textBox1.Text = dt.Rows[0]["unit_num"].ToString();
                textBox2.Text = dt.Rows[0]["product_code"].ToString();
                KD.Text = dt.Rows[0]["izmKD"].ToString();
                equipmentText.Text = dt.Rows[0]["equipment"].ToString();
                textBox3.Text = String.Format("{0:yyyy-MM-dd}", dt.Rows[0]["release_date"]);
                textBox4.Text = GetLastSW(id);
                textBox5.Text = dt.Rows[0]["operating_hours"].ToString();
                textBox6.Text = dt.Rows[0]["remark"].ToString();
                textBox9.Text = dt.Rows[0]["reception_type"].ToString();
                textBox7.Text = GetLastExplPlace(id);
                textBox8.Text = GetLastStatus(id);
                textBox10.Text = GetLastRapair(id);
                comboBox1.Text = dt.Rows[0]["notes"].ToString();
                revisionTB.Text = dt.Rows[0]["revision"].ToString();
            }

            sql = "SELECT deviation_type FROM deviations WHERE unit_id = " + id;

            dt = SQLCustom.SQL_Request(Form1.connection, sql);

            if (dt.Rows.Count > 0)
            {
                textBox11.Text = dt.Rows[0]["deviation_type"].ToString();
            }

            isDeviationChanged = false;
        }

        private string GetLastExplPlace(int id)
        {
            String sql = "SELECT `place_name` FROM `exploit_place` WHERE `unit_id` = " + id.ToString() + " " +
                         "ORDER BY place_id DESC LIMIT 1";

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0]["place_name"].ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetLastSW(int id)
        {
            String sql = "SELECT `sw_version` FROM `software` " +
                         "WHERE `unit_id` = " + id.ToString() + " " +
                         "ORDER BY `implem_date` DESC LIMIT 1";

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0]["sw_version"].ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetFirstSW(int id)
        {
            String sql = "SELECT `sw_version` FROM `software` " +
                         "WHERE `unit_id` = " + id.ToString() + " " +
                         "ORDER BY `implem_date` LIMIT 1";

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0]["sw_version"].ToString();
            }
            else
            {
                return "";
            }
        }

        private string GetLastStatus(int id)
        {
            String sql = "SELECT `change_name` FROM `product_status_changes` " +
                         "WHERE `unit_id` = " + id.ToString() + " " +
                         "ORDER BY change_id DESC LIMIT 1";

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0]["change_name"].ToString();
            }
            else
            {
                return "";
            }
        }

        private string GetLastRapair(int id)
        {
            String sql = "SELECT `r_date` FROM `repairs` " +
                         "WHERE `unit_id` = " + id.ToString() + " " +
                         "ORDER BY `repair_id` DESC LIMIT 1";

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                return string.Format("{0:yyyy-MM-dd}", dt.Rows[0]["r_date"]);
            }
            else
            {
                return "";
            }
        }

        private string GetFirstRapair(int id)
        {
            String sql = "SELECT `r_date` FROM `repairs` " +
                         "WHERE `unit_id` = " + id.ToString() + " " +
                         "ORDER BY `repair_id` LIMIT 1";

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                return string.Format("{0:yyyy-MM-dd}", dt.Rows[0]["r_date"]);
            }
            else
            {
                return "";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBox12.Clear();
            if (textBox12.ReadOnly == true)
            {
                textBox1.BackColor = Color.DarkGray;
                textBox2.BackColor = Color.White;//
                KD.BackColor = Color.White;//
                textBox9.BackColor = Color.DarkGray;
                textBox8.BackColor = Color.DarkGray;
                textBox7.BackColor = Color.DarkGray;
                textBox5.BackColor = Color.White;//
                textBox10.BackColor = Color.DarkGray;
                textBox11.BackColor = Color.White;//
                textBox6.BackColor = Color.DarkGray;
                equipmentText.BackColor = Color.White;//
                maskedTextBox1.BackColor = Color.White;//
                textBox12.BackColor = Color.White;//
                textBox13.BackColor = Color.White;//
                textBox14.BackColor = Color.White;//
                textBox3.BackColor = Color.DarkGray;
                textBox4.BackColor = Color.DarkGray;
                revisionTB.BackColor = Color.White;//
                comboBox1.BackColor = Color.White;//
                button1.BackColor = Color.GreenYellow;

                comboBox1.Enabled = true;
                textBox5.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox2.ReadOnly = false;
                KD.ReadOnly = false;
                equipmentText.ReadOnly = false;
                revisionTB.ReadOnly = false;
                checkBox1.Enabled = true;
                maskedTextBox1.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
                addBtn.Enabled = true;
                ControlBox = false;
            }
            else
            {
                textBox1.BackColor = Color.White;
                textBox2.BackColor = Color.White;
                KD.BackColor = Color.White;
                textBox9.BackColor = Color.White;
                textBox8.BackColor = Color.White;
                textBox7.BackColor = Color.White;
                textBox5.BackColor = Color.White;
                textBox10.BackColor = Color.White;
                textBox11.BackColor = Color.White;
                textBox6.BackColor = Color.DarkGray;
                equipmentText.BackColor = Color.White;
                maskedTextBox1.BackColor = Color.White;
                textBox12.BackColor = Color.White;
                textBox13.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                textBox3.BackColor = Color.White;
                textBox4.BackColor = Color.White;
                revisionTB.BackColor = Color.White;
                comboBox1.BackColor = Color.White;
                button1.BackColor = Color.OrangeRed;


                comboBox1.Enabled = false;
                textBox5.ReadOnly = true;
                textBox11.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox2.ReadOnly = true;
                KD.ReadOnly = true;
                //textBox6.ReadOnly = true;
                equipmentText.ReadOnly = true;
                revisionTB.ReadOnly = true;

                checkBox1.Enabled = false;
                checkBox1.Checked = false;
                maskedTextBox1.ReadOnly = true;
                textBox12.ReadOnly = true;
                textBox13.ReadOnly = true;
                textBox14.ReadOnly = true;
                addBtn.Enabled = false;
            }
            //if (textBox12.ReadOnly == true)
            //{
            //    comboBox1.Enabled = true;
            //    comboBox1.BackColor = Color.White;
            //    textBox5.ReadOnly = false;
            //    textBox5.BackColor = Color.White;
            //    textBox11.ReadOnly = false;
            //    textBox11.BackColor = Color.White;
            //    textBox2.ReadOnly = false;
            //    textBox2.BackColor = Color.White;
            //    KD.ReadOnly = false;
            //    KD.BackColor = Color.White;
            //    equipmentText.ReadOnly = false;
            //    equipmentText.BackColor = Color.White;
            //    revisionTB.ReadOnly = false;
            //    revisionTB.BackColor = Color.White;

            //    checkBox1.Enabled = true;
            //    maskedTextBox1.ReadOnly = false;
            //    maskedTextBox1.BackColor = Color.White;
            //    textBox12.ReadOnly = false;
            //    textBox12.BackColor = Color.White;
            //    textBox13.ReadOnly = false;
            //    textBox13.BackColor = Color.White;
            //    textBox14.ReadOnly = false;
            //    textBox14.BackColor = Color.White;
            //    addBtn.Enabled = true;
            //    ControlBox = false;
            //}
            //else
            //{
            //    comboBox1.Enabled = false;
            //    comboBox1.BackColor = Color.DarkGray;
            //    textBox5.ReadOnly = true;
            //    textBox5.BackColor = Color.DarkGray;
            //    textBox11.ReadOnly = true;
            //    textBox11.BackColor = Color.DarkGray;
            //    textBox6.ReadOnly = true;
            //    textBox6.BackColor = Color.DarkGray;
            //    textBox2.ReadOnly = true;
            //    textBox2.BackColor = Color.DarkGray;
            //    KD.ReadOnly = true;
            //    KD.BackColor = Color.DarkGray;
            //    //textBox6.ReadOnly = true;
            //    equipmentText.ReadOnly = true;
            //    equipmentText.BackColor = Color.DarkGray;
            //    revisionTB.ReadOnly = true;
            //    revisionTB.BackColor = Color.DarkGray;

            //    checkBox1.Enabled = false;
            //    checkBox1.Checked = false;
            //    maskedTextBox1.ReadOnly = true;
            //    maskedTextBox1.BackColor = Color.DarkGray;
            //    textBox12.ReadOnly = true;
            //    textBox12.BackColor = Color.DarkGray;
            //    textBox13.ReadOnly = true;
            //    textBox13.BackColor = Color.DarkGray;
            //    textBox14.ReadOnly = true;
            //    textBox14.BackColor = Color.DarkGray;
            //    addBtn.Enabled = false;
            //}
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (revisionTB.Text == "") revisionTB.Text = "0";

            Cursor = Cursors.WaitCursor;
            string sql = "BEGIN;";
            string product_code;
            string izmKD;
            string operating_hours;
            string remark;
            string notes;
            string sqlOld;
            string equipment;
            string revision;

            string sqlJournal = "BEGIN; ";

            sqlOld = string.Format("Select " +
                "product_code, izmKD, equipment, operating_hours, remark, notes, revision " +
                "FROM unit_info WHERE unit_id = {0}", _id);

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sqlOld);

            product_code = dt.Rows[0]["product_code"].ToString();
            izmKD = dt.Rows[0]["izmKD"].ToString();
            operating_hours = dt.Rows[0]["operating_hours"].ToString();
            remark = dt.Rows[0]["remark"].ToString();
            notes = dt.Rows[0]["notes"].ToString();
            equipment = dt.Rows[0]["remark"].ToString();
            revision = dt.Rows[0]["revision"].ToString();


            sqlJournal += "INSERT INTO journal_log " +
                "(date_time, user, operation, num_code, property, old_value, new_value) " +
                "VALUES ";

            if (product_code != textBox2.Text)
            {
                sqlJournal += string.Format("('{0}', '{1}', '{2}', '{3}', " +
                    "'{4}', '{5}', '{6}'), ",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Form1.userName,
                    "Изменение",
                    textBox1.Text + " " + product_code,
                    "Шифр",
                    product_code,
                    textBox2.Text);
            }

            /*if (revision != revisionTB.Text)

                if (operating_hours != textBox5.Text)
                {
                    sqlJournal += string.Format("('{0}', '{1}', '{2}', '{3}', " +
                        "'{4}', '{5}', '{6}'), ",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                        Form1.userName,
                        "Изменение",
                        textBox1.Text + " " + product_code,
                        "Наработка",
                        operating_hours,
                        textBox5.Text);
                }
            */
            if (remark != textBox6.Text)
            {
                sqlJournal += string.Format("('{0}', '{1}', '{2}', '{3}', " +
                    "'{4}', '{5}', '{6}'), ",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Form1.userName,
                    "Изменение",
                    textBox1.Text + " " + product_code,
                    "Разрешение применения",
                    remark,
                    textBox6.Text);
            }

            if (notes != comboBox1.Text)
            {
                sqlJournal += string.Format("('{0}', '{1}', '{2}', '{3}', " +
                    "'{4}', '{5}', '{6}'), ",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Form1.userName,
                    "Изменение",
                    textBox1.Text + " " + product_code,
                    "Примечания",
                    notes,
                    comboBox1.Text);
            }

            sql += string.Format("UPDATE `unit_info` SET " +
                                       "product_code = '{0}', " +
                                       "izmKD ='{1}', " +
                                       "operating_hours = '{2}', " +
                                       "remark = '{3}', " +
                                       "last_update = '{4}', " +
                                       "notes = '{5}', " +
                                       "equipment = '{6}', " +
                                       "revision = '{7}' " +
                                       "WHERE unit_info.unit_id = {8}; ",
                                       textBox2.Text, KD.Text, textBox5.Text, textBox6.Text,
                                       DateTime.Today.ToString("yyyy-MM-dd"),
                                       comboBox1.Text, equipmentText.Text, revisionTB.Text, _id);

            if (isDeviationChanged)
            {
                string sql2 = string.Format("SELECT * FROM deviations " +
                    "WHERE unit_id = {0};", _id);

                dt = SQLCustom.SQL_Request(Form1.connection, sql2);

                if (dt.Rows.Count > 0)
                {
                    sql += string.Format("UPDATE deviations SET " +
                                    "deviation_type = '{0}' " +
                                    "WHERE unit_id = {1}; ",
                                    textBox11.Text, _id);

                    if (dt.Rows[0]["deviation_type"].ToString() == textBox11.Text)
                    {
                        sqlJournal += string.Format("('{0}', '{1}', '{2}', '{3}', " +
                           "'{4}', '{5}', '{6}'), ",
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                           Form1.userName,
                           "Изменение",
                           textBox1.Text + " " + product_code,
                           "Отклонение от КД",
                           dt.Rows[0]["deviation_type"].ToString(),
                           textBox11.Text);
                    }
                }
                else
                {
                    sql += string.Format("INSERT INTO deviations " +
                        "(unit_id, deviation_type) VALUES ({0}, '{1}'); ",
                        _id, textBox11.Text);
                }
            }

            sql += "COMMIT;";

            sqlJournal = sqlJournal.Remove(sqlJournal.Length - 2, 2);
            sqlJournal += ';';

            sqlJournal += "COMMIT;";

            DataTable dtJournal = SQLCustom.SQL_Request(Form1.connection, sqlJournal);

            dt = SQLCustom.SQL_Request(Form1.connection, sql);

            if (dt.ToString() == "")
            {
                generalDataUpdated();
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox11.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox2.ReadOnly = true;
                KD.ReadOnly = true;
                equipmentText.ReadOnly = true;
                comboBox1.Enabled = false;
                revisionTB.ReadOnly = true;
                makeRequest(_id);
                //MessageBox.Show("Данные успешно сохранены");
                Cursor = Cursors.Default;
                ControlBox = true;
            }
            else
            {
                makeRequest(_id);
                MessageBox.Show("Ошибка записи в БД!");
                Cursor = Cursors.Default;
                ControlBox = true;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (frm3 == null || frm3.IsDisposed)
            {
                string title = m_unitTitle + " - Ресурсы";
                frm3 = new Form3(_id, title);
                frm3.Show();
            }
            else
            {
                frm3.WindowState = FormWindowState.Normal;
                frm3.Activate();
                frm3.BringToFront();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (frm5 == null || frm5.IsDisposed)
            {
                frm5 = new Form5(_id, this);
                frm5.Show();
            }
            else
            {
                frm5.WindowState = FormWindowState.Normal;
                frm5.Activate();
                frm5.BringToFront();
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (frm6 == null || frm6.IsDisposed)
            {
                frm6 = new Form6(_id, this);
                frm6.Show();
            }
            else
            {
                frm6.WindowState = FormWindowState.Normal;
                frm6.Activate();
                frm6.BringToFront();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (frm4 == null || frm4.IsDisposed)
            {
                frm4 = new Form4(_id, this);
                frm4.Show();
            }
            else
            {
                frm4.WindowState = FormWindowState.Normal;
                frm4.Activate();
                frm4.BringToFront();
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (frm7 == null || frm7.IsDisposed)
            {
                frm7 = new Form7(_id, this);
                frm7.Show();
            }
            else
            {
                frm7.WindowState = FormWindowState.Normal;
                frm7.Activate();
                frm7.BringToFront();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Вы уверены, что хотите удалить всю информацию по данному изделию?",
                "Предупреждение", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show(
                    "Вы точно уверены, что хотите удалить всю информацию по данному изделию?",
                    "Предупреждение", MessageBoxButtons.YesNo);

                if (dialogResult2 == DialogResult.Yes)
                {
                    String sql = "BEGIN;";

                    sql += String.Format("DELETE FROM `unit_info` WHERE unit_id = {0};", _id);

                    sql += string.Format("UPDATE unit_info SET last_update = '{0}' " +
                         "WHERE unit_id = {1};", DateTime.Today.ToString("yyyy-MM-dd"), _id);

                    sql += "COMMIT;";

                    DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

                    if (dt.ToString() != "")
                    {
                        MessageBox.Show("Ошибка удаления");
                    }
                    else
                    {
                        MessageBox.Show("Удаление выполнено успешно");

                        sql = string.Format("INSERT INTO journal_log " +
                             "(date_time, user, operation, num_code) " +
                             "VALUES('{0}', '{1}', '{2}', '{3}')",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                             Form1.userName, "Удаление блока",
                             textBox1.Text + textBox2.Text);

                        dt = SQLCustom.SQL_Request(Form1.connection, sql);

                        frm1.unitDeleted();
                        this.Close();
                    }
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Form1.collectUnits();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (frm8 == null || frm8.IsDisposed)
            {
                frm8 = new Form8(_id, this);
                frm8.Show();
            }
            else
            {
                frm8.WindowState = FormWindowState.Normal;
                frm8.Activate();
                frm8.BringToFront();
            }
        }

        private void setAccessSettings()
        {
            button8.Enabled = false;

            switch (Form1.access)
            {
                case users.Control:
                    break;
                case users.Peb:
                    break;
                case users.Storehouse:
                    break;
                case users.Bgir:
                    break;
                case users.Bmtd:
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button11.Enabled = false;
                    break;
                case users.Bras:
                    button1.Enabled = false;
                    button2.Enabled = false;
                    break;
                case users.Btk:
                    break;
                case users.SuperRoot:
                    button8.Enabled = true;
                    button8.Visible = true;
                    checkBox1.Visible = true;
                    break;
                case users.Root:
                    button8.Enabled = true;
                    button8.Visible = true;
                    checkBox1.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (frm10 == null || frm10.IsDisposed)
            {
                string title = m_unitTitle;
                frm10 = new Form10(_id, this);
                frm10.Show();
            }
            else
            {
                frm10.WindowState = FormWindowState.Normal;
                frm10.Activate();
                frm10.BringToFront();
            }
        }

        private void createFile()
        {
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");

            BaseFont baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Font f = new iTextSharp.text.Font(baseFont, 12);
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);

            SaveFileDialog savepdf = new SaveFileDialog();
            savepdf.Filter = "PDF Files |*.pdf";

            if (savepdf.ShowDialog() == DialogResult.OK)
            {
                Document pdf = new Document(PageSize.A4, 60, 20, 20, 60);

                PdfWriter.GetInstance(pdf, new FileStream(savepdf.FileName, FileMode.Create));

                PdfPTable failuresTable = new PdfPTable(4);
                PdfPTable softwareTable = new PdfPTable(4);
                PdfPTable worksTable = new PdfPTable(6);
                PdfPTable statusTable = new PdfPTable(3);
                //PdfPTable placeTable = new PdfPTable(5);
                PdfPTable repairTable = new PdfPTable(4);
                //
                PdfPTable headerTable1 = new PdfPTable(6);
                PdfPTable headerTable2 = new PdfPTable(3);
                PdfPTable workHoursTable = new PdfPTable(5);
                PdfPTable placeTable = new PdfPTable(2);
                PdfPTable contractsTable = new PdfPTable(4);
                PdfPTable dispatchesTable = new PdfPTable(3);

                Form4 failuresForm = new Form4(_id, this);
                Form5 softwareForm = new Form5(_id, this);
                Form6 worksForm = new Form6(_id, this);
                Form7 statusForm = new Form7(_id, this);
                Form8 placeForm = new Form8(_id, this);
                Form10 repairForm = new Form10(_id, this);

                Paragraph rusParagraph;
                PdfPCell cell;

                PdfPTable resourceTable = new PdfPTable(6);

                pdf.Open();

                rusParagraph = new Paragraph(DateTime.Today.Date.ToString("yyyy-MM-dd"), f);
                rusParagraph.Alignment = Element.ALIGN_RIGHT;
                pdf.Add(rusParagraph);

                rusParagraph = new Paragraph("Сведения\n\n", titleFont);
                rusParagraph.Alignment = Element.ALIGN_CENTER;
                pdf.Add(rusParagraph);

                cell = new PdfPCell(new Paragraph());
                cell.HorizontalAlignment = 1;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                headerTable1.DefaultCell.Border = 0;
                headerTable1.SetWidths(new float[] { 13, 25, 15, 10, 20, 16 });
                headerTable1.DefaultCell.HorizontalAlignment = 1;
                headerTable1.WidthPercentage = 100f;
                headerTable1.AddCell(new Paragraph("Изделие", f));
                cell.Phrase = new Paragraph(textBox2.Text, f);
                headerTable1.AddCell(cell);
                headerTable1.AddCell(new Paragraph("Заводской номер", f));
                cell.Phrase = new Paragraph(textBox1.Text, f);
                headerTable1.AddCell(cell);
                headerTable1.AddCell(new Paragraph("Дата выпуска", f));
                cell.Phrase = new Paragraph(textBox3.Text, f);
                headerTable1.AddCell(cell);

                pdf.Add(headerTable1);
                pdf.Add(rusParagraph = new Paragraph("\n", f));

                headerTable2.WidthPercentage = 100f;
                headerTable2.DefaultCell.HorizontalAlignment = 1;
                headerTable2.DefaultCell.BorderWidth = 0;
                headerTable2.AddCell(new Paragraph("Текущая версия ПО", f));
                headerTable2.AddCell(new Paragraph("", f));
                headerTable2.AddCell(new Paragraph("Статус изделия", f));
                cell.Phrase = new Paragraph(textBox4.Text, f);
                //cell.Border = 15;
                headerTable2.AddCell(cell);
                headerTable2.AddCell(new Paragraph("", f));
                cell.Phrase = new Paragraph(textBox8.Text, f);
                headerTable2.AddCell(cell);

                pdf.Add(headerTable2);
                pdf.Add(new Paragraph("\n", f));

                workHoursTable.DefaultCell.Border = 0;
                workHoursTable.DefaultCell.HorizontalAlignment = 1;
                workHoursTable.WidthPercentage = 100f;
                workHoursTable.AddCell(new Paragraph("Наработка, час", f));
                workHoursTable.AddCell(new Paragraph("СНЭ", f));
                workHoursTable.AddCell(new Paragraph("ППР", f));
                workHoursTable.AddCell(new Paragraph("Кол-во ремонтов", f));
                workHoursTable.AddCell(new Paragraph("Дата последнего ремонта", f));
                workHoursTable.AddCell(new Paragraph(" ", f));
                cell.Phrase = new Paragraph(textBox5.Text, f);
                workHoursTable.AddCell(cell);
                cell.Phrase = new Paragraph(calculateAfterFirstRepair().ToString(), f);
                workHoursTable.AddCell(cell);   // PPR hz
                cell.Phrase = new Paragraph(repairForm.dataGridView1.Rows.Count.ToString(), f);
                workHoursTable.AddCell(cell);
                cell.Phrase = new Paragraph(textBox10.Text, f);
                workHoursTable.AddCell(cell);
                pdf.Add(workHoursTable);
                pdf.Add(new Paragraph("\n", f));

                placeTable.SetWidths(new float[] { 25, 75 });
                placeTable.DefaultCell.Border = 0;
                placeTable.DefaultCell.HorizontalAlignment = 1;
                placeTable.WidthPercentage = 100f;
                placeTable.AddCell(new Paragraph("Место эксплуатации", f));
                cell.Phrase = new Paragraph(textBox7.Text, f);
                placeTable.AddCell(cell);
                pdf.Add(placeTable);
                pdf.Add(new Paragraph("\n", f));

                string sql = string.Format("SELECT contract_num, " +
                    "contract_date, associate " +
                    "FROM contracts WHERE unit_id = {0}", _id);

                DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

                contractsTable.SetWidths(new float[] { 22, 32, 14, 32 });
                contractsTable.DefaultCell.Border = 0;
                contractsTable.DefaultCell.HorizontalAlignment = 1;
                contractsTable.WidthPercentage = 100f;
                contractsTable.AddCell(new Paragraph("Договор поставки", f));
                contractsTable.AddCell(new Paragraph("№ Договора", f));
                contractsTable.AddCell(new Paragraph("Дата", f));
                contractsTable.AddCell(new Paragraph("С кем заключен", f));
                contractsTable.AddCell(new Paragraph(" ", f));

                if (dt.Rows.Count > 0)
                {
                    cell.Phrase = new Paragraph(dt.Rows[0]["contract_num"].ToString(), f);
                    contractsTable.AddCell(cell);
                    cell.Phrase = new Paragraph(string.Format("{0:yyyy-MM-dd}", dt.Rows[0]["contract_date"]), f);
                    contractsTable.AddCell(cell);
                    cell.Phrase = new Paragraph(dt.Rows[0]["associate"].ToString(), f);
                    contractsTable.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Paragraph(" ", f);
                    contractsTable.AddCell(cell);
                    cell.Phrase = new Paragraph(" ", f);
                    contractsTable.AddCell(cell);
                    cell.Phrase = new Paragraph(" ", f);
                    contractsTable.AddCell(cell);
                }
                pdf.Add(contractsTable);
                pdf.Add(new Paragraph("\n", f));

                sql = string.Format("SELECT date_sending, invoice, " +
                    "FROM unit_dispatches; WHERE unit_id = {0}", _id);

                dt = SQLCustom.SQL_Request(Form1.connection, sql);

                dispatchesTable.SetWidths(new float[] { 25, 15, 60 });
                dispatchesTable.DefaultCell.Border = 0;
                dispatchesTable.DefaultCell.HorizontalAlignment = 1;
                dispatchesTable.WidthPercentage = 100f;
                dispatchesTable.AddCell(new Paragraph("Реквизиты отправки", f));
                dispatchesTable.AddCell(new Paragraph("Дата", f));
                dispatchesTable.AddCell(new Paragraph("Накладная", f));

                if (dt.Rows.Count > 0)
                {
                    dispatchesTable.AddCell(new Paragraph(" "));
                    cell.Phrase = new Paragraph(string.Format("{0:yyyy-MM-dd}", dt.Rows[0]["date_sending"]), f);
                    dispatchesTable.AddCell(cell);
                    cell.Phrase = new Paragraph(dt.Rows[0]["invoice"].ToString(), f);
                    dispatchesTable.AddCell(cell);
                }
                else
                {
                    dispatchesTable.AddCell(new Paragraph(" "));
                    cell.Phrase = new Paragraph(" ", f);
                    dispatchesTable.AddCell(cell);
                    cell.Phrase = new Paragraph(" ", f);
                    dispatchesTable.AddCell(cell);
                }
                pdf.Add(dispatchesTable);
                pdf.Add(new Paragraph("\n", f));

                pdf.Add(new Paragraph("Ресурс\n\n", f));
                if (createResourceTable(_id, resourceTable, f))
                {
                    pdf.Add(resourceTable);
                }

                pdf.Add(new Paragraph("Изменения ПО\n\n", titleFont));
                addTableToFile(softwareForm.dataGridView1, softwareTable, pdf, f);

                pdf.Add(new Paragraph("Отклонение от КД", titleFont));
                pdf.Add(new Paragraph(textBox11.Text, f));

                pdf.Add(new Paragraph("Разрешение применения", titleFont));
                pdf.Add(new Paragraph(textBox6.Text, f));

                pdf.Add(new Paragraph("Список отказов\n\n", titleFont));
                addTableToFile(failuresForm.dataGridView1, failuresTable, pdf, f);

                pdf.Add(new Paragraph("Список работ\n\n", titleFont));
                addTableToFile(worksForm.dataGridView1, worksTable, pdf, f);

                pdf.Add(new Paragraph("Ремонт\n\n", titleFont));
                addTableToFile(repairForm.dataGridView1, repairTable, pdf, f);

                pdf.Close();

                MessageBox.Show("Файл успешно создан");
            }
        }

        /*private string isWarranty(int id)
        {
            string sql = "SELECT period_value FROM varranty_res " +
                         "WHERE unit_id = " + id;
            DataTable varrantyRes = SQLCustom.SQL_Request(Form1.connection, sql);

            if (varrantyRes.Rows.Count == 0)
            {
                return "-";
            }
            DateTime releaseDate = Convert.ToDateTime(textBox3.Text);
            double difference = (DateTime.Today - releaseDate).TotalDays;

            int varrantyPeriod = (int)varrantyRes.Rows[0]["period_value"];

            if (difference > varrantyPeriod * 30 + varrantyPeriod / 2)
            {
                return "-";
            }
            else
            {
                return releaseDate.AddMonths(varrantyPeriod).ToShortDateString();
            }
        }*/

        private bool createResourceTable(int id, PdfPTable table, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell();
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.SetWidths(new float[] { 17, 17, 13, 19, 17, 17 });
            table.WidthPercentage = 100f;
            table.DefaultCell.HorizontalAlignment = 1;
            table.AddCell("");
            table.AddCell(new Paragraph("Гарантийный", font));
            table.AddCell(new Paragraph("До 1-го ремонта", font));
            table.AddCell(new Paragraph("Межремонтный", font));
            table.AddCell(new Paragraph("Назначенный", font));
            table.AddCell(new Paragraph("По тех. сотоянию", font));

            String sql = "SELECT varranty_res.period_value, " +
                        "bef_first_repair_res.period_value, " +
                        "between_repairs_res.period_value, " +
                        "assigned_res.period_value, " +
                        "refurbished_res.period_value, " +
                        "varranty_res.operating_hours, " +
                        "bef_first_repair_res.operating_hours, " +
                        "between_repairs_res.operating_hours, " +
                        "assigned_res.operating_hours, " +
                        "refurbished_res.operating_hours, " +
                        "unit_info.var_stor_period " +
                        "FROM unit_info, varranty_res, bef_first_repair_res, " +
                        "between_repairs_res, assigned_res, refurbished_res " +
                        "WHERE unit_info.unit_id = varranty_res.unit_id AND " +
                        "unit_info.unit_id = bef_first_repair_res.unit_id AND " +
                        "unit_info.unit_id = between_repairs_res.unit_id AND " +
                        "unit_info.unit_id = assigned_res.unit_id AND " +
                        "unit_info.unit_id = refurbished_res.unit_id AND " +
                        "unit_info.unit_id = " + id.ToString();

            table.AddCell(new Paragraph("Срок экспл., мес.", font));

            //table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;

            DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < 5; ++i)
                {
                    cell.Phrase = new Paragraph(dt.Rows[0][i].ToString(), font);
                    table.AddCell(cell);
                }
                table.AddCell(new Paragraph("Наработка, час.", font));

                for (int i = 5; i < 10; ++i)
                {
                    cell.Phrase = new Paragraph(dt.Rows[0][i].ToString(), font);
                    table.AddCell(cell);
                }
                table.AddCell(new Paragraph("Срок хранения, мес.", font));

                cell.Phrase = new Paragraph(dt.Rows[0][10].ToString(), font);
                table.AddCell(cell);

                for (int i = 0; i < 5; ++i)
                {
                    cell.Phrase = new Paragraph("", font);
                    table.AddCell(cell);
                }

                return true;
            }
            return false;
        }

        private void addTableToFile(DataGridView dgv, PdfPTable table, Document file, iTextSharp.text.Font f)
        {
            table.WidthPercentage = 100f;

            if (dgv.Rows.Count > 0)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Visible)
                        table.AddCell(new Paragraph(column.HeaderText, f));
                }
            }

            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 1; i < row.Cells.Count; ++i)
                {
                    table.AddCell(new Paragraph(row.Cells[i].Value.ToString(), f));
                }
            }
            file.Add(table);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            createFile();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            isDeviationChanged = true;
        }

        public void generalDataUpdated()
        {
            frm1.collectUnits();
        }

        private void populateNotesBox()
        {
            string sql = "SELECT DISTINCT notes FROM unit_info";

            DataTable result = SQLCustom.SQL_Request(Form1.connection, sql);

            comboBox1.Items.Clear();

            foreach (DataRow row in result.Rows)
            {
                comboBox1.Items.Add(row[0]);
            }
        }

        private int calculateAfterFirstRepair()
        {
            int res = 0;
            if (textBox10.Text != "")
            {
                if (Int32.TryParse(textBox5.Text, out res))
                {
                    string sql = string.Format("SELECT operating_hours FROM repairs " +
                    "WHERE unit_id = {0}", _id);

                    DataTable dt = SQLCustom.SQL_Request(Form1.connection, sql);

                    foreach (DataRow row in dt.Rows)
                    {
                        res -= (int)row["operating_hours"];
                    }
                }
            }
            return res;
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            addInf = "Разрешение №" + maskedTextBox1.Text + " от " + dateTimePicker1.Value.ToString() + ". " +
                textBox12.Text + ". Действует с №" + textBox13.Text + ", на партию " + textBox14.Text + " шт." + Environment.NewLine;
            textBox6.Text += addInf;
        }
        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.TextLength == 4)
            {
                SendKeys.Send("{tab}");
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.TextLength == 2)
            {
                SendKeys.Send("{tab}");
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //textBox6.SelectionStart = textBox6.Text.Length;//скролл вниз по мере появления записей в textbox
            //textBox6.ScrollToCaret();//скролл вниз по мере появления записей в textbox
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox6.ReadOnly = false;
                textBox6.BackColor = Color.White;
            }
            else
            {
                textBox6.ReadOnly = true;
                textBox6.BackColor = Color.DarkGray;
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            textBox12.ForeColor = Color.Black;
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acceptanceButton_Click(object sender, EventArgs e)
        {

        }

        private void revisionTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void revisionTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}