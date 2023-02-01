namespace Library.Mvc.ViewModel
{
    public class BookViewModel
    {
        public string Title { get; set; }

        public int BookId { get; set; }
        public string AuthorName { get; set; }
        public string BorrowStatus { get; set; }

        public DateTime? ExpectedReturn { get; set; }
    }

}
