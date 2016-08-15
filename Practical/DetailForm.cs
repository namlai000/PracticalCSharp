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
    public partial class DetailForm : Form
    {
        Computer c;
        ExamEntities1 entity = new ExamEntities1();

        public DetailForm(Computer c)
        {
            InitializeComponent();
            this.c = c;
            dataBind();
        }

        void dataBind()
        {
            lbId.Text = c.ComputerID.ToString().Trim();
            txtManu.Text = c.Manufacturer.Trim();
            txtModel.Text = c.Model.Trim();
            txtType.Text = c.Type.Trim();
            dtpManuDate.Value = DateTime.Parse(c.ManufacturingDate.Trim());
            txtColor.Text = c.Color.Trim();
            txtWeight.Text = c.Weight.ToString().Trim();
            txtPrice.Text = c.Price.ToString().Trim();
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

        void updateData()
        {
            int id = int.Parse(lbId.Text);
            Computer c = entity.Computers.First(x => x.ComputerID == id);
            c.Manufacturer = txtManu.Text;
            c.Model = txtModel.Text;
            c.Type = txtType.Text;
            c.ManufacturingDate = dtpManuDate.Value.ToShortDateString();
            c.Color = txtColor.Text;
            c.Weight = double.Parse(txtWeight.Text);
            c.Price = double.Parse(txtPrice.Text);
            entity.SaveChanges();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!checkValid())
            {
                updateData();
                MessageBox.Show("Data has been updated!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
    }
}
