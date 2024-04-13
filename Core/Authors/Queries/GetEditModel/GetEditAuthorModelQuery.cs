using Core.Authors.Queries.Common;

namespace Core.Authors.Queries.GetEditModel
{
    public class GetEditAuthorModelQuery : IRequest<AuthorModel>
    {
        public string Id { get; set; } = null!;
    }
}
