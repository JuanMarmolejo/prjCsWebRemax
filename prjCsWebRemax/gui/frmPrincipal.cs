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
    public partial class frmPrincipal : Form
    {
        frmEmployees frEmp;
        frmSearchEmployee frSEmp;
        frmClients frCli;
        frmSearchClient frSCli;
        frmHouses frHou;
        frmSearchHouses frSHou;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            clsGlobals.ActiveCompany = new Company("Regex");
            switch (clsGlobals.ActiveUser.Role)
            {
                case Role.Administrator:
                    EnablePrivileges(true, true, true, true, true, true, true, true);
                    break;
                case Role.Agent:
                    EnablePrivileges(false, true, true, true, false, true, true, true);
                    break;
                case Role.User:
                    EnablePrivileges(false, false, false, true, false, true, false, false);
                    break;
            }
        }

        private void EnablePrivileges(bool Memp, bool Mcli, bool Mhou, bool Semp, bool Scli, bool Shou, bool Sreg, bool Scon)
        {
            employeesToolStripMenuItem.Enabled = Memp;
            clientsToolStripMenuItem.Enabled = Mcli;
            housesToolStripMenuItem.Enabled = Mhou;
            employeesToolStripMenuItem1.Enabled = Semp;
            clientsToolStripMenuItem1.Enabled = Scli;
            housesToolStripMenuItem1.Enabled = Shou;
            registerToolStripMenuItem.Enabled = Sreg;
            consultToolStripMenuItem.Enabled = Scon;
        }

        //The variable is null when the form is closed.
        private void frEmpFormClosed(object sender, FormClosedEventArgs e)
        {
            frEmp = null;
        }
        
        private void frSEFormClosed(object sender, FormClosedEventArgs e)
        {
            frSEmp = null;
        }
        
        private void frCliFormClosed(object sender, FormClosedEventArgs e)
        {
            frCli = null;
        }
        
        private void frSCliFormClosed(object sender, FormClosedEventArgs e)
        {
            frSCli = null;
        }
        
        private void frHouFormClsed(object sender, FormClosedEventArgs e)
        {
            frHou = null;
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Verify that the form is not open.
            if (frEmp == null)
            {
                frEmp = new frmEmployees();
                frEmp.MdiParent = this;
                frEmp.FormClosed += new FormClosedEventHandler(frEmpFormClosed);
                frEmp.Show();
            }
        }

        private void managementToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void employeesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frSEmp == null)
            {
                frSEmp = new frmSearchEmployee();
                frSEmp.MdiParent = this;
                frSEmp.FormClosed += new FormClosedEventHandler(frSEFormClosed);
                frSEmp.Show();
            }
        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frCli == null)
            {
                frCli = new frmClients();
                frCli.MdiParent = this;
                frCli.FormClosed += new FormClosedEventHandler(frCliFormClosed);
                frCli.Show();
            }
        }

        private void clientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frSCli == null)
            {
                frSCli = new frmSearchClient();
                frSCli.MdiParent = this;
                frSCli.FormClosed += new FormClosedEventHandler(frSCliFormClosed);
                frSCli.Show();
            }
        }

        private void housesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frHou == null)
            {
                frHou = new frmHouses();
                frHou.MdiParent = this;
                frHou.FormClosed += new FormClosedEventHandler(frHouFormClsed);
                frHou.Show();
            }
        }

        private void housesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frSHou == null)
            {
                frSHou = new frmSearchHouses();
                frSHou.MdiParent = this;
                frSHou.FormClosed += new FormClosedEventHandler(frSHouFormClosed);
                frSHou.Show();
            }
        }

        private void frSHouFormClosed(object sender, FormClosedEventArgs e)
        {
            frSHou = null;
        }
    }
}
