using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjCsWebRemax.data
{
    class DataSource
    {
        public static OleDbConnection myCon;
        public static DataSet mySet;
        public static OleDbDataAdapter adpEmmployees;
        public static OleDbDataAdapter adpClients;
        public static OleDbDataAdapter adpHouses;

        internal static void StartData()
        {
            try
            {
                StartConnection();
                
                OleDbCommand mycmd = new OleDbCommand("SELECT * FROM Employees", myCon);

                adpEmmployees = new OleDbDataAdapter(mycmd);

                mySet = new DataSet();
                adpEmmployees.Fill(mySet, "Employees");

                OleDbCommand mycmd2 = new OleDbCommand("SELECT * FROM Clients", myCon);
                adpClients = new OleDbDataAdapter(mycmd2);
                adpClients.Fill(mySet, "Clients");

                OleDbCommand mycmd3 = new OleDbCommand("SELECT * FROM Houses", myCon);
                adpHouses = new OleDbDataAdapter(mycmd3);
                adpHouses.Fill(mySet, "Houses");

                mySet.Relations.Add(new DataRelation("EC", mySet.Tables["Employees"].Columns["RefEmployee"], mySet.Tables["Clients"].Columns["RefAgent"]));
                mySet.Relations.Add(new DataRelation("CH", mySet.Tables["Clients"].Columns["RefClient"], mySet.Tables["Houses"].Columns["RefClient"]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataRow FindHouse(string refhouse)
        {
            string expression = "RefHouse = " + refhouse;
            DataRow[] dataResult = mySet.Tables["Houses"].Select(expression);
            return dataResult.First();
        }

        internal static void UpdateHouses(DataTable myTb)
        {
            OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(adpHouses);
            adpHouses.Update(myTb);
        }

        internal static DataTable HousesByAgent(int refEmployee)
        {
            StartConnection();
            String sql = "SELECT Houses.RefHouse, Houses.Area, Houses.Bathrooms, Houses.Bedrooms, Houses.City, " +
                "Houses.ConstructionYear, Houses.Price, Houses.PropertyType, Houses.Description, Houses.RefClient " +
                "FROM((Clients INNER JOIN Employees ON Clients.RefAgent = Employees.RefEmployee) INNER JOIN Houses " +
                "ON Clients.RefClient = Houses.RefClient) WHERE(Employees.RefEmployee = @refEmployee)";

            OleDbCommand myCmd = new OleDbCommand(sql, myCon);
            myCmd.Parameters.AddWithValue("@refEmployee", refEmployee);
            adpEmmployees = new OleDbDataAdapter(myCmd);

            DataSet mySet = new DataSet();
            adpEmmployees.Fill(mySet);

            return mySet.Tables[0];
        }

        internal static DataTable AllSellers()
        {
            string expression = "ClientType = '" + ClientType.Seller + "'";
            DataRow[] myRows = mySet.Tables["Clients"].Select(expression);
            DataTable myResult = mySet.Tables["Clients"].Clone();
            foreach (DataRow rw in myRows)
            {
                myResult.ImportRow(rw);
            }
            return myResult;
        }

        internal static DataTable HousesOfSellers(int refClient)
        {
            string expression = "RefClient = " + refClient;
            DataRow[] myRows = mySet.Tables["Houses"].Select(expression);
            DataTable myResult = mySet.Tables["Houses"].Clone();
            foreach (DataRow rw in myRows)
            {
                myResult.ImportRow(rw);
            }
            return myResult;
        }

        internal static DataTable AllHouses()
        {
            return mySet.Tables["Houses"];
        }

        internal static DataTable AllAgents()
        {
            string expression = "Role = '" + Role.Agent + "'";
            DataRow[] myRows = mySet.Tables["Employees"].Select(expression);
            DataTable myResult = mySet.Tables["Employees"].Clone();
            foreach (DataRow rw in myRows)
            {
                myResult.ImportRow(rw);
            }
            return myResult;
        }

        internal static string AgentName(int refClient)
        {
            try
            {
                StartConnection();
                String sql = "SELECT Employees.FirstName, Employees.LastName " +
                "FROM(Clients INNER JOIN Employees ON Clients.RefAgent = Employees.RefEmployee) " +
                "WHERE(Clients.RefClient = @refClient)";

                OleDbCommand myCmd = new OleDbCommand(sql, myCon);
                myCmd.Parameters.AddWithValue("@refClient", refClient);
                OleDbDataAdapter adpAgentName = new OleDbDataAdapter(myCmd);

                DataSet mySet = new DataSet();
                adpAgentName.Fill(mySet);

                DataRow myrw = mySet.Tables[0].Rows[0];

                return myrw["FirstName"].ToString() + " " + myrw["LastName"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static DataTable ClientsByAgent(int refEmployee)
        {
            string expression = "RefAgent = " + refEmployee;
            DataRow[] myRows = mySet.Tables["Clients"].Select(expression);
            DataTable myResult = mySet.Tables["Clients"].Clone();
            foreach (DataRow rw in myRows)
            {
                myResult.ImportRow(rw);
            }
            return myResult;
        }

        internal static DataTable AllClients()
        {
            return mySet.Tables["Clients"];
        }

        internal static void UpdateEmployees(DataTable myTb)
        {
            OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(adpEmmployees);
            adpEmmployees.Update(myTb);
        }

        internal static void UpdateClients(DataTable myTb)
        {
            OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(adpClients);
            adpClients.Update(myTb);
        }

        internal static DataTable AllEmployees()
        {
            return mySet.Tables["Employees"];
        }

        internal static DataTable EmployeeByUsername(string Username)
        {
            try
            {
                StartConnection();
                String sql = "SELECT Employees.* FROM Employees WHERE(UserName = @Username)";

                OleDbCommand myCmd = new OleDbCommand(sql, myCon);
                myCmd.Parameters.AddWithValue("@Username", Username);
                adpEmmployees = new OleDbDataAdapter(myCmd);

                DataSet mySet = new DataSet();
                adpEmmployees.Fill(mySet);

                return mySet.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void StartConnection()
        {
            myCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='C:\Users\Juan Carlos\source\WebSites\WebRemax\App_Data\prjRemaxDB.mdb';Persist Security Info=True");
            myCon.Open();
        }
    }
}
