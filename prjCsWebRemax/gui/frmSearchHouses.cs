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
    public partial class frmSearchHouses : Form
    {
        public frmSearchHouses()
        {
            InitializeComponent();
        }

        private void frmSearchHouses_Load(object sender, EventArgs e)
        {
            HouseList allHouses = new HouseList();

            allHouses.RecoverHouses();
            lstHouses.DataSource = allHouses.Elements;
            lstHouses.DisplayMember = "RefHouse";
            lstHouses.ValueMember = "RefHouse";
        }

        private void lstHouses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                House myHouse = new House();
                myHouse.Find(lstHouses.SelectedValue.ToString());
                HouseToText(myHouse);
            }
            catch
            {

            }
        }

        private void HouseToText(House myHouse)
        {
            lblArea.Text = myHouse.Area.ToString();
            lblBath.Text = myHouse.Bathrooms.ToString();
            lblBed.Text = myHouse.Bedrooms.ToString();
            lblCity.Text = myHouse.City.ToString();
            lblPrice.Text = myHouse.Price.ToString();
            lblType.Text = myHouse.PropertyType.ToString();
            lblYear.Text = myHouse.ConstructionYear.ToString();
            richDescription.Text = myHouse.Description.ToString();
        }
    }
}
