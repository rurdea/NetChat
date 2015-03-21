using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace CriChat
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            // load current connection string
            txtConnectionString.Text = ConfigurationManager.ConnectionStrings["CriChat.Properties.Settings.DatabaseConnectionString"].ConnectionString;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // try to connect to the database
            try
            {
                using (SqlConnection con = new SqlConnection(txtConnectionString.Text))
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid connection string. " + ex.ToString());
                return;
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["CriChat.Properties.Settings.DatabaseConnectionString"].ConnectionString = txtConnectionString.Text;
            config.Save();

            this.Close();
        }
    }
}
