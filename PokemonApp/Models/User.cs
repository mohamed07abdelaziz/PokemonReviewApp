namespace PokemonReviewApp.Models
{
    public class User
    {
        public string USerName { get; set; }=string.Empty;
        public byte[] HashPassword { get; set; }
        public byte[] SaltPassword { get; set; }

        public string RefreshToken { get; set; } =string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
