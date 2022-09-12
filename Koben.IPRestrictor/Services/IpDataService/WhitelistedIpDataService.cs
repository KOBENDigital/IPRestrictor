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
  internal class WhitelistedIpDataService : IWhitelistedIpDataService
	{
		private readonly IDatabaseProvider _dbProvider;
		private readonly IDataModelMapper<WhitelistedIpPoco, WhitelistedIpDto> _modelMapper;

		public WhitelistedIpDataService(IDatabaseProvider dbProvider, IDataModelMapper<WhitelistedIpPoco, WhitelistedIpDto> modelMapper)
		{
			_dbProvider = dbProvider;
			_modelMapper = modelMapper;
		}

		public IEnumerable<WhitelistedIpDto> GetAll()
		{
			using var db = _dbProvider.GetDatabase();

			var sql = Sql.Builder.Select($"* FROM {WhitelistedIpPoco.TableName} ORDER BY Alias");

			var data = db.Fetch<WhitelistedIpPoco>(sql);

			var mapped = _modelMapper.Map(data);

			return mapped;
		}

		public WhitelistedIpDto Get(long id)
		{
			using var db = _dbProvider.GetDatabase();

			var data = db.Query<WhitelistedIpPoco>().SingleOrDefault(x => x.Id == id);

			var mapped = _modelMapper.Map(data);

			return mapped;
		}

		public Page<WhitelistedIpDto> Get(int pageNumber, int pageSize)
		{
			throw new System.NotImplementedException();
		}

		public WhitelistedIpDto Insert(WhitelistedIpDto model)
		{
			using var db = _dbProvider.GetDatabase();

			var id = Convert.ToInt32(db.Insert(_modelMapper.Map(model)));

			return Get(id);
		}

		public IEnumerable<WhitelistedIpDto> Insert(IEnumerable<WhitelistedIpDto> models)
		{
			List<WhitelistedIpDto> results = new List<WhitelistedIpDto>();
			foreach (var model in models)
			{
				results.Add(Insert(model));
			}
			return results;
		}

		public WhitelistedIpDto Update(WhitelistedIpDto model)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<WhitelistedIpDto> Update(IEnumerable<WhitelistedIpDto> models)
		{
			throw new System.NotImplementedException();
		}

		public bool Delete(long id)
		{
			using var db = _dbProvider.GetDatabase();

			return db.Delete<WhitelistedIpPoco>(id) > 0;
		}
	}
}