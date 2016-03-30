namespace PsadWebsite.App_Code.EnitityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Organisation")]
    public partial class Organisation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organisation()
        {
            Operators = new HashSet<Operators>();
        }

        [Key]
        public Guid RecID { get; set; }

        [Required]
        [StringLength(100)]
        public string OrganisationName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public int? Phone { get; set; }

        [StringLength(100)]
        public string Contact { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operators> Operators { get; set; }
    }
}
