using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericController.Models
{
    [Table("MstLegalChecklist")]
    public class MstLegalChecklist : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LegalChkId { get; set; }

        public int? SrNo { get; set; }

        public string? ScreenPoint { get; set; }

        public string? Recommendation { get; set; }

        public string? ApprovalForDeviation { get; set; }

        public int? SortSeq { get; set; }

        [StringLength(1)]
        public string? IsDeleted { get; set; } = "N";

        [StringLength(50)]
        public string? ControlType { get; set; }

        [StringLength(1000)]
        public string? Value { get; set; }

        public bool? IsFileUpload { get; set; }

        public bool? IsMandetory { get; set; } = false;

        [StringLength(50)]
        public string? ValidationType { get; set; }

        public string? Termsnconditions { get; set; }
    }
}