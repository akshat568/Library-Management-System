namespace LibraryManagement.Models
{
    public class PublicationIndexViewModel
    {
        public IEnumerable<Publication> Publications { get; set; }
            = new List<Publication>();

        public PublicationType Type { get; set; }

        public string? SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 5;

        public int TotalItems { get; set; }
    }
}