using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class LoginRegisterForm : Form
    {
        splashScreen mainForm;
        string connectionString = "server=localhost, 1434;database=chatappdb;user id =chatappuser; password =P@ssword!;";
        public LoginRegisterForm(string action, splashScreen splash)
        {
            InitializeComponent();
            mainForm = splash;
            //always go back to main screen
            if (action =="Login")
            {
                this.txtConfirm.Visible = false;
                this.btnRegister.Visible = false;
                this.lblConfirm.Visible = false;
            }
            else
            {
                //Register button
                this.btnLogin.Visible = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ChatController c = new ChatController(connectionString);
            if(c.LoginUser(this.txtUsername.Text, this.txtPassword.Text))
            {
                //prompt success
                //MessageBox.Show("Login worked");
                Form1 x = new Form1(this.txtUsername.Text, this);
                this.Hide();
                x.Show();
            }
            else
            {
                MessageBox.Show("Login failed");
            }
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
           if (txtPassword.Text == txtConfirm.Text)
            {
                ChatController c = new ChatController(connectionString);
                if(c.RegisterUser(txtUsername.Text, txtPassword.Text, (Bitmap)picAvatar.Image))
                {
                    MessageBox.Show("Registration successful");

                }
                else
                {
                    MessageBox.Show("Registration failed");
                }
            }
           else
                MessageBox.Show("Passwords dont match");


        }

        private void btnback_Click(object sender, EventArgs e)
        {
            mainForm.Show();
            this.Close();
        }

        private void LoginRegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.Show();
        }
        public void HideSplash()
        {
            mainForm.Hide();
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "";
            if(openFileDialog1.ShowDialog() ==DialogResult.OK)
            {
                picAvatar.Image = new Bitmap(openFileDialog1.FileName);

            }
            else
                MessageBox.Show("Invalid image");
        }

        private void btnGetUserAvatar_Click(object sender, EventArgs e)
        {
            ChatController c = new ChatController(connectionString);
            this.picAvatarSearch.Image = c.GetAvatarByUsername(txtUserSearch.Text);
        }
    }
}
