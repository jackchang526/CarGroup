using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	public class SerialCompareVoteDal
	{
		/// <summary>
		/// 投票
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public int UpdateVote(SerialCompareVoteEntity entity)
		{
			if (entity.Serial1 <= 0 || entity.Serial2 <= 0)
				return -1;
			SqlParameter[] _params = new SqlParameter[]{
                new SqlParameter("@Serial1", SqlDbType.Int)  ,
				new SqlParameter("@Serial2", SqlDbType.Int) ,
				 new SqlParameter("@Vote1", SqlDbType.Int) ,
				 new SqlParameter("@Vote2",SqlDbType.Int)
            };
			_params[0].Value = entity.Serial1;
			_params[1].Value = entity.Serial2;
			_params[2].Value = entity.VoteCount1;
			_params[3].Value = entity.VoteCount2;
			return SqlHelper.ExecuteNonQuery(WebConfig.DefaultConnectionString, CommandType.StoredProcedure, "[SP_SerialCompareVote_VoteIn]", _params);
		}
		/// <summary>
		/// 获取投票
		/// </summary>
		/// <param name="serial1"></param>
		/// <param name="serial2"></param>
		/// <returns></returns>
		public DataSet GetSerialVote(int serial1, int serial2)
		{
			if (serial1 <= 0 || serial2 <= 0) return null;
			string sql = @"SELECT  VoteCount1,VoteCount2
					FROM    dbo.Car_SerialCompareVote
					WHERE   SerialId1 = @Serial1
							AND SerialId2 = @Serial2 ";
			SqlParameter[] _params = new SqlParameter[]{
                new SqlParameter("@Serial1", SqlDbType.Int)  ,
				new SqlParameter("@Serial2", SqlDbType.Int)
            };
			_params[0].Value = serial1;
			_params[1].Value = serial2;
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
		}

	}
}
