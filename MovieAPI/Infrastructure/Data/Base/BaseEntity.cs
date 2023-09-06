using System;
using System.Security.Principal;

namespace MovieAPI.Infrastructure.Data.Base
{ 
    public abstract class BaseEntity 
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

