// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;
using Couchbase;
using Couchbase.KeyValue;

var cluster = await Cluster.ConnectAsync("couchbase://localhost", "admin", "@dm1n123$");

var bucket = await cluster.BucketAsync("ttl-test");

var collection = bucket.DefaultCollection();

var documentKey = "ttl-test-doc-1";

var document = new {
    Name = "Lift Weight",
    Description = "This is a test document to validate TTL"
};

var upsertResult = await collection.UpsertAsync(documentKey, document, options => options.Expiry(TimeSpan.FromSeconds(5)));

await Task.Delay(TimeSpan.FromSeconds(1));

var getResult = await GetDocument(documentKey, collection);

Console.WriteLine($"Document: {documentKey} - {getResult.ContentAs<dynamic>()}");

await Task.Delay(TimeSpan.FromSeconds(1));

var getResultAfterTTL = await GetDocument(documentKey, collection);

Console.WriteLine($"Document after TTL: {documentKey} - {getResultAfterTTL.ContentAs<dynamic>()}");

static async Task<IGetResult> GetDocument(string documentKey, ICouchbaseCollection collection){
    try{
        return await collection.GetAsync(documentKey);
        
    }
    catch(Exception ex){
        Console.WriteLine($"ErrorMessage: {ex.Message} \r\n Stacktrack: {ex.StackTrace}");
        throw;
    }
    
}

