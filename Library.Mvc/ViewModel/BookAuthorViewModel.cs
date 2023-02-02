using Library.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Mvc.ViewModel
{
    public class BookAuthorViewModel
    {
        public List<Book>? Books { get; set; }
        public SelectList? Authors { get; set; }
        public string? BookAuthor { get; set; }
        public string? SearchString { get; set; }
    }
}
