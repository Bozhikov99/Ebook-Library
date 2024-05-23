namespace Core.Common.Interfaces
{
    public interface IHypermediaResource
    {
        string Id { get; set; }

        IEnumerable<ILink> Links { get; set; }
    }
}
