using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class ServiceCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public required string Description { get; set; }

        // UserId will be set from the authenticated user's claims
    }
}