﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Library.Mvc.Models
{
    public partial class User
    {
        public User()
        {
            BorrowedBooks = new HashSet<BorrowedBook>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}