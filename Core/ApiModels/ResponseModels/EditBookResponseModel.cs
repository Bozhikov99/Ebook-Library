using Core.ApiModels.InputModels.Books;

namespace Core.ApiModels.ResponseModels
{
    public class EditBookResponseModel
    {
        public string Id { get; set; }

        public BookInputModel Model { get; set; }

        public BookInputDataModel BookData { get; set; }
    }
}
