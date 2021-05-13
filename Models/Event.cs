using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.Models
{
    public class Event
    {
        public long Id { get; set; }

        public Application Application { get; set; }

        [Required]
        public Player Player { get; set; }

        public String Type { get; set; }

        [Timestamp]
        public DateTime Timestamp { get; set; }
    }
}
