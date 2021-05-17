using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamificationApi.Models
{
    public class Application
    {
        public long Id { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        public String ApiKey { get; set; }

        [DataType(DataType.Password)]
        public String ApiSecret { get; set; }
    }
}
