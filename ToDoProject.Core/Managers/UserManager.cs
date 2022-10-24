using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TODOProject.Extensions;
using TODOProject.Core.Managers.interfaces;
using TODOProject.DbModel.Models;
using TODOProject.ViewModel.Dto;
using TODOProject.ViewModel.ViewModel;
using TODOProject.ViewModel.ViewModels;

namespace TODOProject.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ToDoContext _db;
        private readonly IMapper _mapper;

        #region ctor

        public UserManager(ToDoContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        #endregion ctor 

        #region public

        public UserViewModel Login(LoginDto dto)
        {
           var user = _db.Users.FirstOrDefault(x => x.Email.ToLower().Equals(dto.Email.ToLower()));

            if (user == null || !VerifyHashPassword(dto.Password,user.Password))
            {
                throw new ServiceValidationException(300, "Invalid user email or password received ");
            }

            var res = _mapper.Map<UserViewModel>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";
            return res;
        }

        public UserViewModel SignUp(CreateUserDto dto)
        {
            if (_db.Users.Any(x => x.Email.ToLower().Equals(dto.Email.ToLower())))
            {
                throw new ServiceValidationException("User Already exist");
            }

            var hasehedPassword = HashPassword(dto.Password);

            var user = _db.Users.Add(new User
            {
                FirstName = dto.FirstName,
                LastName  = dto.LastName,
                Email = dto.Email,   
                Password = hasehedPassword,
                ConfirmPassword = hasehedPassword,
                Image = string.Empty
            }).Entity;

            _db.SaveChanges();

            var res = _mapper.Map<UserViewModel>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";

            return res;
        }

        public UserModel UpdateProfile(UserModel currentUser, UserModel request)
        {
            var user = _db.Users
                                .FirstOrDefault(a => a.Id == currentUser.Id)
                                ?? throw new ServiceValidationException("User not Found");
            var url = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.Helper.SaveImage(request.ImageString, "profileImages");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseUrl = "https://localhost:44373/";
                user.Image = @$"{baseUrl}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            _db.SaveChanges();

            return _mapper.Map<UserModel>(user);
        }
        public void Delete(UserModel currentUser, int id)
        {
            if (currentUser.Id == id)
            {
                throw new ServiceValidationException("You have no access to delete Your Self");
            }
            var user = _db.Users
                               .FirstOrDefault(a => a.Id == id)
                               ?? throw new ServiceValidationException("User not Found !");

            user.Archived = true;
            _db.SaveChanges();
        }
         
        #endregion public 

        #region private 

        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        private static bool VerifyHashPassword(string password, string HashedPasword)
        {
            return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
        }

        private string GenerateJWTToken(User user)
        {
            var jwtKey = "#test.key*&^vanthis%$^&*()$%^@#$@!@#%$#^%&*%^*";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim("LastName", user.LastName),
                new Claim("DateOfJoining", user.CreatedAt.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var issuer = "test.com";

            var token = new JwtSecurityToken(
                        issuer,
                        issuer,
                        claims,
                        expires: DateTime.Now.AddDays(20),
                        signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion private  


    }
}
