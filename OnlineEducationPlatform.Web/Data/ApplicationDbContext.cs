using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Web.Models;
using System.Security.Claims;
using System.Text.Json;

namespace OnlineEducationPlatform.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassSubject> ClassSubjects { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ExamSubmission> ExamSubmissions { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmission { get; set; }
        public DbSet<Book> Books { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ClassSubject>().HasKey(cs => new { cs.ClassId, cs.SubjectId });
            modelBuilder.Entity<AssignmentSubmission>().HasKey(asb => new { asb.AssignmentId, asb.StudentId });

            modelBuilder.Entity<AssignmentSubmission>()
                .Property(p => p.Score)
                .HasPrecision(18,2);
            modelBuilder.Entity<ExamSubmission>()
                .Property(p => p.Score)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Question>()
                .Property(p => p.Points)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasMany(a => a.Submissions)
                       .WithOne(s => s.Assignment)
                       .HasForeignKey(s => s.AssignmentId);
            });
            modelBuilder.Entity<Notification>( entity =>
            {
                entity.Property(n => n.Title).IsRequired(false);
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // Class Relationships
            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.TeacherId).IsRequired(false);
                // Class -> Teacher (One-to-Many)
                entity.HasOne(c => c.Teacher)
                      .WithMany() // Optionally, you can add .WithMany(t => t.Classes) if you want reverse navigation
                      .HasForeignKey(c => c.TeacherId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Class -> Exams (One-to-Many)
                entity.HasMany(c => c.Exams)
                      .WithOne(e => e.Class)
                      .HasForeignKey(e => e.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Class -> ClassSubject (One-to-Many)
                entity.HasMany(c => c.Subjects)
                      .WithOne(e => e.Class)
                      .HasForeignKey(e => e.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);
                // Class -> Assignments (One-to-Many)
                entity.HasMany(c => c.Assignments)
                      .WithOne(a => a.Class)
                      .HasForeignKey(a => a.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                // Subject -> Exams (One-to-Many)
                entity.HasMany(s => s.Exams)
                      .WithOne(e => e.Subject)
                      .HasForeignKey(e => e.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Subject -> ClassSubject (One-to-Many)
                entity.HasMany(c => c.Classes)
                      .WithOne(e => e.Subject)
                      .HasForeignKey(e => e.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Subject -> Assignment (One-to-Many)
                entity.HasMany(c => c.Assignments)
                      .WithOne(a => a.Subject)
                      .HasForeignKey(a => a.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Exam Relationships
            modelBuilder.Entity<Exam>(entity =>
            {

                // Exam -> Questions (One-to-Many)
                entity.HasMany(e => e.Questions)
                      .WithOne(q => q.Exam)
                      .HasForeignKey(q => q.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Exam -> Submissions (One-to-Many)
                entity.HasMany(e => e.Submissions)
                      .WithOne(s => s.Exam)
                      .HasForeignKey(s => s.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Question Configuration
            modelBuilder.Entity<Question>(entity =>
            {
                // JSON Column Configuration
                entity.Property(q => q.Options)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));
            });

            // Enrollment (Join Table)
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Composite PK
                entity.HasKey(e => new { e.ClassId, e.StudentId });

                // Enrollment -> Class (Many-to-One)
                entity.HasOne(e => e.Class)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.ClassId);

                // Enrollment -> Student (Many-to-One)
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId);
            });

            // Exam Submission
            modelBuilder.Entity<ExamSubmission>(entity =>
            {
                entity.HasKey(s => new { s.ExamId, s.StudentId });
                // JSON Answers Storage
                entity.Property(s => s.Answers)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => JsonSerializer.Deserialize<Dictionary<int, string>>(v, new JsonSerializerOptions()));

                // Submission -> Student (Many-to-One)
                entity.HasOne(s => s.Student)
                      .WithMany(s => s.ExamSubmissions)
                      .HasForeignKey(s => s.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AssignmentSubmission>(entity =>
            {
                // Submission -> Student (Many-to-One)
                entity.HasOne(a => a.Student)
                      .WithMany(s => s.AssignmentSubmission)
                      .HasForeignKey(s => s.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            base.OnModelCreating(modelBuilder);

        }
    }
}
