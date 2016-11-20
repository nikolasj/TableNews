using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=intera\sqlexpress;Database=nick_news;Integrated Security=True");
        //}

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    // Customize the ASP.NET Identity model and override the defaults if needed.
        //    // For example, you can rename the ASP.NET Identity table names and more.
        //    // Add your customizations after calling base.OnModelCreating(builder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<PageCategories>(entity =>
            {
                entity.ToTable("pageCategories");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.ToTable("pages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("datetime");

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasColumnName("header")
                    .HasMaxLength(400);

                entity.Property(e => e.Html)
                    .IsRequired()
                    .HasColumnName("html")
                    .HasColumnType("text");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(400);

                entity.Property(e => e.IsTop)
                    .HasColumnName("isTop")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PagCategoryId).HasColumnName("pagCategory_ID");

                entity.HasOne(d => d.PagCategory)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.PagCategoryId)
                    //.OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_pages_pageCategories");
            });
        }

        //public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        //public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<PageCategories> PageCategories { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
    }
}
