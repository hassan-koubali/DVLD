using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class ClsCountryData
    {
        public static bool GetCountrInfoByID(int CountryID, ref string CountryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectionString);

            string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryName = reader["CountryName"].ToString();
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                throw new Exception("An error occurred while retrieving country information.", ex);
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        public static bool GetCountryInfoByName(string CountryName, ref int CountryID)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectionString);

            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryName = reader["CountryID"].ToString();
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                throw new Exception("An error occurred while retrieving country information.", ex);
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }
    
        public static DataTable GetAllCountries()
        {
            DataTable countriesTable = new DataTable();
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectionString); string query = "SELECT * FROM Countries";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    countriesTable.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                throw new Exception("An error occurred while retrieving all countries.", ex);
            }
            finally
            {
                connection.Close();
            }
            return countriesTable;
        }

    }
}
