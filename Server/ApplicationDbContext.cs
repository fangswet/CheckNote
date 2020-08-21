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
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Answer>()
                .Property(a => a.Correct)
                .HasDefaultValue(true);

            builder.Entity<Note>()
                .HasOne(n => n.Parent).WithMany(n => n.Children).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>(user =>
            {
                user.HasIndex(u => u.Email).IsUnique();
                user.HasIndex(u => u.UserName).IsUnique();
            });

            builder.Entity<Question>(question =>
            {
                question.Property(q => q.Correct).HasDefaultValue(true);
                question.Property(q => q.Difficulty).HasDefaultValue(QuestionDifficulty.Easy);
            });

            builder.Entity<CourseNote>(courseNote =>
            {
                courseNote.HasOne(cn => cn.Course).WithMany(c => c.CourseNotes).OnDelete(DeleteBehavior.Restrict).IsRequired();
                courseNote.HasOne(cn => cn.Note).WithMany(n => n.CourseNotes).OnDelete(DeleteBehavior.Restrict).IsRequired();
            });

            builder.Entity<CourseLike>(courseLike =>
            {
                courseLike.HasOne(cl => cl.Course).WithMany(c => c.Likes).OnDelete(DeleteBehavior.Restrict);
                courseLike.HasOne(cl => cl.User).WithMany(u => u.Likes).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<TestResult>(testResult =>
            {
                testResult.Property(tr => tr.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
                testResult.HasOne(tr => tr.Course).WithMany(c => c.TestResults).OnDelete(DeleteBehavior.Restrict);
                testResult.HasOne(tr => tr.User).WithMany(u => u.TestResults).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
    }
}
