using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;

namespace Amy.Toolkit.PlainElastic
{
    public class ElasticSearchHelper
    {
        public static readonly ElasticSearchHelper Intance = new ElasticSearchHelper();

        private ElasticConnection Client;

        private ElasticSearchHelper()
        {
            Client = new ElasticConnection("192.168.10.60", 9200);
        }

        /// <summary>
        /// 数据索引
        /// </summary>       
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="id">索引文档id，不能重复,如果重复则覆盖原先的</param>
        /// <param name="jsonDocument">要索引的文档,json格式</param>
        /// <returns>索引结果</returns>
        public IndexResult Index(string indexName, string indexType, string id, string jsonDocument)
        {

            var serializer = new JsonNetSerializer();
            string cmd = new IndexCommand(indexName, indexType, id);
            OperationResult result = Client.Put(cmd, jsonDocument);
            var indexResult = serializer.ToIndexResult(result.Result);
            return indexResult;
        }
        /// <summary>
        /// 数据索引
        /// </summary>       
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="id">索引文档id，不能重复,如果重复则覆盖原先的</param>
        /// <param name="jsonDocument">要索引的文档,json格式</param>
        /// <returns>索引结果</returns>
        public IndexResult Delete(string indexName, string indexType, string id)
        {
            try
            {
                var serializer = new JsonNetSerializer();
                string cmd = new IndexCommand(indexName, indexType, id);
                OperationResult result = Client.Delete(cmd);
                var indexResult = serializer.ToIndexResult(result.Result);
                return indexResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 数据索引
        /// </summary>       
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="id">索引文档id，不能重复,如果重复则覆盖原先的</param>
        /// <param name="jsonDocument">要索引的文档,object格式</param>
        /// <returns>索引结果</returns>
        public IndexResult Index(string indexName, string indexType, string id, object document)
        {
            var serializer = new JsonNetSerializer(); 
            var jsonDocument = serializer.Serialize(document); 
            return Index(indexName, indexType, id, jsonDocument);
        }
        
        /// <summary>
        /// 全文检索
        /// </summary>
        /// <typeparam name="T">搜索类型</typeparam>
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="query">查询条件（单个字段或者多字段或关系）</param>
        /// <param name="from">当前页（0为第一页）</param>
        /// <param name="size">页大小</param>
        /// <returns>搜索结果</returns>
        public SearchResult<T> Search<T>(string indexName, string indexType, QueryBuilder<T> query, int from, int size)
        {
            var queryString = query.From(from).Size(size).Build(); 
            var cmd = new SearchCommand(indexName, indexType); 
            var result = Client.Post(cmd, queryString); 
            var serializer = new JsonNetSerializer(); 
            return serializer.ToSearchResult<T>(result);
        }
        /// <summary>
        /// 全文搜索 采用SCROLL方式
        /// </summary>
        /// <typeparam name="T">搜索类型</typeparam>
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="query">查询条件（单个字段或者多字段或关系）</param>
        /// <param name="from">当前页（0为第一页）</param>
        /// <param name="size">页大小</param>
        /// <returns>搜索结果</returns>
        public SearchResult<T> Search<T>(string indexName, string indexType, QueryBuilder<T> query ,ref string scroll_id)
        {
            var queryString = query.Build();
            if (string.IsNullOrEmpty(scroll_id))
            {
                string scrollingSearchCommand = new SearchCommand(indexName, indexType)
                          .Scroll("3m");
                string results = Client.Post(scrollingSearchCommand, queryString);
                var serializer = new JsonNetSerializer();
                var noteResults = serializer.ToSearchResult<T>(results);
                scroll_id = noteResults._scroll_id;
                return noteResults;
            }
            else {
                try
                {
                    //当前已经存在分页。
                    string results = Client.Get(Commands.SearchScroll(scroll_id).Scroll("3m"));
                    var serializer = new JsonNetSerializer();
                    var noteResults = serializer.ToSearchResult<T>(results);
                    scroll_id = noteResults._scroll_id;
                    return noteResults;
                }
                catch (Exception ex)
                {
                    string scrollingSearchCommand = new SearchCommand(indexName, indexType)
                        .Scroll("3m");
                    string results = Client.Post(scrollingSearchCommand, queryString);
                    var serializer = new JsonNetSerializer();
                    var noteResults = serializer.ToSearchResult<T>(results);
                    scroll_id = noteResults._scroll_id;
                    return noteResults;
                }
            }  
            //string results = Client.Get(Commands.SearchScroll(scroll_id).Scroll("3m"));
             
        }


        public List<string> GetIKTokenFromStr(string key)
        {
            string s = "/db_materialture/_analyze?analyzer=ik_smart&text=" + key;
            var result = Client.Get(s);
            var serializer = new JsonNetSerializer();
            var list = serializer.Deserialize(result, typeof(ik)) as ik;
            return list.tokens.Select(c => c.token).ToList();
        }
        public class ik
        {
             public List<tokens> tokens{get ;set;}
        }
        public class tokens
        {
            public string token { get; set; }
            public string start_offset { get; set; }
            public string end_offset { get; set; }

            public string type { get; set; }

            public string position { get; set; }
        }
    }
}