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
    public partial class frmClients : Form
    {
        int current = 0;
        string mode = "";
        ClientList myListClients;
        public frmClients()
        {
            InitializeComponent();
        }

        private void frmClients_Load(object sender, EventArgs e)
        {
            myListClients = new ClientList();
            EmployeeList myListAgents = new EmployeeList();

            //Add search options
            cboBy.Items.Add("-- Select an option --");
            cboBy.Items.Add("First Name");
            cboBy.Items.Add("Last Name");
            cboBy.Items.Add("Role");
            cboBy.Items.Add("Username");
            cboBy.SelectedIndex = 0;

            //Add the types of clients.
            cboTypeClient.DataSource = Enum.GetValues(typeof(ClientType));

            //Retrieve employees and clients from the database and display them in the form.
            switch (clsGlobals.ActiveUser.Role)
            {
                case Role.Administrator:
                    myListClients.RecoverClients();
                    myListAgents.RecoverAgents();
                    cboAgent.DisplayMember = "FullName";
                    cboAgent.ValueMember = "RefEmployee";
                    cboAgent.DataSource = myListAgents.Elements;
                    break;
                case Role.Agent:
                    myListClients.ClientsByAgent(clsGlobals.ActiveUser.RefEmployee);
                    Dictionary<int, string> tmp = new Dictionary<int, string>();
                    tmp.Add(clsGlobals.ActiveUser.RefEmployee, clsGlobals.ActiveUser.FullName);
                    cboAgent.DataSource = new BindingSource(tmp, null);
                    cboAgent.DisplayMember = "Value";
                    cboAgent.ValueMember = "Key";
                    break;
                case Role.User:
                    
                    break;
            }
            

            //Binding Agents
            
            Employee tp = new Employee();
            
            try
            {
                employeeToText(myListClients.ClientAt(current));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            gridResult.DataSource = myListClients.Elements;

            enableButtons(true, true, true, false, false);
            hideSearchControls();
        }

        private void employeeToText(Client client)
        {
            txtFn.Text = client.FirstName.ToString();
            txtLn.Text = client.LastName.ToString();
            txtTelephone.Text = client.PhoneNumber.ToString();
            txtEmail.Text = client.Email.ToString();
            txtBirthDate.Text = client.BirthDate.ToString("yyyy-MM-dd");
            cboTypeClient.Text = client.ClientType.ToString();
            cboAgent.SelectedValue = client.RefAgent;
            lblInfo.Text = "Client " + (current + 1) + " out of " + clsGlobals.ActiveCompany.Employees.Quantity;
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
            cboAgent.SelectedIndex = -1;
            cboTypeClient.SelectedIndex = -1;
            txtFn.Focus();
            enableButtons(false, false, false, true, true);
            lblInfo.Text = "Entering a new client";
            mode = "Add";
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            txtFn.Focus();
            lblInfo.Text = "Modifying client information";
            enableButtons(false, false, false, true, true);
            mode = "Modify";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            employeeToText(myListClients.ClientAt(current));
            enableButtons(true, true, true, false, false);
            mode = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Client newCli;
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                DateTime myBD = Convert.ToDateTime(txtBirthDate.Text);
                ClientType myClientType = (ClientType)Enum.Parse(typeof(ClientType), cboTypeClient.SelectedItem.ToString());
                int myAgent = Convert.ToInt32(cboAgent.SelectedValue);
                newCli = new Client(txtFn.Text, txtLn.Text, myBD, txtTelephone.Text, txtEmail.Text, myClientType, myAgent);
                if (mode == "Add")
                {
                    myListClients.Add(newCli);
                    current = myListClients.Quantity - 1;
                }
                if (mode == "Modify")
                {
                    myListClients.Modify(current, newCli);
                }
                enableButtons(true, true, true, false, false);
                employeeToText(myListClients.ClientAt(current));
                gridResult.DataSource = null;
                gridResult.DataSource = myListClients.Elements;
                mode = "";
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            current = 0;
            employeeToText(myListClients.ClientAt(current));
            ClearRowsDatagrid();
            gridResult.Rows[current].Selected = true;
        }

        private void ClearRowsDatagrid()
        {
            foreach (DataGridViewRow rw in gridResult.Rows)
            {
                rw.Selected = false;
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (current > 0)
            {
                current--;
                employeeToText(myListClients.ClientAt(current));
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (current < (myListClients.Quantity - 1))
            {
                current++;
                employeeToText(myListClients.ClientAt(current));
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            current = myListClients.Quantity - 1;
            employeeToText(myListClients.ClientAt(current));
            ClearRowsDatagrid();
            gridResult.Rows[current].Selected = true;
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
            employeeToText(myListClients.ClientAt(current));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this client.", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                myListClients.DeleteClient(current);
                current = current > 0 ? current - 1 : 0;
                employeeToText(myListClients.ClientAt(current));
                gridResult.DataSource = null;
                gridResult.DataSource = myListClients.Elements;
                ClearRowsDatagrid();
                gridResult.Rows[current].Selected = true;
            }
        }
    }
}
