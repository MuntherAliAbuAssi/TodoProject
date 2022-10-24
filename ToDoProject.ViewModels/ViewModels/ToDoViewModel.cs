using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOProject.ViewModel.ViewModels
{ 
    public class ToDoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Archived { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }

    }
}
