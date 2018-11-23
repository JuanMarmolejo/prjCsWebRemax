using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using prjCsWebRemax.data;

namespace prjCsWebRemax
{
    public class House
    {
        private int vRefHouse;
        private String vCity;
        private PropertyType vPropertyType;
        private int vBedrooms;
        private int vBathrooms;
        private int vConstructionYear;
        private Double vArea;
        private Double vPrice;
        private String vDescription;
        private int vRefClient;
        private String vFacade;
        private String vImage1;
        private String vImage2;
        private String vImage3;

        public House()
        {
        }

        public House(string vCity, PropertyType vPropertyType, int vBedrooms, int vBathrooms, int vConstructionYear, double vArea, double vPrice, string vDescription, int vRefClient)
        {
            this.vCity = vCity;
            this.vPropertyType = vPropertyType;
            this.vBedrooms = vBedrooms;
            this.vBathrooms = vBathrooms;
            this.vConstructionYear = vConstructionYear;
            this.vArea = vArea;
            this.vPrice = vPrice;
            this.vDescription = vDescription;
            this.vRefClient = vRefClient;
        }

        public int RefHouse { get => vRefHouse; set => vRefHouse = value; }
        public string City { get => vCity; set => vCity = value; }
        public PropertyType PropertyType { get => vPropertyType; set => vPropertyType = value; }
        public int Bedrooms { get => vBedrooms; set => vBedrooms = value; }
        public int Bathrooms { get => vBathrooms; set => vBathrooms = value; }
        public int ConstructionYear { get => vConstructionYear; set => vConstructionYear = value; }
        public Double Area { get => vArea; set => vArea = value; }
        public Double Price { get => vPrice; set => vPrice = value; }
        public string Description { get => vDescription; set => vDescription = value; }
        public int RefClient { get => vRefClient; set => vRefClient = value; }
        public string Facade { get => vFacade; set => vFacade = value; }
        public string Image1 { get => vImage1; set => vImage1 = value; }
        public string Image2 { get => vImage2; set => vImage2 = value; }
        public string Image3 { get => vImage3; set => vImage3 = value; }

        public void Find(string refhouse)
        {
            DataRow myRow = DataSource.FindHouse(refhouse); ;
            CopyDataRow(myRow);
        }
        
        internal void InsertHouseDB()
        {
            DataTable myTb = DataSource.AllHouses();
            DataRow myRow = null;
            myRow = myTb.NewRow();

            //myRow["RefHouse"] = RefHouse;
            myRow["City"] = City;
            myRow["PropertyType"] = PropertyType;
            myRow["Bedrooms"] = Bedrooms;
            myRow["Bathrooms"] = Bathrooms;
            myRow["ConstructionYear"] = ConstructionYear;
            myRow["Area"] = Area;
            myRow["Price"] = Price;
            myRow["Description"] = Description;
            myRow["RefClient"] = RefClient;

            myTb.Rows.Add(myRow);

            DataSource.UpdateHouses(myTb);
        }

        internal void ModifyHouseDB(int current)
        {
            DataTable myTb = DataSource.AllHouses();
            DataRow myRow = null;
            myRow = myTb.Rows[current];

            myRow["City"] = City;
            myRow["PropertyType"] = PropertyType;
            myRow["Bedrooms"] = Bedrooms;
            myRow["Bathrooms"] = Bathrooms;
            myRow["ConstructionYear"] = ConstructionYear;
            myRow["Area"] = Area;
            myRow["Price"] = Price;
            myRow["Description"] = Description;
            myRow["RefClient"] = RefClient;
            myRow["Facade"] = Facade;
            myRow["Image1"] = Image1;
            myRow["Image2"] = Image2;
            myRow["Image3"] = Image3;

            DataSource.UpdateHouses(myTb);
        }
        
        internal void CopyDataRow(DataRow myRow)
        {
            vRefHouse = Convert.ToInt32(myRow["RefHouse"]);
            vCity = myRow["City"].ToString();
            vPropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), myRow["PropertyType"].ToString());
            vBedrooms = Convert.ToInt32(myRow["Bedrooms"]);
            vBathrooms = Convert.ToInt32(myRow["Bathrooms"]);
            vConstructionYear = Convert.ToInt32(myRow["ConstructionYear"]);
            vArea = Convert.ToDouble(myRow["Area"]);
            vPrice = Convert.ToDouble(myRow["Price"]);
            vDescription = myRow["Description"].ToString();
            vRefClient = Convert.ToInt32(myRow["RefClient"]);
        }

        internal void UpdateImage1(int current)
        {
            DataTable myTb = DataSource.AllHouses();
            DataRow myRow = null;
            myRow = myTb.Rows[current];

            myRow["Image1"] = Image1;

            DataSource.UpdateHouses(myTb);
        }

        internal void UpdateFacade(int current)
        {
            DataTable myTb = DataSource.AllHouses();
            DataRow myRow = null;
            myRow = myTb.Rows[current];

            myRow["Facade"] = Facade;

            DataSource.UpdateHouses(myTb);
        }

        internal void UpdateImage2(int current)
        {
            DataTable myTb = DataSource.AllHouses();
            DataRow myRow = null;
            myRow = myTb.Rows[current];

            myRow["Image2"] = Image2;

            DataSource.UpdateHouses(myTb);
        }

        internal void UpdateImage3(int current)
        {
            DataTable myTb = DataSource.AllHouses();
            DataRow myRow = null;
            myRow = myTb.Rows[current];

            myRow["Image3"] = Image3;

            DataSource.UpdateHouses(myTb);
        }
    }
}