using System;
using System.Collections.Generic;

#nullable disable

namespace TODOProject.DbModel.Models  
{
    public partial class User
    {
        public User()
        {
            ToDos = new HashSet<ToDo>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool Archived { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
