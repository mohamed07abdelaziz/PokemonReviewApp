using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IUserRepo
    {

        bool PasswordExist(byte[] Hash);

        bool UserNameExist(string UserName);

        User GetUser(string username);
        bool CreateUser(User user);

        bool changeRefresh(string refresh, DateTime create,DateTime end,string name);
        bool Save();
    }
}




