using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace ARMAdmission
{
    public partial class LoginForm : Form
    {
        
        public bool UserSuccessfullyAuthenticated { get; private set; }
        public bool UserCanEdit { get; private set; }

        public SqlConnection connection { get; private set; }
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            Wizzard wizzard = new Wizzard();
            try
            {
                wizzard.OpenFile();
                wizzard.AuthString(login_box.Text, pass_box.Text);
                try
                {
                    this.connection = new SqlConnection(wizzard.getConnectionString());
                    
                    this.connection.Open();
                    this.UserSuccessfullyAuthenticated = true;
                    
                    MessageBox.Show("Авторизація пройшла успішно", "Information", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    try
                    {
                        SqlCommand query = new SqlCommand($"Select can_edit from user_role where username='{login_box.Text}'", connection);
                        this.UserCanEdit = Convert.ToBoolean(query.ExecuteScalar());
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    this.Close();
                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл не знайдено","Error");
            }
        }


        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
    class Wizzard
    {
        private string __CSPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        private string __CS = "";


        public Wizzard()
        {
            __CSPath = Path.Combine(__CS, "DBConnectionString");
        }
        public bool isFileExist(string path)
        {

            return File.Exists(path);
        }

        public string getPath()
        {
            return __CSPath;
        }

        public string getConnectionString()
        {
            return __CS;
        }

        public void OpenFile()
        {
            if (this.isFileExist(this.__CSPath))
            {
                this.__CS = File.ReadAllText(this.__CSPath);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public void AuthString(string login, string password)
        {
            string[] old_cs = this.__CS.Split(';');
            string user = $"User ID ={login}";
            string pass = $"Password ={password}";

            List<string> old_cs_list = old_cs.ToList();

            old_cs_list.Add(user);
            old_cs_list.Add(pass);
            old_cs = old_cs_list.ToArray();
            __CS = String.Join(";", old_cs);
        }
    }
}
