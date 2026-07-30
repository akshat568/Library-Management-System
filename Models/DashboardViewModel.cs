namespace LibraryManagement.Models
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
        public int TotalBorrowRecords { get; set; }
        public int BooksPublishedThisYear { get; set; }
        public double AvailabilityPercentage { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLibrarians { get; set; }
        public int TotalNewspapers { get; set; }
        public int TotalMagazines { get; set; }

        public List<Book> RecentBooks { get; set; } = new();
        public List<BorrowRecord> ActiveBorrows { get; set; } = new();
        public List<BorrowRecord> RecentActivity { get; set; } = new();
    }
}