using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;

namespace Vintage.Rabbit.Search.CommandHandlers
{
    public class BuildLuceneIndexCommand
    {
        public BuildLuceneIndexCommand()
        {
        }
    }

    internal class BuildLuceneIndexCommandHandler : ICommandHandler<BuildLuceneIndexCommand>
    {
        private IQueryDispatcher _queryDispatcher;

        public BuildLuceneIndexCommandHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(BuildLuceneIndexCommand command)
        {
            PagedResult<Product> products = this._queryDispatcher.Dispatch<PagedResult<Product>, GetProductsQuery>(new GetProductsQuery(1, 5000));

            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);

            var azureDirectory = new AzureDirectory(this.GetCloudBlobStorage(), "products", new RAMDirectory());
            var indexWriter = new IndexWriter(azureDirectory, analyzer, true, new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH));

            foreach(var product in products)
            {
                var doc = new Document();

                doc.Add(new Field("id", product.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("guid", product.Guid.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("Code", product.Code, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("Title", product.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("Description", product.Description, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("Keywords", product.Keywords, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("Price", product.Cost.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
                doc.Add(new Field("Type", product.Type.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));

                indexWriter.AddDocument(doc);
            }
            indexWriter.Close();

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
