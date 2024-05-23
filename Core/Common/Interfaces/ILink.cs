namespace Core.Common.Interfaces
{
    public interface ILink
    {
        string Url { get; set; }

        string Rel { get; set; }

        string Method { get; set; }
    }
}
