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
    public partial class frmSearchClient : Form
    {
        int current = 0;
        ClientList lstClients;
        public frmSearchClient()
        {
            InitializeComponent();
        }

        private void frmSearchClient_Load(object sender, EventArgs e)
        {
            lstClients = new ClientList();
            lstClients.RecoverClients();
            
            listBox.DataSource = lstClients.Elements;
            listBox.DisplayMember = "FullName";
            listBox.ValueMember = "RefClient";
            displayInfoClient(lstClients.ClientAt(current));
        }

        private void displayInfoClient(Client client)
        {
            lblBirthDate.Text = client.BirthDate.ToString("yyyy-MM-dd");
            lblEmail.Text = client.Email.ToString();
            lblName.Text = client.FullName.ToString();
            lblPhone.Text = client.PhoneNumber.ToString();
            lblType.Text = client.ClientType.ToString();
            lblAgent.Text = client.AgentName();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            current = listBox.SelectedIndex;
            Client myCli = lstClients.ClientAt(current);
            displayInfoClient(myCli);
            if (myCli.ClientType == ClientType.Seller)
            {
                myCli.LoadHouses();
                gridResultat.DataSource = myCli.Houses.Elements;
            }
            else
            {
                gridResultat.DataSource = null;
            }
        }
    }
}
