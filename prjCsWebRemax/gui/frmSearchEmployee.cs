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
    public partial class frmSearchEmployee : Form
    {
        int current = 0;
        public frmSearchEmployee()
        {
            InitializeComponent();
        }

        private void frmSearchEmployee_Load(object sender, EventArgs e)
        {
            switch (clsGlobals.ActiveUser.Role)
            {
                case Role.Administrator:
                    clsGlobals.ActiveCompany.Employees.RecoverEmployees();
                    listBox.DataSource = clsGlobals.ActiveCompany.Employees.Elements;
                    break;
                case Role.Agent:
                    clsGlobals.ActiveCompany.Employees.RecoverAgents();
                    listBox.DataSource = clsGlobals.ActiveCompany.Employees.Elements;
                    break;
                case Role.User:
                    clsGlobals.ActiveCompany.Employees.RecoverAgents();
                    listBox.DataSource = clsGlobals.ActiveCompany.Employees.Elements;
                    break;
            }
            try
            {
                displayInfoEmployee(clsGlobals.ActiveCompany.Employees.EmployeeAt(current));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                listBox.DisplayMember = "FullName";
                listBox.ValueMember = "RefEmployee";
            }
        }

        private void displayInfoEmployee(Employee employee)
        {
            lblBirthDate.Text = employee.BirthDate.ToString("yyyy-MM-dd");
            lblEmail.Text = employee.Email.ToString();
            lblName.Text = employee.FullName.ToString();
            lblPhone.Text = employee.PhoneNumber.ToString();
            lblRole.Text = employee.Role.ToString();
            lblSex.Text = employee.Sex.ToString();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            current = listBox.SelectedIndex;
            Employee myEmp = clsGlobals.ActiveCompany.Employees.EmployeeAt(current);
            displayInfoEmployee(myEmp);

            if (clsGlobals.ActiveUser.Role == Role.Administrator)
            {
                if (myEmp.Role == Role.Agent)
                {
                    myEmp.LoadClients();
                    gridResultat.DataSource = myEmp.Clients.Elements;
                }
                else
                {
                    gridResultat.DataSource = null;
                }
            }
        }
    }
}
