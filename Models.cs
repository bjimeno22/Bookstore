namespace Bookstore
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Navigation property for the 1-many relationship
        public List<Book> Books { get; set; }
    }

    public class Book
    {
        public int BookId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        // Foreign key for the 1-many relationship
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }

    public class Genre
    {
        public int GenreId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
