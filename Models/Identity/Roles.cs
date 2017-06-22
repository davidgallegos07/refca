using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.Identity
{
    public class Roles
    {
        public const string Admin = "ADMIN";
        public const string Teacher = "TEACHER";
        public const string Owner = "OWNER";
        public const string AdminAndTeacher = Admin+","+ Teacher;
    }
}