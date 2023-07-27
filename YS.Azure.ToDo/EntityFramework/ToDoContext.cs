﻿using Microsoft.EntityFrameworkCore;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.EntityFramework
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoEntity> ToDoItems { get; set; } = null!;

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
                .ToTable("ArchivedTasks");
            
            modelBuilder
                .Entity<ToDoEntity>()
                .HasKey(_ => _.Id);
            
            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose()
        {
            base.SaveChanges();
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            base.SaveChangesAsync();
            return base.DisposeAsync();
        }
    }
}