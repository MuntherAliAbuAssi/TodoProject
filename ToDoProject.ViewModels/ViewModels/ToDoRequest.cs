using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOProject.ViewModel.ViewModels
{ 
    public class ToDoRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; } 
    }
}
