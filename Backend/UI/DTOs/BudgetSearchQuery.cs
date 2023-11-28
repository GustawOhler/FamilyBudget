using System;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.DTOs
{
    public class BudgetSearchQuery
    {
        [Required]
        [Range(0, 250, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }
        public string? Name { get; set; }
    }
}
