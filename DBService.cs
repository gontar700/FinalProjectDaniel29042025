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

        //Get all data from specific user mothod
        public Person GetPersonByEmail(string email)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                Person person = connection.QuerySingleOrDefault<Person>("select * from users as T1 WHERE T1.email=@Email", new { Email = email });

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
