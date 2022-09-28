using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcLoginRegister.Models
{
    public class UniqueDetail
    {
        [Key]
        public int id { get; set; }
        public string uniqueName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}