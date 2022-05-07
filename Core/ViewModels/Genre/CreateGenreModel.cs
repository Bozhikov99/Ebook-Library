using Common;
using Common.ValidationConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Genre
{
    public class CreateGenreModel
    {
        [Required]
        [StringLength(GenreConstants.NAME_MAX_LENGTH, MinimumLength = GenreConstants.NAME_MIN_LENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Name { get; set; }
    }
}