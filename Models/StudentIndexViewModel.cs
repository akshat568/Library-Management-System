namespace LibraryManagement.Models
{
    public class StudentIndexViewModel
    {
        // Search text entered by the user
        public string? SearchTerm { get; set; }

        // Current pagination page
        public int CurrentPage { get; set; } = 1;

        // Total available pages
        public int TotalPages { get; set; }

        // Number of students displayed on each page
        public int PageSize { get; set; } = 5;

        // Total matching students
        public int TotalItems { get; set; }

        // Students displayed on current page
        public IEnumerable<Student> Students { get; set; }
            = new List<Student>();
    }
}