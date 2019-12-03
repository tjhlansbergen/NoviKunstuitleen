using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{

    public enum NoviUserType
    {
        Student, Teacher
    };
    public class NoviUser : IdentityUser
    {
        public string NoviNumber { get; set; }
        public NoviUserType Type { get; set; }
    }
}
