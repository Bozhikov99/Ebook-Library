namespace Common.MessageConstants
{
    public class ErrorMessageConstants
    {
        public const string INVALID_LENGTH = "{0} must be between {2} and {1} characters long";
        public const string INVALID_LENGTH_MAXONLY = "{0} must be less than {1}";

        public const string GENRE_EXISTS = "Genre \"{0}\" already exists";
        public const string CREATE_GENRE_UNEXPECTED = "Unexpected error creating a genre";
        public const string INVALID_GENRE = "Genre \"{0}\" does not exist";
        public const string EDIT_GENRE_UNEXPECTED = "Unexpected error editing a genre";
        public const string DELETE_GENRE_UNEXPECTED = "Unexpected error deleting a genre";

        public const string AUTHOR_EXISTS = "Author \"{0}\" already exists";
        public const string CREATE_AUTHOR_UNEXPECTED = "Unexpected error creating an author";
        public const string DELETE_AUTHOR_UNEXPECTED = "Unexpected error deleting an author";
        public const string EDIT_AUTHOR_UNEXPECTED = "Unexpected error editing an author";

        public const string COVER_SIZE = "Book cover must be between 100 KB and 50 MB";
        public const string CONTENT_SIZE = "Book cover must be between 50 KB and 8 MB";
        public const string COVER_ISNULL = "You must upload the book's cover";
        public const string CONTENT_ISNULL = "You must upload the book's content";
        public const string COVER_INVALID_FORMAT = "Invalid cover image format";
        public const string CONTENT_INVALID_FORMAT = "Invalid book content format";
        public const string COVER_ALLOWED_FORMATS = "The cover should be .PNG or .JPEG/JPG image";
        public const string CONTENT_ALLOWED_FORMATS = "The book's content must be .pdf";

        public const string CREATE_BOOK_UNEXPECTED = "Unexpected error creating a book";
        public const string BOOK_DOES_NOT_EXIST = "Book does not exist";
        public const string EDIT_BOOK_UNEXPECTED = "Unexpected error editing a book";
        public const string DELETE_BOOK_UNEXPECTED = "Unexpected error deleting a book";
        public const string BOOK_EXISTS = "Book \"{0}\" already exists";

        public const string PASSWORDS_MUST_MATCH = "Passwords must match";
        public const string INVALID_USER = "Invalid user";
        public const string LOGIN_UNEXPECTED = "Unexpected error loging in";
        public const string REGISTER_UNEXPECTED = "Unexpected error registering";
        public const string USER_EXISTS = "Username is already taken";
        public const string EMAIL_EXISTS = "Email is already taken";

        public const string ROLES_EMPTY = "Roles must not be empty";
        public const string INVALID_ROLE_INPUT = "Role name must contain letters only";
        public const string ROLE_NOT_FOUND = "Role not found";

        public const string REVIEW_NOT_FOUND = "Review does not exist";
        public const string UNAUTHORIZED_REVIEW = "Cannot delete another user's review";

        public const string UNEXPECTED_ERROR = "Unexpected error";

    }
}
