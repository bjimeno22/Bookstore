namespace Bookstore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            // Create custom index
            modelBuilder.Entity<Author>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
            new User {UserId=1, Email="testdummy4@ecu.edu", FirstName="Test", LastName="Dummy",RoleId=1, PasswordHash="94AC41D46689F403BB6B927B1289746FD68C652FCEC01B056D92E060F042FE3CFFDB209FCDA9C5C8334A68831EA90BE4DA506DA8BD0D2465214409B7FA87959D", PasswordSalt="7A683670A4029ED01D0BF5044DD8B968B54E7615F32C55F4D65C40B976DEA2D26EBD803CB3AB0486604D130C03EBE7CCC783D2D076D5A421DB68E91DB1F730F0"},
            new User {UserId=2, Email="testdummy5@gmail.com", FirstName="Testing", LastName="Snow", RoleId=2, PasswordHash="8287D5BE343A25AA0B69B9F3F9CF96F1B9F07B0683C02FF2A4A3AC0AF90B83A484A8DE935D01ECD5C60093F443BDB697593625C54D88A0E678B9595FF694C90D", PasswordSalt="0B76C1E84C72197D3DF98199784358DBE9F3A1545D3DD4C485B15521C2D847C8EDD65A49588ADE36C40253E377FA6CA103123BB90E41D5AD79AD1F3EC689B921"}
            );
        }
    }
}
