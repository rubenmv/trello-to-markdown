using System.Text.Json.Serialization;

namespace TrelloToMarkdown
{
    public class TrelloBoard 
    {
        [JsonPropertyName("cards")]
        public IEnumerable<TrelloCard>? Cards { get; set; }
        [JsonPropertyName("lists")]
        public IEnumerable<TrelloList>? Lists { get; set; }
        [JsonPropertyName("checklists")]
        public IEnumerable<TrelloCheckList>? Checklists { get; set; }
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class TrelloCard
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("idList")]
        public string? IdList { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("desc")]
        public string? Desc { get; set; }
        [JsonPropertyName("attachments")]
        public IEnumerable<TrelloAttachment>? Attachments { get; set; }
        [JsonPropertyName("dateLastActivity")]
        public DateTime? DateLastActivity { get; set; }
        [JsonPropertyName("closed")]
        public bool Closed { get; set; } = false;

        public bool Transfered { get; set; } = false;
        public string DateOnlyString => DateLastActivity.HasValue ? DateLastActivity.Value.ToString("yyyy-MM-dd") : "0000-00-00";
    }

    public class TrelloAttachment
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name{ get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class TrelloList
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("closed")]
        public bool Closed { get; set; } = false;
    }

    public class TrelloCheckList
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("idCard")]
        public string? IdCard { get; set; }
        [JsonPropertyName("checkItems")]
        public IEnumerable<TrelloCheckItem>? CheckItems { get; set; }
    }

    public class TrelloCheckItem
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        public bool Checked => State == "complete";
    }
}