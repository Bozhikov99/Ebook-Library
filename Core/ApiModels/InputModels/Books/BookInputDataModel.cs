using Core.Authors.Queries.Common;
using Core.ViewModels.Genre;

namespace Core.ApiModels.InputModels.Books
{
    public class BookInputDataModel
    {
        public IEnumerable<AuthorModel> Authors { get; set; } = new List<AuthorModel>();

        public IEnumerable<ListGenreModel> Genres { get; set; } = new List<ListGenreModel>();
    }
}
