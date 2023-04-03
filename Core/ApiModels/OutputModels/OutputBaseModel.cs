using System.Text.Json.Serialization;

namespace Core.ApiModels.OutputModels
{
    public abstract class OutputBaseModel
    {
        public string Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<HateoasLink> Links { get; set; }
    }
}
