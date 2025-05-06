using Dapper;
using MySql.Data.MySqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;


namespace FinalProjectDaniel29042025
{
    public class DBService
    {
        private readonly string connectionString;

        public DBService(string connectionString)
        {
            this.connectionString = connectionString;

            //var connection = new MySqlConnection(connectionString);

            //connection.Open();
        }

        //Get all data from specific user by email
        public Person GetPersonByEmail(string email)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                Person person = connection.QuerySingleOrDefault<Person>("select * from users as T1 WHERE T1.email=@Email", new { Email = email });

                return person;
            }
        }

        //Get all persons/users
        public List<Person> GetUsers()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var persons = connection.Query<Person>("SELECT * FROM users;").ToList();

                return persons;
            }
        }

        //Get all persons/users
        public Person GetUserById(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                Person person = connection.QuerySingleOrDefault<Person>("SELECT * FROM users WHERE id = @Id", new { Id = id });

                return person;
            }
        }


        //Insert new validated user into validated user table.
        public int InsertNewUser(string email, string password, string firstName, string lastName)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string hashedPass = DBService.GetMd5Hash(password);

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    return connection.QuerySingleOrDefault<int>("INSERT INTO users (email, password, firstName, lastName, status, role, created_at, updated_at)" +
                        "VALUES (@Email,@Password,@FirstName,@LastName,'pending','user',@CreatedAt,@UpdateAt)",
                        new { Email = email, Password = hashedPass, FirstName = firstName, LastName = lastName, CreatedAt = today, UpdateAt = today });
                }
                catch
                {
                    return -1;
                }

            }
        }


        public Boolean UpdateUser(Person selectedUser)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "UPDATE users\r\nSET email = @Email, password = @Password , firstName = @FirstName, lastName = @LastName, status= @Status, role=@Role, created_at= @Created_at ,updated_at=@Updated_at \r\nWHERE id = @ID;";

                DateTime currentDate = DateTime.Now;
                var currentDateFormatted = currentDate.ToString("YYYY-MM-DD");
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@ID", selectedUser.Id);
                    command.Parameters.AddWithValue("@Email", selectedUser.Email);
                    command.Parameters.AddWithValue("@Password", selectedUser.Password);
                    command.Parameters.AddWithValue("@FirstName", selectedUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", selectedUser.LastName);
                    command.Parameters.AddWithValue("@Status", selectedUser.Status);
                    command.Parameters.AddWithValue("@Role", selectedUser.Role);
                    command.Parameters.AddWithValue("@Created_at", currentDate);
                    command.Parameters.AddWithValue("@Updated_at", currentDate);

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check the number of affected rows
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        //Static function that hashes given password string
        public static string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // format as hexadecimal
                }
                return sb.ToString();
            }
        }
    }
}
