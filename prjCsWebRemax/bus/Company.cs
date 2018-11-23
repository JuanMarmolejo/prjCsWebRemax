﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using prjCsWebRemax.data;

namespace prjCsWebRemax
{
    public class Company
    {
        private string vNom;
        private EmployeeList vEmployees;

        public Company(string vNom)
        {
            this.Nom = vNom;
            DataSource.StartData();
            vEmployees = new EmployeeList();
        }

        public string Nom { get => vNom; set => vNom = value; }
        public EmployeeList Employees { get => vEmployees; set => vEmployees = value; }
    }
}