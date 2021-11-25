using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class Form1 : Form
    {
        string userLoggedIn = "";
        LoginRegisterForm previousForm;
        ChatController c;
        public Form1(string username, LoginRegisterForm frm)
        {
            InitializeComponent();
            c = new ChatController("server=localhost, 1434;database=chatappdb;user id =chatappuser; password =P@ssword!;");

            //new to user stuff
            DataTable users = c.GetUsers();
            lstUser.DataSource = users;
            lstUser.ValueMember = "USERNAME";


            this.userLoggedIn = username;
            this.lblusername.Text = "Logged in as: " + username;
            this.previousForm = frm;
            timer1.Start();
            try
            {
                c.CreateSchema();
            } catch
            {

            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //this now specifies the user to send the message
            c.SendMessage(txtMessage.Text, userLoggedIn);
            txtMessage.Clear();
        }

        private void btnLoggedOut_Click(object sender, EventArgs e)
        {
            previousForm.Show();
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            previousForm.Show();
            previousForm.HideSplash();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            //THis chooses what column shows up
            //Similar to overriding the ToString()
            lstMessages.ValueMember = "ChatMessage";
            DataTable tmp = c.GetMessages();
            if (tmp.Rows.Count > lstMessages.Items.Count)
            {
                lstMessages.DataSource = tmp;
            }
            lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        }
    }
}
