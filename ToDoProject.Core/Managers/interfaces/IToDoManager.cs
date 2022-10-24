using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOProject.ViewModel.ViewModels;
 
namespace TODOProject.Core.Managers.interfaces
{
    public  interface IToDoManager : IManager
    {
        ToDoResponse GetToDo(int Page = 1, int pageSize = 5, string sortColumn = "", string sortDirection = "ascending", string searchText = "");
        
        ToDoViewModel GetToDo(int id);

        ToDoViewModel PutToDo(UserModel currentUser, ToDoRequest toDoRequest);
        
        void ArchivedToDo(UserModel currentUser, int id);

        void IsRead(int id);

    }
}
