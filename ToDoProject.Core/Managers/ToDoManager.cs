using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ToDoManager : IToDoManager
    {
        private readonly ToDoContext _db;
        private readonly IMapper _mapper;

        #region ctor

        public ToDoManager(ToDoContext db, IMapper mapper)
        { 
            _db = db;
            _mapper = mapper;
        }

        #endregion ctor

        #region public
        public void ArchivedToDo(UserModel currentUser, int id)
        {
            if (!currentUser.IsAdmin)
            {
                throw new ServiceValidationException("You don't have Permission to Archive todo !");
            }
            var data = _db.ToDos
                               .FirstOrDefault(a => a.Id == id && !a.Archived)
                               ?? throw new ServiceValidationException("Invalid toDo id received");
           
            data.Archived = true;
            _db.SaveChanges();
        }

        public ToDoViewModel GetToDo(int id)
        {
            var res = _db.ToDos.Include("User")
                               .FirstOrDefault(a => a.Id == id && !a.Archived);
            return _mapper.Map<ToDoViewModel>(res);
        }

        public ToDoResponse GetToDo(int page = 1, int pageSize = 5, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var queryRes = _db.ToDos
                                    .Where(a => !a.IsRead && (string.IsNullOrWhiteSpace(searchText)
                                                   || (a.Title.Contains(searchText)
                                                       || a.Contents.Contains(searchText))));

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var userIds = res.Data
                             .Select(a => a.UserId)
                             .Distinct()
                             .ToList();

            var users = _db.Users
                                     .Where(a => userIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<UserResult>(x));

            var data = new ToDoResponse
            {
                Blog = _mapper.Map<PagedResult<ToDoViewModel>>(res),
                User = users
            };

            data.Blog.Sortable.Add("Title", "Title");
            data.Blog.Sortable.Add("CreatedAt", "Created Date");

            return data;
        }

        public void IsRead(int id)
        {
            var data = _db.ToDos
                                .FirstOrDefault(a => a.Id == id && !a.Archived)
                                ?? throw new ServiceValidationException("Invalid toDo id received");

            data.IsRead = true;
            _db.SaveChanges();
        }

        public ToDoViewModel PutToDo(UserModel currentUser, ToDoRequest toDoRequest)
        {
            ToDo toDo = null;
            if (!currentUser.IsAdmin)
            {
                throw new ServiceValidationException("You don't have permission to add or update ToDo");
            }
            if (toDoRequest.Id > 0)
            {
                toDo = _db.ToDos
                                .FirstOrDefault(a => a.Id == toDoRequest.Id)
                                 ?? throw new ServiceValidationException("Invalid toDo id received");

                toDo.Title = toDoRequest.Title;
                toDo.Contents = toDoRequest.Contents;
            }
            else
            {
                toDo = _db.ToDos.Add(new ToDo
                {
                    Title = toDoRequest.Title,
                    Contents = toDoRequest.Contents,
                    UserId = currentUser.Id
                }).Entity;
            }

            _db.SaveChanges();
            return _mapper.Map<ToDoViewModel>(toDo);
        }

        #endregion public 
    }
}
