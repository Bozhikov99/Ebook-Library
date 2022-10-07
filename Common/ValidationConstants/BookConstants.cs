namespace Common.ValidationConstants
{
    public class BookConstants
    {
        public const int RELEASEYEAR_MAX = 2022;

        public const int TITLE_MAXLENGTH = 500;

        public const int DESCRIPTION_MAXLENGTH = 5000;
        public const int DESCRIPTION_MINLENGTH = 20;

        public const int PAGE_SIZE = 4;

        public static string[] AllowedImageTypes = { "image/jpeg", "image/jpg", "image/png" };

        public const string AllowedContentType = "application/pdf";
    }
}
