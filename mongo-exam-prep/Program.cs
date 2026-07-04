using MongoDB.Driver;

const string uri = "mongodb://localhost:52601/";
var client = new MongoClient(uri);

var coll = client.GetDatabase("test").GetCollection<MyDoc>("documents");
coll.InsertOne(new MyDoc
{
    Name = "Test",
});



public class MyDoc
{
    public string Name { get; set; }
}