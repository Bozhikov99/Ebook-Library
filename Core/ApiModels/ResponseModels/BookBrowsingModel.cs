using Core.Books.Queries.GetBooks;

namespace Core.ApiModels.ResponseModels
{
    public class BookBrowsingModel
    {
        public IEnumerable<BookModel> Books { get; set; }
    }
}
