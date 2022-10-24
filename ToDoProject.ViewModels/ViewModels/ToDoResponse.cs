using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOProject.Extensions;
using TODOProject.ViewModel.ViewModel;

namespace TODOProject.ViewModel.ViewModels
{ 
    public class ToDoResponse
    {
        public PagedResult<ToDoViewModel> Blog { get; set; } 

        public Dictionary<int, UserResult> User { get; set; }
    }
}
