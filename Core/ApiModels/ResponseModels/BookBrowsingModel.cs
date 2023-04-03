using Core.ApiModels.OutputModels.Book;

namespace Core.ApiModels.ResponseModels
{
    public class BookBrowsingModel
    {
        public IEnumerable<ListBookOutputModel> Books { get; set; }

        public string[] Genres { get; set; }
    }
}
