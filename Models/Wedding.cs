using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Validations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        public int WeddingId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string Wedder1 { get; set; }

        [Required]
        public string Wedder2 { get; set; }

        public List<RSVP> GuestList { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date { get; set; }

        [Required]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Wedding() { }
    }

    public class RSVP
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int WeddingId { get; set; }

        [Required]
        public int UserID { get; set; }

        public Wedding Wedding { get; set; }
        public User Guest { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }
}