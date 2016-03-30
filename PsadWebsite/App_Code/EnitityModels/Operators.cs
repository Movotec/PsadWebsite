namespace PsadWebsite.App_Code.EnitityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Operators
    {
        [Key]
        public Guid RecID { get; set; }

        public Guid? OrganisationID { get; set; }

        [Required]
        [StringLength(100)]
        public string OperatorName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int? Phone { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [StringLength(1)]
        public string Type { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        public virtual Organisation Organisation { get; set; }
    }
}
