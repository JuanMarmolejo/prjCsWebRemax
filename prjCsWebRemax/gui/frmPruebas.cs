using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using prjCsWebRemax.data;

namespace prjCsWebRemax.gui
{
    public partial class frmPruebas : Form
    {
        public frmPruebas()
        {
            InitializeComponent();
        }

        private void frmPruebas_Load(object sender, EventArgs e)
        {
            clsGlobals.ActiveCompany = new Company("Regex");
            HouseList lstCasas = new HouseList();

            lstCasas.HousesOfSellers(1);
            gridResult.DataSource = lstCasas.Elements;
        }
    }
}
