using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamificationApi.Models
{
    public class Rule
    {
        public long Id { get; set; }
        
        public Application Application { get; set; }
        
        public String OnEventType { get; set; }
        
        public int NumberOfPoints { get; set; }

        public int NumberOfCredits { get; set; }

        public Badge Badge { get; set; }
    }
}
