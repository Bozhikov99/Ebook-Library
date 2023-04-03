namespace Core.ApiModels.OutputModels.User
{
    public class ListUserOutputModel : OutputBaseModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
