using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class splashScreen : Form
    {
        public splashScreen()
        {
            InitializeComponent();
            ChatController c = new ChatController("server=localhost,1434;database=chatappdb;user id =chatappuser; password =P@ssword!;");
            try
            {
                c.CreateSchema();
            }
            catch(Exception ex)
            {

            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginRegisterForm frm = new LoginRegisterForm("Login", this);
            this.Hide();
            frm.ShowDialog();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            LoginRegisterForm frm = new LoginRegisterForm("Register", this);
            this.Hide();
            frm.ShowDialog();
        }
    }
}
