using Microsoft.EntityFrameworkCore;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.EntityFramework
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoEntity> ToDoItems { get; set; } = null!;

        public DbSet<TaskFilesEntity> TaskFiles { get; set; } = null!;

        public ToDoContext()
        {
            
        }

        public ToDoContext(DbContextOptions<ToDoContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ToDoEntity>()
                .ToTable("ArchivedTasks")
                .HasKey(_ => _.Id);

            modelBuilder
                .Entity<TaskFilesEntity>()
                .ToTable("TaskFiles")
                .HasKey(_ => _.Id);

            modelBuilder
                .Entity<ToDoEntity>()
                .HasMany(_ => _.Files)
                .WithOne(_ => _.Task)
                .HasForeignKey(_ => _.TaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}