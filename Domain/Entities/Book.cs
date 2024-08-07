﻿using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Book
    {
        public Book()
        {
            Id = Guid.NewGuid()
                .ToString();

            BookGenres = new HashSet<BookGenre>();

            BookUsers = new HashSet<BookUser>();

            Reviews = new HashSet<Review>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [MaxLength(BookConstants.DESCRIPTION_MAXLENGTH)]
        public string Description { get; set; }
        
        public byte[] Cover { get; set; }

        public byte[] Content { get; set; }

        [Range(0, BookConstants.RELEASEYEAR_MAX)]
        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public virtual ICollection<BookGenre> BookGenres { get; set; }

        public virtual ICollection<BookUser> BookUsers { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
