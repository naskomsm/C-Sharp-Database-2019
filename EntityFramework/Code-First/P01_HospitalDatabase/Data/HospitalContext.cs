namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options) 
            : base(options)
        {
        }

        public virtual DbSet<Diagnose> Diagnoses { get; set; }
        
        public virtual DbSet<Medicament> Medicaments { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Visitation> Visitations { get; set; }

        public virtual DbSet<PatientMedicament> PatientMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // not-unicode column
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(p => p.Email)
                .IsUnicode(false);
            });


            // many-to-many
            modelBuilder.Entity<PatientMedicament>()
                .HasKey(pm => new { pm.MedicamentId, pm.PatientId });

            modelBuilder.Entity<PatientMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId);

            modelBuilder.Entity<PatientMedicament>()
                .HasOne(pm => pm.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pm => pm.PatientId);
        }
    }
}
