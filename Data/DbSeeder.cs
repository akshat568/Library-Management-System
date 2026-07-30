using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            LibraryContext context)
        {
            // =============================================
            // Students
            // =============================================

            if (!await context.Students.AnyAsync())
            {
                var students = new List<Student>
                {
                    new Student
                    {
                        StudentName = "Aarav Sharma",
                        Email = "aarav.sharma@example.com",
                        Phone = "9876543210"
                    },

                    new Student
                    {
                        StudentName = "Priya Verma",
                        Email = "priya.verma@example.com",
                        Phone = "9876543211"
                    },

                    new Student
                    {
                        StudentName = "Rohan Singh",
                        Email = "rohan.singh@example.com",
                        Phone = "9876543212"
                    },

                    new Student
                    {
                        StudentName = "Ananya Gupta",
                        Email = "ananya.gupta@example.com",
                        Phone = "9876543213"
                    },

                    new Student
                    {
                        StudentName = "Rahul Mehta",
                        Email = "rahul.mehta@example.com",
                        Phone = "9876543214"
                    },

                    new Student
                    {
                        StudentName = "Sneha Patel",
                        Email = "sneha.patel@example.com",
                        Phone = "9876543215"
                    }
                };

                await context.Students.AddRangeAsync(students);
            }


            // =============================================
            // Librarians
            // =============================================

            if (!await context.Librarians.AnyAsync())
            {
                var librarians = new List<Librarian>
                {
                    new Librarian
                    {
                        Name = "Rajesh Kumar",
                        Age = 35,
                        Phone = "9123456701"
                    },

                    new Librarian
                    {
                        Name = "Neha Sharma",
                        Age = 29,
                        Phone = "9123456702"
                    },

                    new Librarian
                    {
                        Name = "Amit Verma",
                        Age = 41,
                        Phone = "9123456703"
                    },

                    new Librarian
                    {
                        Name = "Pooja Singh",
                        Age = 32,
                        Phone = "9123456704"
                    },

                    new Librarian
                    {
                        Name = "Vikram Gupta",
                        Age = 38,
                        Phone = "9123456705"
                    },

                    new Librarian
                    {
                        Name = "Meera Joshi",
                        Age = 27,
                        Phone = "9123456706"
                    }
                };

                await context.Librarians.AddRangeAsync(librarians);
            }


            // =============================================
            // Publications
            // =============================================

            if (!await context.Publications.AnyAsync())
            {
                var publications = new List<Publication>
                {
                    // Newspapers

                    new Publication
                    {
                        Title = "The Daily Chronicle",
                        Publisher = "Chronicle Media",
                        PublishedDate = DateTime.Today,
                        Type = PublicationType.Newspaper,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "Morning Herald",
                        Publisher = "Herald Publications",
                        PublishedDate = DateTime.Today.AddDays(-1),
                        Type = PublicationType.Newspaper,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "City Times",
                        Publisher = "City Media Group",
                        PublishedDate = DateTime.Today.AddDays(-2),
                        Type = PublicationType.Newspaper,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "National Express",
                        Publisher = "Express Publications",
                        PublishedDate = DateTime.Today.AddDays(-3),
                        Type = PublicationType.Newspaper,
                        IsAvailable = false
                    },

                    new Publication
                    {
                        Title = "Business Daily",
                        Publisher = "Business Media",
                        PublishedDate = DateTime.Today.AddDays(-4),
                        Type = PublicationType.Newspaper,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "Weekend Journal",
                        Publisher = "Journal Media",
                        PublishedDate = DateTime.Today.AddDays(-5),
                        Type = PublicationType.Newspaper,
                        IsAvailable = true
                    },


                    // Magazines

                    new Publication
                    {
                        Title = "Technology Today",
                        Publisher = "Tech Media",
                        PublishedDate = DateTime.Today,
                        Type = PublicationType.Magazine,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "Science World",
                        Publisher = "Science Publications",
                        PublishedDate = DateTime.Today.AddDays(-5),
                        Type = PublicationType.Magazine,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "Business Insight",
                        Publisher = "Insight Media",
                        PublishedDate = DateTime.Today.AddDays(-10),
                        Type = PublicationType.Magazine,
                        IsAvailable = false
                    },

                    new Publication
                    {
                        Title = "Travel Explorer",
                        Publisher = "Explorer Publications",
                        PublishedDate = DateTime.Today.AddDays(-15),
                        Type = PublicationType.Magazine,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "Modern Living",
                        Publisher = "Lifestyle Media",
                        PublishedDate = DateTime.Today.AddDays(-20),
                        Type = PublicationType.Magazine,
                        IsAvailable = true
                    },

                    new Publication
                    {
                        Title = "Digital Future",
                        Publisher = "Future Media",
                        PublishedDate = DateTime.Today.AddDays(-25),
                        Type = PublicationType.Magazine,
                        IsAvailable = true
                    }
                };

                await context.Publications
                    .AddRangeAsync(publications);
            }


            // =============================================
            // Save everything
            // =============================================

            await context.SaveChangesAsync();
        }
    }
}