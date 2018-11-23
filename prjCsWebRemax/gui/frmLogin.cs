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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Employee Emp = new Employee();
                Emp.RecoverUser(txtUsername.Text);
                if (Emp.Login(txtPassword.Text))
                {
                    clsGlobals.ActiveUser = Emp;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Username or password is invalid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Clear();
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Clear();
                txtPassword.Clear();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PasswordChar = '*';
        }
    }
}
