using AutoMapper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Security.Policy;

namespace PokemonReviewApp.Repository

{
    public class UserRepository :IUserRepo

    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public UserRepository(DataContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public bool PasswordExist(byte[] Hash)
        {
            return _context.Users.Any(U => U.HashPassword == Hash);
        }
        

        public bool UserNameExist(string username)
        {
            return _context.Users.Any(U => U.USerName == username);
        }
        public bool changeRefresh(string refresh, DateTime create, DateTime end,string name)
        {
            _context.Users.Where(e => e.USerName == name).FirstOrDefault().RefreshToken= refresh;
            _context.Users.Where(e => e.USerName == name).FirstOrDefault().TokenExpires = end;
            _context.Users.Where(e => e.USerName == name).FirstOrDefault().TokenCreated = create;

           return Save();
        }
        public User GetUser(string username)
        {
            return _context.Users.Where(e => e.USerName == username).FirstOrDefault();
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();

        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }



    }

}

