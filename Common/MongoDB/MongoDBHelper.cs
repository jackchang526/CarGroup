using BitAuto.CarUtils.Define;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Common.MongoDB
{
    /// <summary>
    /// Mongodb操作的封装类。    
    /// </summary>    
    public sealed class MongoDBHelper
    {
        public static readonly string connectionString_Default;
        public static readonly string database_Default;
        public static readonly string datatable_Default;

        static MongoDBHelper()
        {
            connectionString_Default = WebConfig.MongoDBConnectionString;
            database_Default = WebConfig.MongoDBDefaultDataBase;
            datatable_Default = WebConfig.MongoDBDefaultDataTable;
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <typeparam name="T">实例的类型</typeparam>
        /// <param name="connectionString">连接串</param>
        /// <param name="databaseName">数据库名</param>
        /// <param name="collectionName">集合名</param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "测试") 
        /// 或者 Query.EQ("Title", "测试") 或者Query.And(Query.Matches("Title", "测试"),
        /// Query.EQ("Author", "sandy")) 等等</param>
        /// <returns>T</returns>
        public static T GetOne<T>(IMongoQuery query)
        {
            MongoClient client = new MongoClient(connectionString_Default);
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase(database_Default);
            T result = default(T);
            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(datatable_Default);
                result = myCollection.FindOneAs<T>(query);
            }
            return result;
        }

        public static T GetOne<T>(IMongoQuery query, string[] fields, IMongoSortBy sortBy=null)
        {
            FieldsDocument fd = new FieldsDocument();
            if (fields.Length > 0)
            {
                foreach (string f in fields)
                {
                    fd.Add(f, 1);
                }
            }
            MongoClient client = new MongoClient(connectionString_Default);
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase(database_Default);
            T result = default(T);
            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(datatable_Default);
                var fieldList = myCollection.FindAs<T>(query).SetFields(fd);
                if (sortBy != null)
                {
                    fieldList.SetSortOrder(sortBy);
                }
                if (fieldList.Any())
                {
                    result = fieldList.First();
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "感冒") 或者 Query.EQ("Title", "感冒") 或者Query.And(Query.Matches("Title", "感冒"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        
        #region 获取指定字段

        public static MongoCursor<BsonDocument> GetFields(IMongoQuery query, string[] fields, IMongoSortBy sortBy = null)
        {
            MongoCursor<BsonDocument> mongoCursor = null;
            FieldsDocument fd = new FieldsDocument();
            if (fields.Length > 0)
            {
                foreach (string f in fields)
                {
                    fd.Add(f, 1);
                }
            }
            MongoClient client = new MongoClient(connectionString_Default);
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase(database_Default);
            using (server.RequestStart(database))
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(datatable_Default);
                mongoCursor = myCollection.FindAs<BsonDocument>(query).SetFields(fd);
            }
            return mongoCursor;
        }

        #endregion
    }

}
