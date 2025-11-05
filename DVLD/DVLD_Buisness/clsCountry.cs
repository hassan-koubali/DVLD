using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public clsCountry()
        {
            this.CountryID = 0;
            this.CountryName = "";
        }
        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }
        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";
            if (ClsCountryData.GetCountrInfoByID(CountryID, ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            return null;

        }
        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;
            if (ClsCountryData.GetCountryInfoByName(CountryName, ref CountryID))
            {
                return new clsCountry(CountryID, CountryName);
            }
            return null;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = ClsCountryData.GetAllCountries();

            return dt;
        }
    }
}
