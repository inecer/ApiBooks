using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiBooks.Entities;

public partial class ApibooksContext : DbContext
{
    public ApibooksContext()
    {
    }

    public ApibooksContext(DbContextOptions<ApibooksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=;database=Apibooks");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Books", "Apibooks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.Available)
                .HasDefaultValueSql("'1'")
                .HasColumnName("available");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Reservations", "Apibooks");

            entity.HasIndex(e => e.BookId, "fk_book");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ReservationDate)
                .HasColumnType("date")
                .HasColumnName("reservation_date");
            entity.Property(e => e.ReturnDate)
                .HasColumnType("date")
                .HasColumnName("return_date");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
