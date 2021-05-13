using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.Models
{
    public class Badge
    {
        public long Id { get; set; }

        public Application Application { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }
        
        public String Icon { get; set; }
    }
}
