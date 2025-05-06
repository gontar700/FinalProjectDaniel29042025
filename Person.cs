namespace FinalProjectDaniel29042025
{
    public class Person
    {
        private int id;
        
        private string email;

        private string password;

        private string firstName;

        private string lastName;

        private string status;

        private string role;

        private string created_at;

        private string updated_at;
        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Status { get => status; set => status = value; }
        public string Role { get => role; set => role = value; }
        public string Created_at { get => created_at; set => created_at = value; }
        public string Updated_at { get => updated_at; set => updated_at = value; }
        
    }
}
