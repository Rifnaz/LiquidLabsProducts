namespace DbLayer.Data.Models
{
	public class Product
	{
		public int Id { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string Category { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int Stock { get; set; }

		public List<Reviews> Reviews { get; set; } = new();
	}
}
