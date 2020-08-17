using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CheckNote.Server
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IConfiguration configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) 
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Answer>()
                .Property(a => a.Correct)
                .HasDefaultValue(true);

            //builder.Entity<User>(user =>
            //{
            //    user.HasIndex(u => u.Email).IsUnique();
            //    user.HasIndex(u => u.UserName).IsUnique();
            //});

            builder.Entity<Question>(question =>
            {
                question.Property(q => q.Correct).HasDefaultValue(true);
                question.Property(q => q.Difficulty).HasDefaultValue(QuestionDifficulty.Easy);
            });

            builder.Entity<CourseNote>(courseNote =>
            {
                courseNote.HasOne(cn => cn.Course).WithMany(c => c.Notes).OnDelete(DeleteBehavior.Restrict).IsRequired();
                courseNote.HasOne(cn => cn.Note).WithMany(n => n.Courses).OnDelete(DeleteBehavior.Restrict).IsRequired();
            });
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Course> Courses { get; set; }

    }
}
