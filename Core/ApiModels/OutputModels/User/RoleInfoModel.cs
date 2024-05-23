using Core.Common.Interfaces;

namespace Core.ApiModels.OutputModels.User
{
    public class RoleInfoModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
