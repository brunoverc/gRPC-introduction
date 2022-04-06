using Grpc.Net.Client;
using GrpcCommunicationClient;

using var channel = GrpcChannel.ForAddress("https://localhost:7106");
var client = new Greeter.GreeterClient(channel);

var repply = await client.SayHelloAsync(new HelloRequest { Name = "Communication test client Bliss Applications" });

Console.WriteLine("Greeting: " + repply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();