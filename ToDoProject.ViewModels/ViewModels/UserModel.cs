using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOProject.ViewModel.ViewModels
{ 
    public class UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

        public string ImageString { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}
