using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ASPNBD.Models;

namespace ASPNBD.Services
{
	public interface IDataService
	{
		Task<IEnumerable<Computers>> GetComputersAsync(int? year, string? name);
		Task<Computers> GetComputerAsync(string id);
		Task Create(Computers computer);
		Task Update(Computers computer);
		Task Delete(string id);
		Task<byte[]> GetImage(string id);
		Task StoreImage(string id, Stream imageStream, string fileName);
	}
}
