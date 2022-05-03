using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ValidationConstants
{
    public class AuthorConstants
    {
        public const string AUTHOR_NAME_REGEX = @"^([A-Z][a-z]+)$";
        public const int AUTHOR_NAME_MAXLENGTH = 75;
        public const int AUTHOR_NAME_MINLENGTH = 3;
    }
}
