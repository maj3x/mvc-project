using System;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.ViewModels
{
    public class AssignmentModel : BaseModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Teslim tarihi zorunludur")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string AssignedById { get; set; }
        public string AssignedByName { get; set; }

        [Required(ErrorMessage = "Öğrenci seçimi zorunludur")]
        public string AssignedToId { get; set; }
        public string AssignedToName { get; set; }

        public AssignmentStatus Status { get; set; }
    }
}
