namespace LibraryManagement.Models
{
    public class BookListViewModel
    {
        // Books displayed on the current page
        public IEnumerable<Book> Books { get; set; }
            = new List<Book>();

        // Current search text
        public string? SearchQuery { get; set; }

        // Current pagination page
        public int CurrentPage { get; set; } = 1;

        // Total number of pages
        public int TotalPages { get; set; }

        // Number of books displayed per page
        public int PageSize { get; set; } = 5;

        // Total number of matching books
        public int TotalItems { get; set; }
    }
}