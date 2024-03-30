using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2Forms
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Server=deea888;Database=GuitarStore;Integrated Security=true;");
        SqlDataAdapter adapterSuppliers, adapterOrders;
        DataSet ds = new DataSet();
        BindingSource bsSuppliers = new BindingSource();
        BindingSource bsOrders = new BindingSource();
        DataRelation dataRelation;
        SqlCommandBuilder command;
        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            adapterOrders.Update(ds, "ORDERS");

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridOrders.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridOrders.SelectedRows)
                {
                    dataGridOrders.Rows.Remove(row);
                }
                adapterOrders.Update(ds, "ORDERS");
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            bsOrders.MoveNext();
            dataGridOrders.ClearSelection();
            dataGridOrders.Rows[bsOrders.Position].Selected = true;
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            bsOrders.MovePrevious();
            dataGridOrders.ClearSelection();
            dataGridOrders.Rows[bsOrders.Position].Selected = true;
        }

        private void ConnectButton_Click_1(object sender, EventArgs e)
        {
            adapterSuppliers = new SqlDataAdapter("SELECT * FROM SUPPLIERS", conn);
            adapterOrders = new SqlDataAdapter("SELECT * FROM ORDERS", conn);
            command = new SqlCommandBuilder(adapterOrders);

            adapterSuppliers.Fill(ds, "SUPPLIERS");
            adapterOrders.Fill(ds, "ORDERS");
            dataRelation = new DataRelation("FK_Suppliers_Orders",
                ds.Tables["SUPPLIERS"].Columns["SupplierID"],
                ds.Tables["ORDERS"].Columns["supplier_id"]);
            ds.Relations.Add(dataRelation);

            bsSuppliers.DataSource = ds;
            bsSuppliers.DataMember = "SUPPLIERS";
            bsOrders.DataSource = bsSuppliers;
            bsOrders.DataMember = "FK_Suppliers_Orders";
            this.dataGridSupll.DataSource = bsSuppliers;
            this.dataGridSupll.ReadOnly = true;

            this.dataGridOrders.DataSource = bsOrders;
            this.textBoxOrder.DataBindings.Add("Text", bsOrders, "TotalPrice");
            this.textBoxOrder.BackColor= Color.MediumPurple;


        }
    }
}
