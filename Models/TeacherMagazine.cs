using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class TeacherMagazine
    {
        public string TeacherId { get; set; }
        public int MagazineId { get; set; }
        public int Order { get; set; }
        public Teacher Teacher { get; set; }
        public Magazine Magazine { get; set; }
    }
}