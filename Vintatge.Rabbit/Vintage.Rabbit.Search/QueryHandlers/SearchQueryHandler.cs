using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Products.Entities;
using Microsoft.WindowsAzure.Storage;
using Lucene.Net.Search;
using Lucene.Net.Store.Azure;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using System.Configuration;
using Vintage.Rabbit.Products.QueryHandlers;
using Lucene.Net.Store;
using Vintage.Rabbit.Common.Entities;

namespace Vintage.Rabbit.Search.QueryHandlers
{
    public class SearchQuery
    {
        public string Text { get; private set; }

        public SearchQuery(string text)
        {
            this.Text = text;
        }
    }

    internal class SearchQueryHandler : IQueryHandler<PagedResult<Product>, SearchQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public SearchQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public PagedResult<Product> Handle(SearchQuery query)
        {
            try
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
                var azureDirectory = new AzureDirectory(this.GetCloudBlobStorage(), "products", new RAMDirectory());

                var searcher = new IndexSearcher(azureDirectory);

                var parser2 = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT, new string[] { "Code", "Title", "Description", "Keywords", "Price", "Type", "id" }, analyzer);

                var luceneQuery = parser2.Parse(query.Text);

                var hits = searcher.Search(luceneQuery, 20);

                IList<Guid> productGuids = new List<Guid>();

                foreach (var hit in hits.ScoreDocs)
                {
                    var doc = searcher.Doc(hit.Doc);

                    Guid guid = new Guid(doc.GetField("guid").StringValue);
                    productGuids.Add(guid);
                }

                IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByGuidsQuery>(new GetProductsByGuidsQuery(productGuids));
                IList<Product> returnProducts = new List<Product>();

                foreach (Guid productGuid in productGuids)
                {
                    if (products.Any(o => o.Guid == productGuid))
                    {
                        returnProducts.Add(products.First(o => o.Guid == productGuid));
                    }
                }

                PagedResult<Product> result = new PagedResult<Product>();
                result.AddRange(returnProducts);

                return result;
            }
            catch(Exception exception)
            {
                return new PagedResult<Product>();
            }
        }

        private CloudStorageAccount GetCloudBlobStorage()
        {
            string connectionInfo = ConfigurationManager.AppSettings["BlobStorageConnectionString"];

            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionInfo);

            return storageAccount;
        }
    }
}
