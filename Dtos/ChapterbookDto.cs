using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class ChapterbookDto
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string BookTitle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; }
        public string Editorial { get; set; }
        public bool IsApproved { get; set; }
        public string ChapterbookPath { get; set; }

    }
}