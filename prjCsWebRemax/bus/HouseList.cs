using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using prjCsWebRemax.data;

namespace prjCsWebRemax
{
    public class HouseList
    {
        private List<House> myList;

        public HouseList()
        {
            myList = new List<House>();
        }

        public int Quantity
        {
            get => myList.Count;
        }

        public List<House> Elements
        {
            get => myList;
        }

        internal void RecoverHouses()
        {
            DataTable tbHouses;
            House myHou;
            myList = new List<House>();

            tbHouses = DataSource.AllHouses();
            foreach (DataRow myRow in tbHouses.Rows)
            {
                myHou = new House();
                myHou.CopyDataRow(myRow);
                myList.Add(myHou);
            }
        }

        internal House HouseAt(int current)
        {
            try
            {
                return myList.ElementAt<House>(current);
            }
            catch (Exception ex)
            {
                ArgumentException Argex = new ArgumentException("No houses were found.", ex);
                throw Argex;
            }
            
        }

        internal void DeleteHouse(int current)
        {
            myList.RemoveAt(current);
            DataTable myTb = DataSource.AllHouses();
            myTb.Rows[current].Delete();
            DataSource.UpdateHouses(myTb);
        }

        internal void Add(House newHou)
        {
            myList.Add(newHou);
            newHou.InsertHouseDB();
        }

        internal void HousesByAgent(int refEmployee)
        {
            DataTable tbHouses;
            House myHou;
            myList = new List<House>();

            tbHouses = DataSource.HousesByAgent(refEmployee);
            foreach (DataRow myRow in tbHouses.Rows)
            {
                myHou = new House();
                myHou.CopyDataRow(myRow);
                myList.Add(myHou);
            }
        }

        internal void Modify(int current, House newHou)
        {
            myList[current] = newHou;
            newHou.ModifyHouseDB(current);
        }

        internal void HousesOfSellers(int refClient)
        {
            DataTable tbHouses;
            House myHou;
            myList = new List<House>();

            tbHouses = DataSource.HousesOfSellers(refClient);
            foreach (DataRow myRow in tbHouses.Rows)
            {
                myHou = new House();
                myHou.CopyDataRow(myRow);
                myList.Add(myHou);
            }
        }
    }
}