using MediSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Infrastructure.Persistence
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();
        public DbSet<Appointments> Appointments => Set<Appointments>();
        public DbSet<Doctor> Doctors => Set<Doctor>();   
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Reminder> Reminders => Set<Reminder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // one to one
            modelBuilder.Entity<User>().HasOne(u => u.patient).WithOne(p => p.user).HasForeignKey<Patient>(p => p.UserID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasOne(u => u.doctor).WithOne(d => d.user).HasForeignKey<Doctor>(d => d.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prescription>().HasOne(p => p.Patient).WithMany(p => p.prescriptions).HasForeignKey(p => p.PatientID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription>().HasOne(p => p.Doctor).WithMany(d => d.prescriptions).HasForeignKey(p => p.DoctorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointments>().HasOne(a => a.Doctor).WithMany().HasForeignKey(p => p.DoctorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointments>().HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DoctorSchedule>().HasOne(s => s.Doctor).WithMany().HasForeignKey(s => s.DoctorId).OnDelete(DeleteBehavior.Cascade);


        }


    }
}
