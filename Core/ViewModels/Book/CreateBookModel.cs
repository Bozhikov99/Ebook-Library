using Common.ValidationConstants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Book
{
    public class CreateBookModel
    {
        [Required]
        public string Title { get; set; }

        [MaxLength(BookConstants.DESCRIPTION_MAXLENGTH)]
        public string Description { get; set; }

        public byte[] Cover { get; set; }

        [Range(0, BookConstants.RELEASEYEAR_MAX)]
        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
    }
}
