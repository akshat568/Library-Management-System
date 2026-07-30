namespace LibraryManagement.Models
{
    public class LibrarianIndexViewModel
    {
        public IEnumerable<Librarian> Librarians { get; set; }
            = new List<Librarian>();

        public string? SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 5;

        public int TotalItems { get; set; }
    }
}