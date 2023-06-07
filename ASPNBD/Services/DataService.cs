using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ASPNBD.Models;

namespace ASPNBD.Services
{
	public class DataService : IDataService
	{
		private readonly IMongoCollection<Computers> _computers;
		private readonly GridFSBucket _gridFS;

		public DataService(IOptions<ComputerStoreDatabaseSettings> computerDatabaseSettings, GridFSBucket fSBucket)
		{
			var mongoClient = new MongoClient(computerDatabaseSettings.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(computerDatabaseSettings.Value.DatabaseName);

			_computers = mongoDatabase.GetCollection<Computers>(computerDatabaseSettings.Value.ComputersCollectionName);

			_gridFS = fSBucket;
		}

		public async Task Create(Computers computer) => await _computers.InsertOneAsync(computer);

		public async Task Delete(string id) => await _computers.DeleteOneAsync(computer => computer.Id == id);

		public async Task<Computers> GetComputerAsync(string id) => await _computers.Find(computer => computer.Id == id).FirstOrDefaultAsync();

		public async Task<IEnumerable<Computers>> GetComputersAsync(int? year, string? name)
		{
			var builder = new FilterDefinitionBuilder<Computers>();
			var filter = builder.Empty;

			if(!string.IsNullOrWhiteSpace(name))
			{
				filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
			}

			if(year.HasValue)
			{
				filter = filter & builder.Eq("Year", year.Value);
			}

			return await _computers.Find(filter).ToListAsync();
		}

		public async Task<byte[]> GetImage(string id) => await _gridFS.DownloadAsBytesAsync(new ObjectId(id));

		public async Task StoreImage(string id, Stream imageStream, string fileName)
		{
			var computers = await GetComputerAsync(id);
			if(computers.HasImage())
			{
				await _gridFS.DeleteAsync(new ObjectId(computers.ImageId));
			}
			var imageId = await _gridFS.UploadFromStreamAsync(fileName, imageStream);
			computers.ImageId = imageId.ToString();

			var filter = Builders<Computers>.Filter.Eq("_id", id);
			var update = Builders<Computers>.Update.Set("ImageId", computers.ImageId);

			await _computers.UpdateOneAsync(filter, update);
		}

		public async Task Update(Computers computer) => await _computers.ReplaceOneAsync(c => c.Id == computer.Id, computer);
	}
}
