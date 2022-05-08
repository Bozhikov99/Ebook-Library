namespace Common
{
    public class ErrorMessageConstants
    {
        public const string INVALID_LENGTH = "{0} must be between {2} and {1} characters long";

        public const string GENRE_EXISTS = "Genre '{0}' already exists";
        public const string CREATE_GENRE_UNEXPECTED = "Unexpected error creating a genre";
        public const string INVALID_GENRE = "Genre '{0}' does not exist";
        public const string EDIT_GENRE_UNEXPECTED = "Unexpected error editing a genre";
        public const string DELETE_GENRE_UNEXPECTED = "Unexpected error deleting a genre";


        public const string AUTHOR_EXISTS = "Author \"{0}\" already exists";
        public const string CREATE_AUTHOR_UNEXPECTED = "Unexpected error creating an author";
        public const string DELETE_AUTHOR_UNEXPECTED = "Unexpected error deleting an author";
        public const string EDIT_AUTHOR_UNEXPECTED = "Unexpected error editing an author";

    }
}
