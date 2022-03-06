using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
    public partial class NCDCContext : DbContext
    {
        public NCDCContext()
        {
        }

        public NCDCContext(DbContextOptions<NCDCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnalysisResults> AnalysisResults { get; set; }
        public virtual DbSet<AnalysisTypes> AnalysisTypes { get; set; }
        public virtual DbSet<Citizens> Citizens { get; set; }
        public virtual DbSet<CitzensAnalysis> CitzensAnalysis { get; set; }
        public virtual DbSet<EmployeePermissions> EmployeePermissions { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<Laboratories> Laboratories { get; set; }
        public virtual DbSet<MedicalAnalysis> MedicalAnalysis { get; set; }
        public virtual DbSet<Nationalities> Nationalities { get; set; }
        public virtual DbSet<Occupations> Occupations { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<ResultTypes> ResultTypes { get; set; }
        public virtual DbSet<UserPermissions> UserPermissions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalysisResults>(entity =>
            {
                entity.HasKey(e => new { e.ResultTypeId, e.AnalysisTypeId });

                entity.HasOne(d => d.AnalysisType)
                    .WithMany(p => p.AnalysisResults)
                    .HasForeignKey(d => d.AnalysisTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalysisResults_MedicalAnalysis");

                entity.HasOne(d => d.ResultType)
                    .WithMany(p => p.AnalysisResults)
                    .HasForeignKey(d => d.ResultTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalysisResults_ResultTypes");
            });

            modelBuilder.Entity<AnalysisTypes>(entity =>
            {
                entity.HasKey(e => e.AnalysisTypeId)
                    .HasName("PK_MedicalAnalysis");

                entity.Property(e => e.AnalysisTypeCode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.AnalysisTypeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AnalysisTypesCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalysisTypes_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.AnalysisTypesModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_AnalysisTypes_Users1");
            });

            modelBuilder.Entity<Citizens>(entity =>
            {
                entity.HasKey(e => e.CitizenId);

                entity.Property(e => e.Address).HasMaxLength(20);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Employee)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.EmployeePlace).HasMaxLength(30);

                entity.Property(e => e.FamilyStatus).HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.LaboratoryId).HasColumnName("laboratoryId");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.MotherName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NationalNo).HasMaxLength(12);

                entity.Property(e => e.PassportNo).HasMaxLength(20);

                entity.Property(e => e.PersonPhoto).HasMaxLength(100);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RegistrationNo).HasMaxLength(20);

                entity.Property(e => e.RegistryNo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CitizensCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Citizens_Users");

                entity.HasOne(d => d.Laboratory)
                    .WithMany(p => p.Citizens)
                    .HasForeignKey(d => d.LaboratoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Citizens_laboratories");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.CitizensModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Citizens_Users1");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Citizens)
                    .HasForeignKey(d => d.NationalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Citizens_Nationalities");

                entity.HasOne(d => d.Occupation)
                    .WithMany(p => p.Citizens)
                    .HasForeignKey(d => d.OccupationId)
                    .HasConstraintName("FK_Citizens_Occupations");
            });

            modelBuilder.Entity<CitzensAnalysis>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LaboratoryId).HasColumnName("laboratoryId");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Analysis)
                    .WithMany(p => p.CitzensAnalysis)
                    .HasForeignKey(d => d.AnalysisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CitzensAnalysis_AnalysisTypes");

                entity.HasOne(d => d.Citzen)
                    .WithMany(p => p.CitzensAnalysis)
                    .HasForeignKey(d => d.CitzenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CitzensAnalysis_Citizens");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CitzensAnalysisCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CitzensAnalysis_employees");

                entity.HasOne(d => d.Laboratory)
                    .WithMany(p => p.CitzensAnalysis)
                    .HasForeignKey(d => d.LaboratoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CitzensAnalysis_laboratories");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.CitzensAnalysisModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_CitzensAnalysis_employees1");

                entity.HasOne(d => d.ResultType)
                    .WithMany(p => p.CitzensAnalysis)
                    .HasForeignKey(d => d.ResultTypeId)
                    .HasConstraintName("FK_CitzensAnalysis_ResultTypes");
            });

            modelBuilder.Entity<EmployeePermissions>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.PermissionId });

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EmployeePermissions)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePermissions_Users");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeePermissions)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePermissions_employees");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.EmployeePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePermissions_Permissions");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.ToTable("employees");

                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RememberToken).HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.EmployeesCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employees_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.EmployeesModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_employees_Users1");
            });

            modelBuilder.Entity<Features>(entity =>
            {
                entity.HasKey(e => e.FeatureId);

                entity.Property(e => e.FeatureId).ValueGeneratedNever();

                entity.Property(e => e.FeatureName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Laboratories>(entity =>
            {
                entity.HasKey(e => e.LaboratoryId);

                entity.ToTable("laboratories");

                entity.Property(e => e.LaboratoryId).HasColumnName("laboratoryId");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LaboratoryName)
                    .IsRequired()
                    .HasColumnName("laboratoryName")
                    .HasMaxLength(150);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.LaboratoriesCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_laboratories_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.LaboratoriesModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_laboratories_Users1");
            });

            modelBuilder.Entity<MedicalAnalysis>(entity =>
            {
                entity.HasKey(e => e.AnalysisTypeId)
                    .HasName("PK_MedicalAnalysis_1");

                entity.Property(e => e.AnalysisTypeCode)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.AnalysisTypeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.MedicalAnalysisCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicalAnalysis_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.MedicalAnalysisModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_MedicalAnalysis_Users1");
            });

            modelBuilder.Entity<Nationalities>(entity =>
            {
                entity.HasKey(e => e.NationalitiyId);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NationalitiyNme)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Occupations>(entity =>
            {
                entity.HasKey(e => e.OccupationId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OccupationName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.OccupationsCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Occupations_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.OccupationsModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Occupations_Users1");
            });

            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.Property(e => e.PermissionId).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permissions_Features");
            });

            modelBuilder.Entity<ResultTypes>(entity =>
            {
                entity.HasKey(e => e.ResultTypeId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ResultTypeName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ResultTypesCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultTypes_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ResultTypesModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_ResultTypes_Users1");
            });

            modelBuilder.Entity<UserPermissions>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PermissionId });

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.UserPermissionsCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermissions_Users1");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermissions_Permissions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermissionsUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermissions_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(80);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RememberToken).HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Users");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.InverseModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Users_Users1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
