using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjCsWebRemax.gui
{
    public partial class frmHouses : Form
    {
        HouseList myListHouses;
        int current = 0;
        string mode = "";
        public frmHouses()
        {
            InitializeComponent();
        }

        private void frmHouses_Load(object sender, EventArgs e)
        {
            myListHouses = new HouseList();
            ClientList myListSeller = new ClientList();

            //Add search options
            cboBy.Items.Add("-- Select an option --");
            cboBy.Items.Add("First Name");
            cboBy.Items.Add("Last Name");
            cboBy.Items.Add("Role");
            cboBy.Items.Add("Username");
            cboBy.SelectedIndex = 0;

            //Add the types of clients.
            cboTypeProperty.DataSource = Enum.GetValues(typeof(PropertyType));

            //Retrieve employees and clients from the database and display them in the form.
            switch (clsGlobals.ActiveUser.Role)
            {
                case Role.Administrator:
                    myListHouses.RecoverHouses();
                    myListSeller.RecoverSellers();
                    break;
                case Role.Agent:
                    myListSeller.ClientsByAgent(clsGlobals.ActiveUser.RefEmployee);
                    myListHouses.HousesByAgent(clsGlobals.ActiveUser.RefEmployee);
                    break;
                case Role.User:

                    break;
            }
            
            //Binding Agents
            cboSellerClient.DisplayMember = "FullName";
            cboSellerClient.ValueMember = "RefClient";
            cboSellerClient.DataSource = myListSeller.Elements;

            try
            {
                employeeToText(myListHouses.HouseAt(current));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            gridResult.DataSource = myListHouses.Elements;

            enableButtons(true, true, true, false, false);
            hideSearchControls();
        }

        private void employeeToText(House house)
        {
            cboSellerClient.SelectedValue = house.RefClient;
            txtCity.Text = house.City.ToString();
            cboTypeProperty.Text = house.PropertyType.ToString();
            txtBedrooms.Text = house.Bedrooms.ToString();
            txtBathrooms.Text = house.Bathrooms.ToString();
            txtConstructiionYear.Text = house.ConstructionYear.ToString();
            txtArea.Text = house.Area.ToString();
            txtPrice.Text = house.Price.ToString();
            txtDescription.Text = house.Description.ToString();
            lblInfo.Text = "House " + (current + 1) + " out of " + myListHouses.Quantity;
        }

        private void enableButtons(bool Add, bool Modify, bool Remove, bool Save, bool Cancel)
        {
            btnAdd.Enabled = Add;
            btnModify.Enabled = Modify;
            btnRemove.Enabled = Remove;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }

        private void hideSearchControls()
        {
            txtSearch.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            cboBy.Visible = false;
            btnSearch.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grpBox.Controls)
            {
                if (ctr is TextBox)
                {
                    ctr.Text = "";
                }
            }
            cboSellerClient.SelectedIndex = -1;
            cboTypeProperty.SelectedIndex = -1;
            cboSellerClient.Focus();
            enableButtons(false, false, false, true, true);
            pictureButtons(false);
            lblInfo.Text = "Entering a new house";
            mode = "Add";
        }

        private void pictureButtons(bool v)
        {
            btnFacade.Enabled = v;
            btnImg1.Enabled = v;
            btnImg2.Enabled = v;
            btnImg3.Enabled = v;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            try
            {
                employeeToText(myListHouses.HouseAt(current));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                enableButtons(true, true, true, false, false);
                pictureButtons(true);
                mode = "";
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            cboSellerClient.Focus();
            lblInfo.Text = "Modifying house information";
            enableButtons(false, false, false, true, true);
            pictureButtons(false);
            mode = "Modify";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this house.", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                myListHouses.DeleteHouse(current);
                current = current > 0 ? current - 1 : 0;
                employeeToText(myListHouses.HouseAt(current));
                gridResult.DataSource = null;
                gridResult.DataSource = myListHouses.Elements;
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void ClearRowsDatagrid()
        {
            foreach (DataGridViewRow rw in gridResult.Rows)
            {
                rw.Selected = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                House newHou;
                if (ValidateChildren(ValidationConstraints.Enabled))
                {
                    PropertyType myPropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), cboTypeProperty.SelectedItem.ToString());
                    int mySeller = Convert.ToInt32(cboSellerClient.SelectedValue);
                    int myBathRoom = Convert.ToInt32(txtBathrooms.Text);
                    int myBedroom = Convert.ToInt32(txtBedrooms.Text);
                    int myYear = Convert.ToInt32(txtConstructiionYear.Text);
                    double myArea = Convert.ToDouble(txtArea.Text);
                    double myPrice = Convert.ToDouble(txtPrice.Text);

                    newHou = new House(txtCity.Text, myPropertyType, myBedroom, myBathRoom, myYear, myArea, myPrice, txtDescription.Text, mySeller);
                    if (mode == "Add")
                    {
                        myListHouses.Add(newHou);
                        current = myListHouses.Quantity - 1;
                    }
                    if (mode == "Modify")
                    {
                        myListHouses.Modify(current, newHou);
                    }
                    enableButtons(true, true, true, false, false);
                    pictureButtons(true);
                    employeeToText(myListHouses.HouseAt(current));
                    gridResult.DataSource = null;
                    gridResult.DataSource = myListHouses.Elements;
                    mode = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        //private House TextToddHouse()
        //{
        //    House Hou;

        //    PropertyType myPropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), cboTypeProperty.SelectedItem.ToString());
        //    int mySeller = Convert.ToInt32(cboSellerClient.SelectedValue);
        //    int myBathRoom = Convert.ToInt32(txtBathrooms.Text);
        //    int myBedroom = Convert.ToInt32(txtBedrooms.Text);
        //    int myYear = Convert.ToInt32(txtConstructiionYear.Text);
        //    double myArea = Convert.ToDouble(txtArea.Text);
        //    double myPrice = Convert.ToDouble(txtPrice.Text);

        //    Hou = new House(txtCity.Text, myPropertyType, myBedroom, myBathRoom, myYear, myArea, myPrice, txtDescription.Text, mySeller);

        //    return Hou;
        //}

        private void btnFirst_Click(object sender, EventArgs e)
        {
            current = 0;
            employeeToText(myListHouses.HouseAt(current));
            ClearRowsDatagrid();
            gridResult.Rows[current].Selected = true;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (current > 0)
            {
                current--;
                employeeToText(myListHouses.HouseAt(current));
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (current < (myListHouses.Quantity - 1))
            {
                current++;
                employeeToText(myListHouses.HouseAt(current));
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            current = myListHouses.Quantity - 1;
            employeeToText(myListHouses.HouseAt(current));
            ClearRowsDatagrid();
            gridResult.Rows[current].Selected = true;
        }

        private void btnFacade_Click(object sender, EventArgs e)
        {
            try
            {
                House newHou = new House();

                dlgOpen.Filter = "Image files | *.jpg; *.jpeg; *.png";
                dlgOpen.Title = "Open";
                dlgOpen.ShowDialog();

                string fileName = dlgOpen.SafeFileName;
                string sourceFile = dlgOpen.FileName;
                string targetPath = @"C:\Users\Juan Carlos\source\WebSites\WebRemax\images";
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(sourceFile, destFile, true);

                newHou.Facade = fileName;
                newHou.UpdateFacade(current);
                MessageBox.Show("The image has been created successfully", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImg1_Click(object sender, EventArgs e)
        {
            try
            {
                House newHou = new House();

                dlgOpen.Filter = "Image files | *.jpg; *.jpeg; *.png";
                dlgOpen.Title = "Open";
                dlgOpen.ShowDialog();

                string fileName = dlgOpen.SafeFileName;
                string sourceFile = dlgOpen.FileName;
                string targetPath = @"C:\Users\Juan Carlos\source\WebSites\WebRemax\images";
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(sourceFile, destFile, true);

                newHou.Image1 = fileName;
                newHou.UpdateImage1(current);
                MessageBox.Show("The image has been created successfully", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImg2_Click(object sender, EventArgs e)
        {
            try
            {
                House newHou = new House();

                dlgOpen.Filter = "Image files | *.jpg; *.jpeg; *.png";
                dlgOpen.Title = "Open";
                dlgOpen.ShowDialog();

                string fileName = dlgOpen.SafeFileName;
                string sourceFile = dlgOpen.FileName;
                string targetPath = @"C:\Users\Juan Carlos\source\WebSites\WebRemax\images";
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(sourceFile, destFile, true);

                newHou.Image2 = fileName;
                newHou.UpdateImage2(current);
                MessageBox.Show("The image has been created successfully", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImg3_Click(object sender, EventArgs e)
        {
            try
            {
                House newHou = new House();

                dlgOpen.Filter = "Image files | *.jpg; *.jpeg; *.png";
                dlgOpen.Title = "Open";
                dlgOpen.ShowDialog();

                string fileName = dlgOpen.SafeFileName;
                string sourceFile = dlgOpen.FileName;
                string targetPath = @"C:\Users\Juan Carlos\source\WebSites\WebRemax\images";
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(sourceFile, destFile, true);

                newHou.Image3 = fileName;
                newHou.UpdateImage3(current);
                MessageBox.Show("The image has been created successfully", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
