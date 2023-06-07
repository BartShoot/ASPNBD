using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ASPNBD.Models
{
	public class Computers
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		[BsonElement("Name")]
		[Display(Name = "Computer Name")]
		public string? Name { get; set; }
		[BsonElement("Year")]
		[Display(Name = "Creation Year")]
		public int? Year { get; set; }
		public string? ImageId { get; set; }
		public bool HasImage() => !string.IsNullOrWhiteSpace(ImageId);
	}
}
