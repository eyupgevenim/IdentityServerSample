using Microsoft.EntityFrameworkCore;

namespace Todo.API.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Todo>(entity => 
                {
                    entity.ToTable(nameof(Todo));
                    entity.HasKey(x => x.Id);
                    entity.Property(x => x.Name).HasMaxLength(100);
                    entity.Property(x => x.Content).HasMaxLength(500);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
