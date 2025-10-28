using System.ComponentModel.DataAnnotations;

namespace GenericController.Models
{
    public abstract class BaseEntity
    {
        [Required]
        public bool IsActive { get; set; } = true;
    }
}