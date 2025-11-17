using DVLD.Classes;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enGendor { Male = 0, Female = 1 };

        enMode _Mode;
        private int _PersonID = -1;
        clsPerson _Person;


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
                _Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person Info";
            }

            llRemove.Visible = (pbImagePerson.ImageLocation != null);
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
            _Person = clsPerson.Find(_PersonID);
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
            txtNationalNo.Text = _Person.NationalNo.Trim();
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

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;
            if (!clsValidation.IsValidEmail(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }

        }

        public void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This Field Is Required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This Field Is Required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExists(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is Used With Another Person!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }


        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbImagePerson.ImageLocation = selectedFilePath;
                llRemove.Visible = true;

            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbImagePerson.ImageLocation = null;
            if (rbMale.Checked)
                pbImagePerson.Image = Resources.Male_512;
            else
                pbImagePerson.Image = Resources.Female_512;
            llRemove.Visible = (pbImagePerson.ImageLocation != null);
        }

        private bool _HandlePersonImage()
        {
            // This Proceder Will Handle The Person Image 
            // it Will Tak Care Of Deleting The Old Image From The Folder
            // in Case The Image Is Changed. and it will Rename The New Image With Guid And
            //Place it In the Images Folder

            if (_Person.ImagePath != pbImagePerson.ImageLocation)
            {
                // Image Path Contains The Old Image Path
                if (_Person.ImagePath != "")
                {
                    //First delete the old image file From The Folder if Case Ther Is Any
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {
                        //We Would Not Delete The File
                        // Log it Later
                    }
                    
                }
                if (pbImagePerson.ImageLocation != null)
                {
                    // Then We Copy The New Image Folder After Rename it From clsUtile.cs
                    string SourceImageFile = pbImagePerson.ImageLocation.ToString();
                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbImagePerson.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error in Copying The Image To The Project Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;

                    }
                }

            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (!_HandlePersonImage())
                return;

            int NationalityCountryID = clsCountry.Find(cmbNationality.Text).CountryID;
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecond.Text.Trim();
            _Person.ThirdName = txtThird.Text.Trim();
            _Person.LastName = txtLast.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.BirthDate = dateTimePicker1.Value;
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.NationalityCountryID = NationalityCountryID;
            if (rbMale.Checked)
                _Person.Gendor = (short)enGendor.Male;
            else
                _Person.Gendor = (short)enGendor.Female;

            if (pbImagePerson.ImageLocation != null)
                _Person.ImagePath = pbImagePerson.ImageLocation;
            else
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();     
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person Info";
                MessageBox.Show("Person Info Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Error: Data Is Not Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Test test = new Test();
            test.ShowDialog();
        }

    }
}
