using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConnectionUsingADO
{
    internal class Program
    {
        static String connString=ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
        static void Main(string[] args)
        {
            Console.WriteLine("Please Select the below options\n" +
    "1. Add Student\n" +
    "2. View Students\n"+
    "3. Update Student\n" +
    "4. Delete Student\n" +
    "5. Get Single Student\n"+
    "0. Exit");
            while (true)
            {
                Console.Write("Please Enter a number");
                int opt = Convert.ToInt32(Console.ReadLine());
                if (opt < 0 || opt > 5)
                {
                    continue;
                }
                else
                {
                    if (opt == 0)
                    {
                        break;
                    }
                    else if (opt == 1)
                    {
                        Console.WriteLine("Please Enter Name to add.");
                        string name = Console.ReadLine();
                        CreateStudent(name);
                    }
                    else if (opt == 2)
                    {
                        ReadStudents();
                    }
                    else if (opt == 3)
                    {
                        Console.WriteLine("Please Enter ID to Update");
                        int id = Convert.ToInt32((Console.ReadLine()));
                        Console.WriteLine("Please Enter Name to Update");
                        string name = Console.ReadLine();
                        UpdateStudent(id, name);
                    }
                    else if (opt == 4)
                    {
                        Console.WriteLine("Please Enter ID to be Deleted");
                        int id = Convert.ToInt32((Console.ReadLine()));
                        DeleteStudent(id);
                    }
                    else
                    {
                        Console.WriteLine("Please Enter ID to see detail");
                        int id = Convert.ToInt32((Console.ReadLine()));
                        GetStudentById(id);
                    }
                }
            }
            
        }
        static void CreateStudent(string name)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string query = "INSERT INTO Student (Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Student record inserted.");
                }
            }
        }
            static void ReadStudents()
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    string query = "SELECT Id, Name FROM Student";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}");
                            }
                        }
                    }
                }
            }
            static void UpdateStudent(int id, string name)
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    string query = "UPDATE Student SET Name = @Name WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", name);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} record(s) updated.");
                    }
                }
            }

            static void DeleteStudent(int id)
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    string query = "DELETE FROM Student WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} record(s) deleted.");
                    }
                }
            }

        static void GetStudentById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string procedureName = "GetStudentById";
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}");
                        }
                        else
                        {
                            Console.WriteLine("No student found with the specified Id.");
                        }
                    }
                }
            }
        }


    }
}
