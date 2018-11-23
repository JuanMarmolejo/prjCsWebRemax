using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using prjCsWebRemax.data;

namespace prjCsWebRemax
{
    public class EmployeeList
    {
        private List<Employee> myList;

        public EmployeeList()
        {
            myList = new List<Employee>();
        }

        public int Quantity
        {
            get => myList.Count;
        }

        public List<Employee> Elements
        {
            get => myList;
        }

        internal void RecoverEmployees()
        {
            DataTable tbEmployees;
            Employee myEmp;
            myList = new List<Employee>();

            tbEmployees = DataSource.AllEmployees();
            foreach (DataRow myRow in tbEmployees.Rows)
            {
                myEmp = new Employee();
                myEmp.CopyDataRow(myRow);
                myList.Add(myEmp);
            }
        }

        internal Employee First()
        {
            return myList.First<Employee>();
        }

        internal void Add(Employee newEmp)
        {
            myList.Add(newEmp);
            newEmp.InsertEmployeeDB();
        }

        internal void RecoverAgents()
        {
            DataTable tbAgents;
            Employee myAge;
            myList = new List<Employee>();

            tbAgents = DataSource.AllAgents();
            foreach (DataRow myRow in tbAgents.Rows)
            {
                myAge = new Employee();
                myAge.CopyDataRow(myRow);
                myList.Add(myAge);
            }
        }

        internal void SaveDatabase()
        {
            DataTable myTb = ConvertTo<Employee>(myList);
            DataSource.UpdateEmployees(myTb);
        }

        private DataTable ConvertTo<T>(List<T> myList)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in myList)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }

        internal Employee Last()
        {
            return myList.Last<Employee>();
        }

        internal Employee EmployeeAt(int current)
        {
            try
            {
                return myList.ElementAt<Employee>(current);
            }
            catch (Exception ex)
            {
                ArgumentException Argex = new ArgumentException("No employees were found", ex);
                throw Argex;
            }
                
        }

        internal void Modify(int current, Employee newEmp)
        {
            myList[current] = newEmp;
            newEmp.ModifyEmployeeDB(current);
        }

        internal void DeleteEmployee(int current)
        {
            myList.RemoveAt(current);
            DataTable myTb = DataSource.AllEmployees();
            myTb.Rows[current].Delete();
            DataSource.UpdateEmployees(myTb);
        }
    }
}