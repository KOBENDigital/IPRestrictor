using Koben.IPRestrictor.Models;
using Koben.IPRestrictor.Services.IpDataService.Interfaces;
using Koben.IPRestrictor.Services.IpDataService.Models;
using Koben.Persistence.Interfaces;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Koben.IPRestrictor.Services.IpDataService
{
  internal class WhiteListedIpDataService : IWhiteListedIpDataService
	{
		private readonly IDatabaseProvider _dbProvider;
		private readonly IDataModelMapper<WhiteListedIpPoco, WhiteListedIpDto> _modelMapper;

		public WhiteListedIpDataService(IDatabaseProvider dbProvider, IDataModelMapper<WhiteListedIpPoco, WhiteListedIpDto> modelMapper)
		{
			_dbProvider = dbProvider;
			_modelMapper = modelMapper;
		}

		public IEnumerable<WhiteListedIpDto> GetAll()
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

		public WhiteListedIpDto Get(long id)
		{
			using var db = _dbProvider.GetDatabase();

			var data = db.Query<WhiteListedIpPoco>().SingleOrDefault(x => x.Id == id);

			var mapped = _modelMapper.Map(data);

			return mapped;
		}

		public Page<WhiteListedIpDto> Get(int pageNumber, int pageSize)
		{
			throw new System.NotImplementedException();
		}

		public WhiteListedIpDto Insert(WhiteListedIpDto model)
		{
			using var db = _dbProvider.GetDatabase();

			var id = Convert.ToInt32(db.Insert(_modelMapper.Map(model)));

			return Get(id);
		}

		public IEnumerable<WhiteListedIpDto> Insert(IEnumerable<WhiteListedIpDto> models)
		{
			return models.Select(Insert).ToList();
		}

		public WhiteListedIpDto Update(WhiteListedIpDto model)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<WhiteListedIpDto> Update(IEnumerable<WhiteListedIpDto> models)
		{
			throw new System.NotImplementedException();
		}

		public bool Delete(long id)
		{
			using var db = _dbProvider.GetDatabase();

			return db.Delete<WhiteListedIpPoco>(id) > 0;
		}
	}
}