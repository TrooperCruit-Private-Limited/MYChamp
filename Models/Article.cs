using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MYChamp.Models
{
    public class Article
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("relevancy")]
        public string Relevancy { get; set; }

        [Column("medialinks")]
        public string MediaLinks { get; set; }

       
        [Column("coverimagedata")]
        [ValidateNever]
        public byte[] CoverImageData { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile CoverPath { get; set; } 

        [Column("category")]
        public string Category { get; set; }

        [Column("useremail")]
        public string UserEmail { get; set; }

        [Column("rating")]
        public double Rating { get; set; }

        [Column("isarchived")]
        public bool IsArchived { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("authorposition")]
        public string AuthorPosition { get; set; }
    }
}
