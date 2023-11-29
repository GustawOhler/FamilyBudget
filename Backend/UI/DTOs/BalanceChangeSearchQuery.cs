using System;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetUI.DTOs
{
    public class BalanceChangeSearchQuery
    {
        [Required]
        public int PageNumber { get; set; }
        [Required]
        [Range(0, 250, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int PageSize { get; set; }
    }
}
