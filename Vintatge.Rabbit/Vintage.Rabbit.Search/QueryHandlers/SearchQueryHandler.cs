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

    internal class SearchQueryHandler : IQueryHandler<IList<Product>, SearchQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public SearchQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public IList<Product> Handle(SearchQuery query)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);
            var azureDirectory = new AzureDirectory(this.GetCloudBlobStorage(), "products");

            var searcher = new IndexSearcher(azureDirectory);

            var parser2 = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT, new string[] { "Code", "Title", "Description", "Keywords", "Price", "Type", "id" }, analyzer);
            
            var luceneQuery = parser2.Parse(query.Text);

            var hits = searcher.Search(luceneQuery, 20);

            IList<int> productIds = new List<int>();

            foreach(var hit in hits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);

                string id = doc.GetField("id").StringValue;
                productIds.Add(Int32.Parse(id));
            }

            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByIdsQuery>(new GetProductsByIdsQuery(productIds));
            IList<Product> returnProducts = new List<Product>();

            foreach(int productId in productIds)
            {
                if(products.Any(o => o.Id == productId))
                {
                    returnProducts.Add(products.First(o => o.Id == productId));
                }
            }

            return returnProducts;
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
