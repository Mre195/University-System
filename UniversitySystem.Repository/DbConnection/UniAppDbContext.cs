
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.Models;

namespace UniversitySystem.Repository.DbConnection;

public partial class UniAppDbContext : DbContext
{
    public UniAppDbContext(DbContextOptions<UniAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EnrollmentStatus> EnrollmentStatuses { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<SpecialRequest> SpecialRequests { get; set; }

    public virtual DbSet<SpecialRequestStatus> SpecialRequestStatuses { get; set; }

    public virtual DbSet<SpecialRequestType> SpecialRequestTypes { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC07226A54D7");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.UserId, "UQ__Admin__1788CC4D7CE98FC7").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserId)
                .HasConstraintName("FK_Admin_User");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC0781373414");

            entity.ToTable("Attendance");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EnrollmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_Enrollment");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course__3214EC071E3829A0");

            entity.ToTable("Course");

            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.Major).WithMany(p => p.Courses)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Major");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC075A6C927C");

            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Head).WithMany(p => p.Departments)
                .HasForeignKey(d => d.HeadId)
                .HasConstraintName("FK_Department_Head_Professor");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Enrollme__3214EC07E61B7FA5");

            entity.ToTable("Enrollment");

            entity.HasIndex(e => new { e.StudentId, e.SectionId }, "UQ_Enrollment_Student_Section").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnrollmentStatusId).HasDefaultValue(1);
            entity.Property(e => e.Grade)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.EnrollmentStatus).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.EnrollmentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_EnrollmentStatusId");

            entity.HasOne(d => d.Section).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Section");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<EnrollmentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Enrollme__3214EC07652B75A4");

            entity.ToTable("EnrollmentStatus");

            entity.HasIndex(e => e.Status, "UQ__Enrollme__3A15923FE8AB8071").IsUnique();

            entity.Property(e => e.Status).HasMaxLength(100);
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Major__3214EC07C38B03D9");

            entity.ToTable("Major");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Majors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Major_Department");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0754E02A53");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Message).HasMaxLength(1000);
            entity.Property(e => e.TargetEmail).HasMaxLength(150);

            entity.HasOne(d => d.SpecialRequest).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SpecialRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_SpecialRequest");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Professo__3214EC07AB70874F");

            entity.ToTable("Professor");

            entity.HasIndex(e => e.UserId, "UQ__Professo__1788CC4DBBDC8E05").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Department).WithMany(p => p.Professors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Professor_Department");

            entity.HasOne(d => d.User).WithOne(p => p.Professor)
                .HasForeignKey<Professor>(d => d.UserId)
                .HasConstraintName("FK_Professor_User");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Section__3214EC075989F060");

            entity.ToTable("Section");

            entity.Property(e => e.RoomCapacity).HasDefaultValue(30);
            entity.Property(e => e.RoomName)
                .HasMaxLength(50)
                .HasDefaultValue("****");
            entity.Property(e => e.Schedule).HasMaxLength(50);

            entity.HasOne(d => d.Course).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_Course");

            entity.HasOne(d => d.Professor).WithMany(p => p.Sections)
                .HasForeignKey(d => d.ProfessorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_Professor");

            entity.HasOne(d => d.Semester).WithMany(p => p.Sections)
                .HasForeignKey(d => d.SemesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_SemesterId");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Semester__3214EC07E19F96BD");

            entity.ToTable("Semester");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SpecialRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SpecialR__3214EC070A043831");

            entity.ToTable("SpecialRequest");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.SpecialRequestStatusId).HasDefaultValue(1);
            entity.Property(e => e.SpecialRequestTypeId).HasDefaultValue(1);
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.SpecialRequests)
                .HasForeignKey(d => d.EnrollmentId)
                .HasConstraintName("FK_SpecialRequest_Enrollment");

            entity.HasOne(d => d.SpecialRequestStatus).WithMany(p => p.SpecialRequests)
                .HasForeignKey(d => d.SpecialRequestStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SpecialRequest_SpecialRequestStatus");

            entity.HasOne(d => d.SpecialRequestType).WithMany(p => p.SpecialRequests)
                .HasForeignKey(d => d.SpecialRequestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SpecialRequest_SpecialRequestType");

            entity.HasOne(d => d.Student).WithMany(p => p.SpecialRequests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SpecialRequest_Student");
        });

        modelBuilder.Entity<SpecialRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SpecialR__3214EC076AC1A81F");

            entity.ToTable("SpecialRequestStatus");

            entity.HasIndex(e => e.Status, "UQ__SpecialR__3A15923FC5C7FAD7").IsUnique();

            entity.Property(e => e.Status).HasMaxLength(100);
        });

        modelBuilder.Entity<SpecialRequestType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SpecialR__3214EC0766E40D11");

            entity.ToTable("SpecialRequestType");

            entity.HasIndex(e => e.Type, "UQ__SpecialR__F9B8A48B2C4B0902").IsUnique();

            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC079543F658");

            entity.ToTable("Student");

            entity.HasIndex(e => e.UserId, "UQ__Student__1788CC4DEEC7F968").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Major).WithMany(p => p.Students)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Major");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .HasConstraintName("FK_Student_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC077CF8D2D4");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534BD128A4C").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
