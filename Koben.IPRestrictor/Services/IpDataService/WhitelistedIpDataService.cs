using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.ObjectMapping.Interfaces;
using Koben.Persistence.NPoco.Interfaces;
using NPoco;

namespace Koben.IPRestrictor.Services.IpDataService
{
	internal class WhiteListedIpDataService : IWhiteListedIpDataService
	{
		private readonly IDatabaseProvider _dbProvider;
		private readonly ITwoWayMapper<WhiteListedIpPoco, WhiteListedIpDto> _modelMapper;

		public WhiteListedIpDataService(IDatabaseProvider dbProvider, ITwoWayMapper<WhiteListedIpPoco, WhiteListedIpDto> modelMapper)
		{
			_dbProvider = dbProvider;
			_modelMapper = modelMapper;
		}

		public IEnumerable<WhiteListedIpDto>? GetAll()
		{
			using var db = _dbProvider.GetDatabase();

			var sql = Sql.Builder
				.Select("*")
				.From(WhiteListedIpPoco.TableName)
				.OrderBy(nameof(WhiteListedIpPoco.Alias));

			var data = db.Fetch<WhiteListedIpPoco>(sql);

			var mapped = _modelMapper.Map(data);

			return mapped;
		}

		public WhiteListedIpDto? Get(long id)
		{
			using var db = _dbProvider.GetDatabase();

			var data = db.Query<WhiteListedIpPoco>().SingleOrDefault(x => x.Id == id);

			var mapped = _modelMapper.Map(data);

			return mapped;
		}

		public long Insert(WhiteListedIpDto model)
		{
			using var db = _dbProvider.GetDatabase();

			var id = Convert.ToInt32(db.Insert(_modelMapper.Map(model)));

			return id;
		}

		public WhiteListedIpDto InsertAndGet(WhiteListedIpDto model)
		{
			var id = Insert(model);

			//suppressing as the only way this can be null is if the create method failed
			return Get(id)!;
		}

		public IEnumerable<long> Insert(IEnumerable<WhiteListedIpDto> models)
		{
			return models.Select(Insert).ToList();
		}

		public bool Update(WhiteListedIpDto model)
		{
			using var db = _dbProvider.GetDatabase();

			var poco = _modelMapper.Map(model);

			if (poco == null)
			{
				return false;
			}

			return db.Update(poco) > 0;
		}

		public WhiteListedIpDto? UpdateAndGet(WhiteListedIpDto model)
		{
			var update = Update(model);

			return update ? Get(model.Id) : null;
		}

		public bool Delete(long id)
		{
			using var db = _dbProvider.GetDatabase();

			return db.Delete<WhiteListedIpPoco>(id) > 0;
		}
	}
}