namespace Bookstore
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Authors.Any())
            {
                var author = new Author { Name = "John Doe" };
                context.Authors.Add(author);

                var book = new Book { Title = "Sample Book", Author = author };
                context.Books.Add(book);

                context.SaveChanges();
            }
        }
    }
}
