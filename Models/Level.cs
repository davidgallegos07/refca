using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Level
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}