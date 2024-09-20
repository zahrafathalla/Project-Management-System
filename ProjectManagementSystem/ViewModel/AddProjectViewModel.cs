using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModel
{
    public class AddProjectViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<int> UserIds { get; set; } = new List<int>();
    }
}