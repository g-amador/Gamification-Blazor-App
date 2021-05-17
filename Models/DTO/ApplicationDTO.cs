using System;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.Models.DTO
{
    public class ApplicationDTO
    {
        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Key]
        public String ApiKey { get; set; }
    }
}
