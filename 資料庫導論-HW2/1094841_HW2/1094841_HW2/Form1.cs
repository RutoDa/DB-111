using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _1094841_HW2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        BindingManagerBase bm;
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|Database1.mdf;" +
                "Integrated Security=True";
                DataSet ds = new DataSet();
                SqlDataAdapter daS = new SqlDataAdapter("Select * From S", cn);
                daS.Fill(ds, "S");
                SqlDataAdapter daSP = new SqlDataAdapter("Select SP.sid_fk, SP.pid_fk, P.pname, SP.cost From SP Inner Join P On SP.pid_fk = P.pid", cn);
                daSP.Fill(ds, "SP");
                ds.Relations.Add("temp", ds.Tables["S"].Columns["sid"],
                    ds.Tables["SP"].Columns["sid_fk"]);
                textBox1.DataBindings.Add("Text", ds, "S.sid");
                textBox2.DataBindings.Add("Text", ds, "S.address");
                textBox3.DataBindings.Add("Text", ds, "S.temp.sid_fk");
                textBox4.DataBindings.Add("Text", ds, "S.temp.pid_fk");
                textBox5.DataBindings.Add("Text", ds, "S.temp.cost");

                bm = this.BindingContext[ds, "S.temp"];
                bm.CurrentChanged += new EventHandler(my_CurrentChanged);

                comboBox1.DataSource = ds;
                comboBox1.DisplayMember = "S.sname";
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "S.temp";
                ShowRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void my_CurrentChanged(object sender, EventArgs e)
        {
            ShowRecord();
        }
        private void ShowRecord() //顯示(現在位置/總數)
        {
            label8.Text = (bm.Position + 1).ToString() + "/" + bm.Count.ToString();
        }
        private void button1_Click(object sender, EventArgs e) // 第一筆
        {
            bm.Position = 0;
            ShowRecord();
        }

        private void button2_Click(object sender, EventArgs e) // 上一筆
        {
            if (bm.Position > 0) bm.Position -= 1;
            ShowRecord();
        }

        private void button3_Click(object sender, EventArgs e) // 下一筆
        {
            if (bm.Position < bm.Count - 1) bm.Position += 1;
            ShowRecord();
        }

        private void button4_Click(object sender, EventArgs e) // 最末筆
        {
            bm.Position = bm.Count - 1;
            ShowRecord();
        }
    }
}
