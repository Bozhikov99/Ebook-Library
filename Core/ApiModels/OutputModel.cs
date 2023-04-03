using System.Text.Json.Serialization;

namespace Core.ApiModels
{
    public abstract class OutputModel
    {
        public string Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<HateoasLink> Links { get; set; }
    }
}
