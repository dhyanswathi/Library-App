namespace Library.Mvc.ViewModel
{
    public class BorrowViewModel
    {
        public int BorrowId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public DateTime? ReturnDate { get; set;}

        public int? Penalty { get; set; }

        public string BookTitle { get; set; }
        public string UserName { get; set; }
    }
}
