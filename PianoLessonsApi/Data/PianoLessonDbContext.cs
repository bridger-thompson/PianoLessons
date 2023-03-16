﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Data;

public partial class PianoLessonDbContext : DbContext
{
    public PianoLessonDbContext()
    {
    }

    public PianoLessonDbContext(DbContextOptions<PianoLessonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<PracticeAssignment> PracticeAssignments { get; set; }

    public virtual DbSet<PracticeLog> PracticeLogs { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAssignment> StudentAssignments { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointment_pkey");

            entity.ToTable("appointment", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_at");
            entity.Property(e => e.StartAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_at");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Subject).HasColumnName("subject");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_student_id_fkey");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_teacher_id_fkey");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_pkey");

            entity.ToTable("course", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("course_teacher_id_fkey");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_history_pkey");

            entity.ToTable("payment_history", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.PayDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("pay_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Student).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_history_student_id_fkey");

            entity.HasOne(d => d.Teacher).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_history_teacher_id_fkey");
        });

        modelBuilder.Entity<PracticeAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("practice_assignment_pkey");

            entity.ToTable("practice_assignment", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Course).WithMany(p => p.PracticeAssignments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("practice_assignment_course_id_fkey");
        });

        modelBuilder.Entity<PracticeLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("practice_log_pkey");

            entity.ToTable("practice_log", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.LogDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("log_date");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Assignment).WithMany(p => p.PracticeLogs)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("practice_log_assignment_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.PracticeLogs)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("practice_log_student_id_fkey");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_pkey");

            entity.ToTable("student", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<StudentAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_assignment_pkey");

            entity.ToTable("student_assignment", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Assignment).WithMany(p => p.StudentAssignments)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_assignment_assignment_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAssignments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_assignment_student_id_fkey");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_course_pkey");

            entity.ToTable("student_course", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_course_course_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_course_student_id_fkey");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("teacher_pkey");

            entity.ToTable("teacher", "piano_lessons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}