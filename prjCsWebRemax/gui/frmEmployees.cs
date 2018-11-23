using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjCsWebRemax.gui
{
    public partial class frmEmployees : Form
    {
        int current = 0;
        string mode = "";
        public frmEmployees()
        {
            InitializeComponent();
        }

        private void frmEmployees_Load(object sender, EventArgs e)
        {
            //Add search options
            cboBy.Items.Add("-- Select an option --");
            cboBy.Items.Add("First Name");
            cboBy.Items.Add("Last Name");
            cboBy.Items.Add("Role");
            cboBy.Items.Add("Username");
            cboBy.SelectedIndex = 0;

            //Add the roles of employees.
            cboRole.DataSource = Enum.GetValues(typeof(Role));

            //Add Sex option
            cboSex.Items.Add("-- Gender --");
            cboSex.Items.Add("Male");
            cboSex.Items.Add("Female");

            //Retrieve employees from the database and display them in the form.
            clsGlobals.ActiveCompany.Employees.RecoverEmployees();
            employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
            gridResult.DataSource = clsGlobals.ActiveCompany.Employees.Elements;

            enableButtons(true, true, true, false, false);
            hideSearchControls();
        }

        private void hideSearchControls()
        {
            txtSearch.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            cboBy.Visible = false;
            btnSearch.Visible = false;
        }

        private void enableButtons(bool Add, bool Modify, bool Remove, bool Save, bool Cancel)
        {
            btnAdd.Enabled = Add;
            btnModify.Enabled = Modify;
            btnRemove.Enabled = Remove;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }

        private void employeeToText(Employee employee)
        {
            txtFn.Text = employee.FirstName.ToString();
            txtLn.Text = employee.LastName.ToString();
            txtUsername.Text = employee.Username.ToString();
            txtTelephone.Text = employee.PhoneNumber.ToString();
            txtEmail.Text = employee.Email.ToString();
            txtBirthDate.Text = employee.BirthDate.ToString("yyyy-MM-dd");
            txtPassword.Text = employee.UserPassword.ToString();
            cboRole.Text = employee.Role.ToString();
            cboSex.Text = employee.Sex.ToString();
            lblInfo.Text = "Employee " + (current + 1) + " out of " + clsGlobals.ActiveCompany.Employees.Quantity;
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
            cboRole.SelectedIndex = -1;
            cboSex.SelectedIndex = 0;
            txtFn.Focus();
            enableButtons(false, false, false, true, true);
            lblInfo.Text = "Entering a new employee";
            mode = "Add";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Employee newEmp;
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                DateTime myBD = Convert.ToDateTime(txtBirthDate.Text);
                Role myRole = (Role)Enum.Parse(typeof(Role), cboRole.SelectedItem.ToString());
                newEmp = new Employee(txtFn.Text, txtLn.Text, myBD, txtTelephone.Text, txtEmail.Text, txtUsername.Text, txtPassword.Text, myRole, cboSex.Text);
                if (mode == "Add")
                {
                    clsGlobals.ActiveCompany.Employees.Add(newEmp);
                    current = clsGlobals.ActiveCompany.Employees.Quantity - 1;
                }
                if (mode == "Modify")
                {
                    clsGlobals.ActiveCompany.Employees.Modify(current, newEmp);
                }
                enableButtons(true, true, true, false, false);
                employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
                gridResult.DataSource = null;
                gridResult.DataSource = clsGlobals.ActiveCompany.Employees.Elements;
                mode = "";
            }
        }

        private void txtLn_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLn.Text))
            {
                e.Cancel = true;
                txtLn.Focus();
                errorProvider.SetError(txtLn, "Enter last name");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtLn, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            Regex regEx;
            string txtValidate = txtEmail.Text;
            string regexTest = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$";
            regEx = new Regex(regexTest);
            if (!regEx.IsMatch(txtValidate))
            {
                e.Cancel = true;
                txtEmail.Focus();
                errorProvider.SetError(txtEmail, "Invalid email");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtEmail, null);
            }
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                e.Cancel = true;
                txtUsername.Focus();
                errorProvider.SetError(txtUsername, "Enter Username");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtUsername, null);
            }
        }

        private void txtTelephone_Validating(object sender, CancelEventArgs e)
        {
            Regex regEx;
            string txtValidate = txtTelephone.Text;
            string regexTest = @"^\(?(?:\d{3})\)?[- .]?\d\d\d[- .]?\d\d\d\d$";
            regEx = new Regex(regexTest);
            if (!regEx.IsMatch(txtValidate))
            {
                e.Cancel = true;
                txtTelephone.Focus();
                errorProvider.SetError(txtTelephone, "Enter the phone number in the format XXX XXX XXXX");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtTelephone, null);
            }
        }

        private void txtBirthDate_Validating(object sender, CancelEventArgs e)
        {
            Regex regEx;
            string txtValidate = txtBirthDate.Text;
            string regexTest = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))";
            regEx = new Regex(regexTest);
            if (!regEx.IsMatch(txtValidate))
            {
                e.Cancel = true;
                txtBirthDate.Focus();
                errorProvider.SetError(txtBirthDate, "Enter the Birthdate in the format yyyy-mm-dd");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtBirthDate, null);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider.SetError(txtPassword, "Enter password");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtPassword, null);
            }
        }

        private void cboRole_Validating(object sender, CancelEventArgs e)
        {
            if (cboRole.SelectedIndex < 0)
            {
                e.Cancel = true;
                cboRole.Focus();
                errorProvider.SetError(cboRole, "A role must be selected");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(cboRole, null);
            }
        }

        private void cboSex_Validating(object sender, CancelEventArgs e)
        {
            if (cboSex.SelectedIndex <= 0)
            {
                e.Cancel = true;
                cboSex.Focus();
                errorProvider.SetError(cboSex, "A gender must be selected");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(cboSex, null);
            }
        }

        private void txtFn_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFn.Text))
            {
                e.Cancel = true;
                txtFn.Focus();
                errorProvider.SetError(txtFn, "Enter first name");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtFn, null);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
            enableButtons(true, true, true, false, false);
            mode = "";
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            current = 0;
            employeeToText(clsGlobals.ActiveCompany.Employees.First());
            ClearRowsDatagrid();
            gridResult.Rows[current].Selected = true;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            current = clsGlobals.ActiveCompany.Employees.Quantity - 1;
            employeeToText(clsGlobals.ActiveCompany.Employees.Last());
            ClearRowsDatagrid();
            gridResult.Rows[current].Selected = true;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (current > 0)
            {
                current--;
                employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (current < (clsGlobals.ActiveCompany.Employees.Quantity - 1))
            {
                current++;
                employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
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

        private void gridResult_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rw in gridResult.Rows)
            {
                if (rw.Selected == true)
                {
                    current = gridResult.Rows.IndexOf(rw);
                }
            }
            employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            txtFn.Focus();
            lblInfo.Text = "Modifying employee information";
            enableButtons(false, false, false, true, true);
            mode = "Modify";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this employee.", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                clsGlobals.ActiveCompany.Employees.DeleteEmployee(current);
                current = current > 0 ? current - 1 : 0;
                employeeToText(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
                gridResult.DataSource = null;
                gridResult.DataSource = clsGlobals.ActiveCompany.Employees.Elements;
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Employee newEmp = new Employee();

                dlgOpen.Filter = "Image files | *.jpg; *.jpeg; *.png";
                dlgOpen.Title = "Open";
                dlgOpen.ShowDialog();

                string fileName = dlgOpen.SafeFileName;
                string sourceFile = dlgOpen.FileName;
                string targetPath = @"C:\Users\Juan Carlos\source\WebSites\WebRemax\images";
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(sourceFile, destFile, true);

                newEmp.Photo = fileName;
                newEmp.UpdatePhoto(current);
                MessageBox.Show("The image has been created successfully", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
