using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace CyberSecurityBot1GUI
{
    public class DatabaseHelper
    {
        private string connectionString = "server=localhost;user=root;password=DalfRebeat65@;database=CyberSecurityDB;";
        private MySqlConnection connection;

        public DatabaseHelper()
        {
            connection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public void AddTask(string title, string description, string reminderDate)
        {
            try
            {
                OpenConnection();

                string query = "INSERT INTO Tasks (Title, Description, ReminderDate, Status) VALUES (@title, @desc, @date, 'Pending')";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@date", reminderDate);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding task: " + ex.Message);
            }
        }
    }
}


