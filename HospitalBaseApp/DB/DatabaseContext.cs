using HospitalBaseApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalBaseApp.DB
{
    public class DatabaseContext : DbContext
    {
        public DbSet<OperationRecordModel> OperationsRecords { get; set; } = null!;

        public DbSet<OperationModel> Operations { get; set; } = null!;

        public DbSet<PatientModel> Patients { get; set; } = null!;

        public DbSet<DoctorModel> Doctors { get; set; } = null!;

        public DbSet<AppointmentModel> Appointments { get; set; } = null!;

        public DatabaseContext() { Database.EnsureCreated(); Appointments.Load(); Doctors.Load(); OperationsRecords.Load(); Operations.Load(); Patients.Load(); }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=HospitalDB.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientModel>().HasKey(p => new {  p.ID });
            modelBuilder.Entity<DoctorModel>().HasKey(x => new {x.Id  });
            modelBuilder.Entity<AppointmentModel>().HasKey(x => new { x.Id });
            base.OnModelCreating(modelBuilder);
        }




    }
    
}
