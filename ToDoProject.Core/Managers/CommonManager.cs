using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOProject.Extensions;
using TODOProject.Core.Managers.interfaces;
using TODOProject.DbModel.Models;
using TODOProject.ViewModel.ViewModels;

namespace TODOProject.Core.Managers 
{
    public class CommonManager : ICommonManager
    {
        private readonly ToDoContext _db;
        private readonly IMapper _mapper;

        #region ctor

        public CommonManager(ToDoContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        #endregion ctor

        #region public

        public UserModel GetUserRole(UserModel user)
        {
            var dbUser = _db.Users
                                 .FirstOrDefault(a => a.Id == user.Id)
                                 ?? throw new ServiceValidationException("Invalid user id received");
            return _mapper.Map<UserModel>(dbUser);
        }

        #endregion public
    }
}
