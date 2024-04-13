using Core.ApiModels.OutputModels;

namespace Core.Authors.Queries.Common
{
    public class AuthorModel : OutputBaseModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
    }
}
