using System;
using System.ComponentModel.DataAnnotations;

namespace QuotesApi.Models
{
    public class Quote
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Quote()
        {
        }
    }
}
