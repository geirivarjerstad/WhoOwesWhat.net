using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Person")]
    public class Person
    {
        public Person()
        {
        }

        [Key]
        public int PersonId { get; set; }
        
        [Required]
        public Guid PersonGuid { get; set; }
         
        [Required]
        public string Displayname { get; set; }

        public string Mobile { get; set; }
        public bool IsDeleted { get; set; }

        
    }
}