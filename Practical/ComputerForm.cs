using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practical
{
    public partial class ComputerForm : Form
    {
        ExamEntities1 entity = new ExamEntities1();

        public ComputerForm()
        {
            InitializeComponent();
            loadData();
        }

        void loadData()
        {
            entity = new ExamEntities1();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = entity.Computers.ToList();
            dataGridView1.Refresh();
        }

        void addComputer()
        {
            Computer c = new Computer();
            c.Manufacturer = txtManu.Text;
            c.Model = txtModel.Text;
            c.Type = txtType.Text;
            c.ManufacturingDate = dtpManuDate.Value.ToShortDateString();
            c.Color = txtColor.Text;
            c.Weight = double.Parse(txtWeight.Text);
            c.Price = double.Parse(txtPrice.Text);
            entity.Computers.Add(c);
            entity.SaveChanges();
        }

        bool checkValid()
        {
            errorProvider1.Clear();

            bool error = false;
            if (string.IsNullOrEmpty(txtManu.Text))
            {
                errorProvider1.SetError(txtManu, "Must not be empty");
                error = true;
            }

            if (string.IsNullOrEmpty(txtModel.Text))
            {
                errorProvider1.SetError(txtModel, "Must not be empty");
                error = true;
            }

            if (string.IsNullOrEmpty(txtType.Text))
            {
                errorProvider1.SetError(txtType, "Must not be empty");
                error = true;
            }

            DateTime dt = dtpManuDate.Value;
            if (dt.Year < 1990 || dt.Year > 2010)
            {
                errorProvider1.SetError(dtpManuDate, "Year must between 1990 and 2010");
                error = true;
            }

            if (string.IsNullOrEmpty(txtColor.Text))
            {
                errorProvider1.SetError(txtColor, "Must not be empty");
                error = true;
            }

            try
            {
                double kg = double.Parse(txtWeight.Text);
                if (kg < 1)
                {
                    errorProvider1.SetError(txtWeight, "Weight must equal or larger than 1 kg");
                    error = true;
                }
            }
            catch (Exception)
            {
                errorProvider1.SetError(txtWeight, "Digit only");
                error = true;
            }

            try
            {
                double price = double.Parse(txtPrice.Text);
                if (price < 0)
                {
                    errorProvider1.SetError(txtPrice, "Price must equal or larger than 0 USD");
                    error = true;
                }
            }
            catch (Exception)
            {
                errorProvider1.SetError(txtPrice, "Digit only");
                error = true;
            }

            return error;
        }

        void deleteData(int id)
        {
            Computer c = entity.Computers.First(x => x.ComputerID == id);
            entity.Computers.Remove(c);
            entity.SaveChanges();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!checkValid())
            {
                addComputer();
                loadData();
                MessageBox.Show("Your computer has been added!");
                lbStatus.Text = "Add is completed";
            } else
            {
                lbStatus.Text = "Invalid data found";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dataGridView1.SelectedRows[0];
                int id = int.Parse(r.Cells[0].Value.ToString());
                Computer c = entity.Computers.First(x => x.ComputerID == id);
                DetailForm detail = new DetailForm(c);
                lbStatus.Text = "Editing";
                DialogResult rs = detail.ShowDialog();
                lbStatus.Text = "Data edited";
                loadData();
            } else
            {
                MessageBox.Show("No row is selected!");
                lbStatus.Text = "No row is selected";
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dataGridView1.SelectedRows[0];
                int id = int.Parse(r.Cells[0].Value.ToString());
                DialogResult rs = MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo);
                if (rs == DialogResult.Yes)
                {
                    deleteData(id);
                    loadData();
                    lbStatus.Text = "Data deleted";
                } else
                {
                    lbStatus.Text = "Data is not deleted";
                }
            }
            else
            {
                MessageBox.Show("No row is selected!");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadData();
            lbStatus.Text = "Data refreshed";
        }
    }
}
