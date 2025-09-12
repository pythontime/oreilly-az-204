// See https://aka.ms/new-console-template for more information
// https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/quickstart-dotnet?tabs=azure-cli

using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

string hostname = "az204orcosmosgraph";
string primaryKey = "[COSMOS DB KEY HERE]";

GremlinServer server = new(
    $"{hostname}.gremlin.cosmos.azure.com",
    443,
    enableSsl: true,
    username: "/dbs/graphdb/colls/Persons",
    password: primaryKey
);

GremlinClient client = new(
    server,
    new GraphSON2MessageSerializer()
);


Dictionary<string, string> gremlinQueries = new Dictionary<string, string>
{
    { "Cleanup",        "g.V().drop()" },
    { "AddVertex 1",    "g.addV('person').property('id', 'thomas').property('firstName', 'Thomas').property('age', 44).property('pk', 'pk')" },
    { "AddVertex 2",    "g.addV('person').property('id', 'mary').property('firstName', 'Mary').property('lastName', 'Andersen').property('age', 39).property('pk', 'pk')" },
    { "AddVertex 3",    "g.addV('person').property('id', 'ben').property('firstName', 'Ben').property('lastName', 'Miller').property('pk', 'pk')" },
    { "AddVertex 4",    "g.addV('person').property('id', 'robin').property('firstName', 'Robin').property('lastName', 'Wakefield').property('pk', 'pk')" },
    { "AddEdge 1",      "g.V('thomas').addE('knows').to(g.V('mary'))" },
    { "AddEdge 2",      "g.V('thomas').addE('knows').to(g.V('ben'))" },
    { "AddEdge 3",      "g.V('ben').addE('knows').to(g.V('robin'))" },
};

foreach (var query in gremlinQueries)
{
    Console.WriteLine(String.Format("Running this query: {0}: {1}",
        query.Key, query.Value));

    await client.SubmitAsync(query.Value);

    Console.WriteLine();
}