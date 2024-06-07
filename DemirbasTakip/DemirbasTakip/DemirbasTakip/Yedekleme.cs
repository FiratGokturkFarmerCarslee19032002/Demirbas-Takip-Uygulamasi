using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemirbasTakip
{
    public partial class Yedekleme : Form
    {
        SqlConnection con = new SqlConnection("server=DESKTOP-202LLLL;initial catalog=demirbas_takip;integrated security=true");
        public Yedekleme()
        {
            InitializeComponent();
            textBox1.Text = "DESKTOP-202LLLL";
            textBox2.Text = "Demirbas_takip";
        }
        

        private void ydk_Click(object sender, EventArgs e)
        {
            
            string database = con.Database.ToString();
            if (textBox3.Text == string.Empty)
            {
                MessageBox.Show("Dosyanın Kaydedilcek Yerini Seçin");

            }
            else
            {
                string cmd = "BACKUP DATABASE [" + database + "]TO DISK= '" + textBox3.Text + "\\" + "database" + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak'";
                con.Open();
                SqlCommand command = new SqlCommand(cmd, con);
                command.ExecuteNonQuery();
                MessageBox.Show("Veritabanı Yedekleme Başarılı");
                con.Close();
                ydk.Enabled = false;

            }
        }

        private void ydkdon_Click(object sender, EventArgs e)
        {
            string database = con.Database.ToString();
            con.Open();
            try
            {
                string str1 = string.Format("ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                SqlCommand cmd1 = new SqlCommand(str1, con);
                cmd1.ExecuteNonQuery();
                string str2 = "USE MASTER RESTORE DATABASE [" + database + "] FROM DISK='" + textBox4.Text + "'WITH REPLACE;";
                SqlCommand cmd2 = new SqlCommand(str2, con);
                cmd2.ExecuteNonQuery();
                string str3 = string.Format("ALTER DATABASE [" + database + "]SET MULTI_USER");
                SqlCommand cmd3 = new SqlCommand(str3, con);
                cmd3.ExecuteNonQuery();
                MessageBox.Show("Kurtarma İşlemi Başarılı", "Bilgi");
                con.Close();


            }
            catch (Exception)
            {

                MessageBox.Show("ex.Message", "Hata");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminPanel  adminPanel= new AdminPanel();
            adminPanel.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dlg.SelectedPath;
                ydk.Enabled = true;

            }
        }

        private void Yedekleme_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SQL SERVER veritabanı yedek dosyası|*.bak";
            dlg.Title = "Database kurtar";
            if (dlg.ShowDialog() == DialogResult.OK)

            {
                textBox4.Text = dlg.FileName;
                button2.Enabled = true;

            }
        }
    }
}
