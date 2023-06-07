namespace ASPNBD.Models
{
	public class ComputerStoreDatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;
		public string DatabaseName { get; set; } = null!;
		public string ComputersCollectionName { get; set; } = null!;
	}
}
