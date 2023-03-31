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

        public const int COVER_MINLENGTH = 102400;
        public const int COVER_MAXLENGTH = 52428800;
        
        public const int CONTENT_MINLENGTH = 51200;
        public const int CONTENT_MAXLENGTH = 8388608;
    }
}
