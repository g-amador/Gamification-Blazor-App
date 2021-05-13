using System;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.Models
{
    public class ApplicationDTO
    {
        public long Id { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Key]
        public String ApiKey { get; set; }

    }
}
