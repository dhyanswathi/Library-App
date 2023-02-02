using Library.Mvc.Models;

namespace Library.Mvc.ViewModel
{
    public class UserViewModel
    {
        public User user { get; set; }
        public List<BorrowedBook>? books { get; set; }
    }
}
