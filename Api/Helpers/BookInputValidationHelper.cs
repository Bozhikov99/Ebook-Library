using Common.MessageConstants;
using Common.ValidationConstants;
using Core.ApiModels.InputModels.Books;

namespace Api.Helpers
{
    public static class BookInputValidationHelper
    {
        private static readonly Func<BookInputModel, string> validateCover = (BookInputModel model) =>
        {
            if (model.Cover is null)
            {
                return ErrorMessageConstants.COVER_ISNULL;
            }

            int coverSize = model
                .Cover
                .Length;

            bool isValid = coverSize >= BookConstants.COVER_MINLENGTH && coverSize <= BookConstants.COVER_MAXLENGTH;

            string output = isValid ? null : ErrorMessageConstants.COVER_SIZE;

            return output;
        };

        private static readonly Func<BookInputModel, string> validateContent = (BookInputModel model) =>
        {
            if (model.Content is null)
            {
                return ErrorMessageConstants.COVER_ISNULL;
            }

            int contentSize = model
                .Content
                .Length;

            bool isValid = contentSize >= BookConstants.CONTENT_MINLENGTH && contentSize <= BookConstants.CONTENT_MAXLENGTH;

            string output = isValid ? null : ErrorMessageConstants.COVER_SIZE;

            return output;
        };

        private static Func<BookInputModel, string>[] validations =
        {
            validateCover,
            validateContent
        };

        public static IEnumerable<string> Validate(BookInputModel model)
        {
            HashSet<string> errors = new HashSet<string>();

            foreach (Func<BookInputModel, string> validation in validations)
            {
                string result = validation(model);

                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }
            }

            return errors;
        }
    }
}
