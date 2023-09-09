namespace LarsProjekt.Models
{
    public class ChangePasswordModel 
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
    }
}
