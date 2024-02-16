using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TinyURL.Models
{
    public class ShortUrlCls
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string OriginalUrl { get; set; } = null!;

        public string ShortUrl { get; set; } = null!;

        public DateTime CreateDate { get; set; }
    }
}
