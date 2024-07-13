using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace LSPApi.DataLayer.Model;
public partial class BookDto
{
    [Key]
    [JsonProperty("bookID")]

    public int BookID { get; set; }

    [JsonProperty("authorID")]
    public int AuthorID { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("subtitle")]
    public string? Subtitle { get; set; }

    [JsonProperty("isbn")]
    public string? ISBN { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("dateCreated")]
    public string? DateCreated { get; set; }

    [JsonProperty("dateUpdated")]
    public string? DateUpdated { get; set; }

    [JsonProperty("cover")]
    public string? Cover { get; set; }

    [JsonProperty("interior")]
    public string? Interior { get; set; }

    [JsonProperty("authorPhoto")]
    public string? AuthorPhoto { get; set; }

    [JsonProperty("authorBio")]
    public string? AuthorBio { get; set; }

    [JsonProperty("coverIdea")]
    public string? CoverIdea { get; set; }

    [JsonProperty("notes")]
    public string? Notes { get; set; }
}
