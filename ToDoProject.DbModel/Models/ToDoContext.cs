using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TODOProject.DbModel.Models 
{
    public partial class ToDoContext : DbContext
    {
        public ToDoContext()
        {
        }
        public bool IgnoreFilter { get; set; }
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDo> ToDos { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-R4I35TV\\SQLEXPRESS;Database=Todo;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.ToTable("ToDo");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ToDos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserId_ToDoId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(getdate())")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<User>().HasQueryFilter(a => !a.Archived || IgnoreFilter);

            modelBuilder.Entity<ToDo>().HasQueryFilter(a => !a.Archived || IgnoreFilter);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
