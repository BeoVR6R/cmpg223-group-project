﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace GrindGo
{
    public partial class MaintainCustomers : Form
    {
        public MaintainCustomers()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=BEO-PC\SQLEXPRESS;Initial Catalog=GrindGo;Integrated Security=True");
        string search = "";

        private void MaintainCustomers_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'grindGoDataSet.CUSTOMER' table. You can move, or remove it, as needed.
            this.cUSTOMERTableAdapter.Fill(this.grindGoDataSet.CUSTOMER);

        }

        private void btn_goBack_Click(object sender, EventArgs e)
        {
            this.Close();
            AdminPanel formAdminPanel = new AdminPanel();
            formAdminPanel.Show();
        }

        private void btn_createCustomer_Click(object sender, EventArgs e)
        {
            AddCustomer formaddCustomer = new AddCustomer();
            formaddCustomer.Show();
        }

        private void btn_refreshTable_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand c = new SqlCommand("SELECT firstName, lastName, residentialAddress, emailAddress FROM adminClass.CUSTOMER", conn);
            SqlDataAdapter sda = new SqlDataAdapter(c);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            txBx_searchCustomer.Text = "";
        }

        private void btn_deleteCustomer_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Customer with email address: " + search + "\n Will be deleted, are you sure?", "ALERT", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM adminClass.CUSTOMER WHERE emailAddress = '" + search + "';";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 60;
                    cmd.CommandText = query;

                    conn.Open();
                    cmd.ExecuteScalar();

                    MessageBox.Show("User " + search + " Deleted Successfully.");
                }
                catch
                {
                    MessageBox.Show("Error has occured.");
                }
                finally
                {
                    conn.Close();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("No changes have been made.");
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (txBx_searchCustomer.Text == "")
            {
                MessageBox.Show("Please enter a search field.");
            }
            else
            {
                try
                {
                    search = txBx_searchCustomer.Text;
                    conn.Open();
                    SqlCommand c = new SqlCommand("SELECT * FROM adminCLass.CUSTOMER WHERE emailAddress = '" + search + "'" + ";", conn);
                    SqlDataAdapter sda = new SqlDataAdapter(c);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch
                {
                    MessageBox.Show("Error. Email Address not found");
                }
                finally
                {
                    conn.Close();
                }
            }
            
        }
    }
}
