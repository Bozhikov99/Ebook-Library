using Core.Common.Interfaces;

namespace Api.Hypermedia
{
    public class Link : ILink
    {
        public string Url { get; set; } = null!;

        public string Rel { get; set; } = null!;

        public string Method { get; set; } = null!;
    }
}
