namespace PsadWebsite.App_Code.EnitityModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PsadDatabase : DbContext
    {
        public PsadDatabase()
            : base("name=PsadDatabase")
        {
        }

        public virtual DbSet<MeasurementData> MeasurementData { get; set; }
        public virtual DbSet<Measurements> Measurements { get; set; }
        public virtual DbSet<Operators> Operators { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Measurements>()
                .HasMany(e => e.MeasurementData)
                .WithOptional(e => e.Measurements)
                .HasForeignKey(e => e.MeasurementID);

            modelBuilder.Entity<Organisation>()
                .HasMany(e => e.Operators)
                .WithOptional(e => e.Organisation)
                .HasForeignKey(e => e.OrganisationID);
        }
    }
}
