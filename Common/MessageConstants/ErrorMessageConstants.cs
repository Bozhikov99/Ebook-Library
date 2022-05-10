namespace Common
{
    public class ErrorMessageConstants
    {
        public const string INVALID_LENGTH = "{0} must be between {2} and {1} characters long";
        public const string INVALID_LENGTH_MAXONLY = "{0} must be less than {1}";

        public const string GENRE_EXISTS = "Genre \"{0}\" already exists";
        public const string CREATE_GENRE_UNEXPECTED = "Unexpected error creating a genre";
        public const string INVALID_GENRE = "Genre '{0}' does not exist";
        public const string EDIT_GENRE_UNEXPECTED = "Unexpected error editing a genre";
        public const string DELETE_GENRE_UNEXPECTED = "Unexpected error deleting a genre";


        public const string AUTHOR_EXISTS = "Author \"{0}\" already exists";
        public const string CREATE_AUTHOR_UNEXPECTED = "Unexpected error creating an author";
        public const string DELETE_AUTHOR_UNEXPECTED = "Unexpected error deleting an author";
        public const string EDIT_AUTHOR_UNEXPECTED = "Unexpected error editing an author";

        public const string COVER_ISNULL = "You must upload the book's cover";
        public const string CONTENT_ISNULL = "You must upload the book's content";
        public const string COVER_INVALID_FORMAT = "Invalid cover image format";
        public const string COVER_ALLOWED_FORMATS = "The cover should be .PNG or .JPEG/JPG image";

        public const string CREATE_BOOK_UNEXPECTED = "Unexpected error creating a book";
        public const string EDIT_BOOK_UNEXPECTED = "Unexpected error editing a book";
        public const string DELETE_BOOK_UNEXPECTED = "Unexpected error deleting a book";
        public const string BOOK_EXISTS = "Book \"{0}\" already exists";
    }
}
