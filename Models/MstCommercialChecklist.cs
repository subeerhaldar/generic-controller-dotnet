using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericController.Models
{
    [Table("MstCommercialChecklist")]
    public class MstCommercialChecklist : BaseEntity
    {
        [Key]
        public int ChkId { get; set; }

        [Required]
        public string Particulars { get; set; } = string.Empty;

        [Required]
        public int SortSeq { get; set; }

        [Required]
        [StringLength(1)]
        public string IsDeleted { get; set; } = "N";

        [Required]
        [StringLength(50)]
        public string ControlType { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Value { get; set; } = string.Empty;

        public bool IsFileUpload { get; set; } = false;

        public bool IsMandetory { get; set; } = false;

        [StringLength(50)]
        public string? ValidationType { get; set; }

        public string? CommercialPolicy { get; set; }

        public string? ForCosting { get; set; }
    }
}