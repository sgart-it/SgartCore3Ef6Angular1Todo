using Microsoft.EntityFrameworkCore;
using SgartCore3Ef6Angular1Todo.Models;

namespace SgartCore3Ef6Angular1Todo.ServerApp
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<MyTask> MyTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TaskItem>()
            //      .HasOne(s => s.Category);
            //modelBuilder.Entity<TaskItem>()
            //.HasOne(e => e.Category);

            // abilito l'inserimento di una identity
            modelBuilder.Entity<MyTask>().Property(e => e.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(e => e.ID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Undefined", Color = "#BCBCBC" },
                new Category { ID = 2, Name = "Red", Color = "#EB8D90" },
                new Category { ID = 3, Name = "Green", Color = "#A6DD9E" },
                new Category { ID = 4, Name = "Blue", Color = "#97B3E4" },
                new Category { ID = 5, Name = "Yellow", Color = "#FFFA87" },
                new Category { ID = 6, Name = "Purple", Color = "#B09CDD" },
                new Category { ID = 7, Name = "Orange", Color = "#F6B280" }
              );

            //System.DateTime dt = System.DateTime.Now;
            //modelBuilder.Entity<MyTask>(b =>
            //{
            //  b.HasData(new MyTask
            //  {
            //    ID = 1,
            //    Date = dt.Date,
            //    Title = "Testa todo list",
            //    Note = null,
            //    Completed = null,
            //    Modified = dt,
            //    Created = dt
            //  });

            //  b.OwnsOne(e => e.Category).HasData(new { MyTaskID = 1, Name = "Orange", Color = "#F6B280" });
            //});
        }
    }
}
