using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace logPropertis
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string A { get; set; }
        public string B { get; set; }
    }


}
