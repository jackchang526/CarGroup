using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.BLL
{

	public class SerialCompareVoteBll
	{
		private readonly SerialCompareVoteDal voteDal = new SerialCompareVoteDal();
		/// <summary>
		/// 获取投票数据
		/// </summary>
		/// <param name="serial1"></param>
		/// <param name="serial2"></param>
		/// <returns></returns>
		public int[] GetSerialVote(int serial1, int serial2)
		{
			int[] result = { 0, 0 };
			DataSet ds = voteDal.GetSerialVote(serial1, serial2);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow dr = ds.Tables[0].Rows[0];
				result[0] = Convert.ToInt32(dr["VoteCount1"]);
				result[1] = Convert.ToInt32(dr["VoteCount2"]);
			}
			return result;
		}
		/// <summary>
		/// 投票
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool UpdateVote(SerialCompareVoteEntity entity)
		{
			return voteDal.UpdateVote(entity) > 0 ? true : false;
		}
	}
}
