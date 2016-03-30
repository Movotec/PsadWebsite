namespace PsadWebsite.App_Code.EnitityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Patients
    {
        [Key]
        public Guid RecID { get; set; }

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public int? BornYear { get; set; }

        public int? BornMonth { get; set; }

        public int? BornDay { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public int? Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public double? ShoeSize { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        public double? AnkleCircumSize { get; set; }

        public double? Length1 { get; set; }

        public double? Length2 { get; set; }

        public double? FibulaLength { get; set; }

        [StringLength(6)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Diagnostic { get; set; }
    }
}
