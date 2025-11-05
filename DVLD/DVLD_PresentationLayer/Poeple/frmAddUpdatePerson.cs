using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Poeple
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;
        public enum enMode {AddNew = 0, Update = 1};
        public enum enGendor { Male = 0, Female = 1};

        enMode _Mode;
        private int _PersonID = -1;
        ClsPesrson _Person;


        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _PersonID = PersonID;
        }

        private DataTable _FillCountryInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();
            
            foreach (DataRow dr in dtCountries.Rows)
            {
                cmbNationality.Items.Add(dr["CountryName"].ToString());
            }
            return dtCountries;
        }
        private void _ResetDefualtValues()
        {
            _FillCountryInComboBox();
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new ClsPesrson();
            }
            else
            {
                lblTitle.Text = "Update Person Info";
            }

            llRemove.Visible = (pbImagePerson.ImageLocation!= null);
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Value = dateTimePicker1.MaxDate;
            dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);
            cmbNationality.SelectedIndex = cmbNationality.FindString("Jordan");


            txtFirstName.Text = "";
            txtSecond.Text = "";
            txtThird.Text = "";
            txtLast.Text = "";
            rbMale.Checked = true;
            txtNationalNo.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            if (rbMale.Checked)
                pbImagePerson.Image = Resources.Male_512;
            else
                pbImagePerson.Image = Resources.Female_512;


        }


        private void _LoadData()
        {
            _Person = ClsPesrson.Find(_PersonID);
            if (_Person == null)
            {
                MessageBox.Show("Error in loading person data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            txtFirstName.Text = _Person.FirstName.Trim();
            txtSecond.Text = _Person.SecondName.Trim();
            txtThird.Text = _Person.ThirdName.Trim();
            txtLast.Text = _Person.LastName.Trim();
            txtNationalNo.Text = _Person.NationalID.Trim();
            dateTimePicker1.Value = _Person.BirthDate;
            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
            txtPhone.Text = _Person.Phone.Trim();
            txtEmail.Text = _Person.Email.Trim();
            txtAddress.Text = _Person.Address.Trim();
            cmbNationality.SelectedIndex = cmbNationality.FindString(_Person.CountryInfo.CountryName);
            if (_Person.ImagePath != "")
            {
                pbImagePerson.ImageLocation = _Person.ImagePath;
            }
            llRemove.Visible = (pbImagePerson.ImageLocation != null);

            if (rbMale.Checked)
                pbImagePerson.Image = Resources.Male_512;
            else
                pbImagePerson.Image = Resources.Female_512;







        }



                private void frmAddUpdatePerson_Load_1(object sender, EventArgs e)
                {
                    this.Size = new Size(1129, 580);

                    //WindowState = FormWindowState.Normal;
                    _ResetDefualtValues();
                    if (_Mode == enMode.Update)
                    {
                        _LoadData();
                    }
                }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            pbImagePerson.Image = Resources.Male_512;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            pbImagePerson.Image = Resources.Female_512;
        }
    }
}
