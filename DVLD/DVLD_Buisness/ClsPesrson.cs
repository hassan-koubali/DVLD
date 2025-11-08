using DVLD_DataAccess;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace DVLD_Buisness
{
    public class ClsPesrson
    {
        public enum enMode {AddNew = 0, Update = 1};
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string fullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName;
            }
        }
        public string NationalNo { get; set; }
        public DateTime BirthDate { get; set; }
        public short Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public clsCountry CountryInfo;
        private string _ImagePath { get; set; }

        public string ImagePath { get { return _ImagePath; } set { _ImagePath = value; } }

        public ClsPesrson()
        {
            this.PersonID = 0;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.NationalNo = "";
            this.BirthDate = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this._ImagePath = "";
            Mode = enMode.AddNew;
        }
        private ClsPesrson(int PersonID, string FirstName, string SecondName,
                string ThirdName, string LastName, string NationalID, DateTime DateOfBirth,
                short Gendor, string Address,string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalID;
            this.BirthDate = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this._ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;
        }
        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(
                this.FirstName,
                this.SecondName,
                this.ThirdName,
                this.LastName,
                this.NationalNo,
                this.BirthDate,
                this.Gendor,
                this.Address,
                this.Phone,
                this.Email,
                this.NationalityCountryID,
                this._ImagePath
                );
            return ( this.PersonID!= -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(
                this.PersonID,
                this.FirstName,
                this.SecondName,
                this.ThirdName,
                this.LastName,
                this.NationalNo,
                this.BirthDate,
                this.Gendor,
                this.Address,
                this.Phone,
                this.Email,
                this.NationalityCountryID,
                this._ImagePath
                );
        }


        public static ClsPesrson Find(int PersonID)
        {
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string NationalID = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = -1;
            string Address = "";
            string Phone = "";
            string Email = "";
            int NationalityCountryID = -1;
            string ImagePath = "";

            bool found = clsPersonData.GetPersonInfoByID(
                PersonID,
                ref FirstName,
                ref SecondName,
                ref ThirdName,
                ref LastName,
                ref NationalID,
                ref DateOfBirth,
                ref Gendor,
                ref Address,
                ref Phone,
                ref Email,
                ref NationalityCountryID,
                ref ImagePath
                );
            if (found)
            {
                return new ClsPesrson(
                    PersonID,
                    FirstName,
                    SecondName,
                    ThirdName,
                    LastName,
                    NationalID,
                    DateOfBirth,
                    Gendor,
                    Address,
                    Phone,
                    Email,
                    NationalityCountryID,
                    ImagePath
                    );
            }
            else
            {
                return null;
            }
        }

        public static ClsPesrson Find(string NationalNo)
        {
            int PersonID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string NationalID = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int NationalityCountryID = -1;
            string ImagePath = "";

            bool found = clsPersonData.GetPersonInfoByNationalNo(
                NationalNo,
                ref PersonID,
                ref FirstName,
                ref SecondName,
                ref ThirdName,
                ref LastName,
                ref DateOfBirth,
                ref Gendor,
                ref Address,
                ref Phone,
                ref Email,
                ref NationalityCountryID,
                ref ImagePath
                );
            if (found)
                {
                return new ClsPesrson(
                    PersonID,
                    FirstName,
                    SecondName,
                    ThirdName,
                    LastName,
                    NationalID,
                    DateOfBirth,
                    Gendor,
                    Address,
                    Phone,
                    Email,
                    NationalityCountryID,
                    ImagePath
                    );
            }
            else
            {
                return null;
            }

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                case enMode.Update:
                    return _UpdatePerson();

            }
            return false;
        }

        public static DataTable GetAllPersons()
        {
            return clsPersonData.GetAllPersons();
        }

        public static bool DeletePersonByID(int PersonID)
        {
             return clsPersonData.DeletePersonByID(PersonID);
        }

        public static bool IsPersonExists(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }
        public static bool IsPersonExists(string NationalID)
        {
            return clsPersonData.IsPersonExist(NationalID);
        }

    }
}
