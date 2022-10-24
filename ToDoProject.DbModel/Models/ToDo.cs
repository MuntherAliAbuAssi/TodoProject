using System;
using System.Collections.Generic;

#nullable disable

namespace TODOProject.DbModel.Models 
{
    public partial class ToDo
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

        public virtual User User { get; set; }
    }
}
