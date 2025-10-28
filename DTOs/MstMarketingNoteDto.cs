using System.ComponentModel.DataAnnotations;

namespace GenericController.DTOs
{
    public class MstMarketingNoteDto
    {
        [Required]
        [StringLength(10)]
        public string NoteCode { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public int IsChildOf { get; set; }

        public int? SortSeq { get; set; }

        [StringLength(1)]
        public string? IsDeleted { get; set; } = "N";

        [StringLength(50)]
        public string? ControlType { get; set; }

        [StringLength(1000)]
        public string? Value { get; set; }

        public bool? IsFileUpload { get; set; }

        [StringLength(100)]
        public string? FileLibrary { get; set; }

        public bool? IsMandatory { get; set; }

        [StringLength(50)]
        public string? ValidationType { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}