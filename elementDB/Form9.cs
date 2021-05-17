using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace elementDB
{

    public partial class Form9 : Form
    {
        public Form9()
        {
            //BackColor = Color.LightBlue;
            //BackColor = Color.FromArgb(0, 0, 0);

            //this.BackgroundImage = elementDB.Properties.Resources.gotovo;
            //this.BackgroundImage = "Resources\gotovo.png";
            BackgroundImageLayout = ImageLayout.Stretch;

            InitializeComponent();
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;

            this.Width = 1280;
            this.Height = 720;

            tableLayoutPanel1.Left = (this.ClientSize.Width - tableLayoutPanel1.ClientSize.Width) / 2;
            tableLayoutPanel1.Top = (this.Height - tableLayoutPanel1.Height) / 2 - 35;
            //this.MaximizeBox = false;
            //this.MinimizeBox = false;






            String host = System.Net.Dns.GetHostName();////////////////////////////////////////////////////для того, чтоб не вводить парольна компьютере Ковальчука В.С.
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
            if (ip.ToString() == "192.168.0.55")
            {
                textBox1.Text = "Kn#T@";//пароль администратора (Ковальчука В.С.)
            }
            else if (ip.ToString() == "192.168.0.19")
            {
                textBox1.Text = "dq82d";//пароль разработчика
                                        //System.Threading.Thread.Sleep(1000);
            }
            //else textBox1.Text = "";
        }

            private void button1_Click(object sender, EventArgs e)
        {
            string connStr = "Server=192.168.0.37;charset= utf8;Database=element_db;Uid=element_user;password=lohozavr;";
            MySqlConnection connection = new MySqlConnection(connStr);

            string sql = "SHOW TABLES";

            DataTable dt = SQLCustom.SQL_Request(connection, sql);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Не удалось подключиться к базе данных");
                return;
            }

            sql = string.Format("SELECT user_id from Users where BINARY pass = '{0}'",
                textBox1.Text);
            dt = SQLCustom.SQL_Request(connection, sql);

            if (dt.Rows.Count > 0)
            {
                users access = (users)Convert.ToInt32(dt.Rows[0][0]);

                switch (access)
                {
                    case users.Btk:
                        Form1.access = users.Btk;
                        Form1.userName = "БТК";
                        break;
                    case users.Bras:
                        Form1.access = users.Bras;
                        Form1.userName = "БРАС";
                        break;
                    case users.Bgir:
                        Form1.access = users.Bgir;
                        Form1.userName = "БГИР";
                        break;
                    case users.Root:
                        Form1.access = users.Root;
                        Form1.userName = "Администратор";
                        break;
                    case users.Bmtd:
                        Form1.access = users.Bmtd;
                        Form1.userName = "БМТД";
                        break;
                    case users.Storehouse:
                        Form1.access = users.Storehouse;
                        Form1.userName = "Склад";
                        break;
                    case users.SuperRoot:
                        Form1.access = users.SuperRoot;
                        Form1.userName = "Разработчик";
                        break;
                    case users.Peb:
                        Form1.access = users.Peb;
                        Form1.userName = "ПЭБ";
                        break;
                    case users.Control:
                        Form1.access = users.Control;
                        Form1.userName = "Управление";
                        break;
                    default:
                        break;
                }

                sql = string.Format("INSERT INTO journal_log " +
                    "(date_time, user, operation) " +
                    "VALUES('{0}', '{1}', '{2}')",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Form1.userName, "Вход");

                dt = SQLCustom.SQL_Request(connection, sql);

                Form1 form = new Form1();
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Введите корректный пароль!");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "dq82d";
            //button1.PerformClick();
        }
    }
}
