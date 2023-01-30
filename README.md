# Introdução ao gRPC com .net 6

![](https://miro.medium.com/max/700/1*_cnL0L2c231UjdrpJvYMgw.png)

O gRPC ou Remote Procedure Call nasceu em 2015 e inicialmente era chamado de SPDY pelo seus criadores, o google, ele é uma estrutura de linguagem independente de alto desempenho.

Se formos a documentação oficial teremos a seguinte definição do gRPC:

> _A high-performance, open source universal RPC framework_

Quando dizemos independente temos sua primeira característica, que por se comunicar via Protobuf se torna ideal para alguns cenários onde existem diversas linguagens dentro da aplicação.

O gRPC tem como principais benefícios:

· Uma estrutura leve, moderna e de alto desempenho.

· Utilização da metodologia Contract First.

· Independe de linguagem. (CrossPlataform)

· Suporta comunicação entre Cliente e Servidor e chamadas bi-direcionais de streaming, o que veremos no exemplo abaixo.

· Tem um uso reduzido de rede por conta da serialização binária.

![](https://miro.medium.com/max/454/1*e46J6zwmeDOjnxqwvrjHvQ.png)

Quando falamos do Protocol Buffers, podemos dizer que é uma linguagem neutra, de plataforma neutra e extensível. É como trabalhar com JSON ou XML mas de forma binária.

Em um rápido comparativo entra chamadas REST e gRPC podemos trazer as seguintes características de cada um:

![](https://miro.medium.com/max/700/1*4Izk-uQNDSuUl578FS09WQ.png)

Veja abaixo um simples comparativo do tráfego de mensagens através dos métodos mais comuns no mercado em comparação com o tráfego Multiplexing que é suportado e é uma das suas principais características.

![](https://miro.medium.com/max/700/1*TqYKal1Ss5y-4Uk3RxyzeQ.png)

Claro que nesse ponto nós já devemos estar pensando em migrar todos os nossos serviços REST para gRPC mas vamos com calma pois sua implementação, por ser muito diferente do desenvolvimento habitual, se torna complexa e pelo fato de trafegar informações de forma binária, nós não conseguimos de forma fácil interpretar dados sendo enviados ou recebidos a não ser de posse do contrato, o que é uma grande vantagem quando falamos em segurança mas se torna um trabalho adicional para o processo de desenvolvimento.

Considere gRPC ideal para alguma dos seguintes cenários:

· Criação de microsserviços leves em que a eficiência é crítica.

· Sistemas multi linguagens.

· Serviços ponto a ponto com a necessidade de streaming em tempo real.

**Desvantagens:**

• Dificuldade em ser suportado via browser ou Clients específicos.

• O endpoint não é fácilmente testável.

• Os status codes são limitados e não padronizados.

Por ter sua implementação baseada em contrato, um arquivo  ***.proto**  terá uma estrutura semelhante a apresentada a seguir:

![](https://miro.medium.com/max/700/1*dsoPaAdBKHqWF--lwRZZ3A.png)

No .Net os objetos de clientes e mensagens são gerados de forma automática com a inclusão dos arquivos ***.proto** ao arquivo de configuração do projeto, como o exemplo abaixo:

    <ItemGroup> 
    	<Protobuf Include="Protos\greet.proto" />
    </ItemGroup>

Vamos agora a dois exemplos práticos para entender melhor a estrutura do gRPC no ASP.Net Core

De acordo com a documentação da Microsoft, os servios gRPC podem ser hospedados em ASP.NET Core e tem total integração com recursos como registro em logs, injeções de dependência, autenticação e autorização.

Para adicionar o gRPC a um aplicativo ASP.NET Core será necessário importarmos o pacote: _gRPC AspNetCore._

Abaixo vamos iniciar dois exemplos, o primeiro de uma cominicação de forma simples apenas para entendermos como ambiente a estrutura do gRPC funcionam.

No segundo exemplo vamos desenvolver um stream de envio de mensagens.

Para esses exemplos utilizaremos o Visual Studio 2022.

Nesse primeiro exemplo utilizaremos uma estrutura semelhante a apresentada na documentação da Microsoft para criação de um cliente gRPC que se comunica com o serviço de boas vindas do gRPC.

· **Inicie o Visual Studio e selecione Criar um novo projeto.**

**· Na caixa de diálogo que aparece pesquise por gRPC e selecione o tipo: ASP.NET Core serviço gRPC.**

![](https://miro.medium.com/max/700/1*7WzIqTZ_75v0MRSCvZD9ZA.png)

**· Na caixa de diálogo Configurar seu novo projeto, insira o seguinte nome: GrpcCommunication.**

**· Clique em Avançar.**

**· Na caixa de diálogo informações adicionais, selecione .NET 6.0 e em seguida selecione Criar.**

Examinando a estrutura criada:

![](https://miro.medium.com/max/436/1*Asre1Kd3eIzzNN4Auv1QSg.png)

-O primeiro arquivo é o mais importante em projetos gRPC é o  **Protos/greet.proto**, como falamos ele é usado para gerar um contrato do gRPC.

-Dentro da pasta Services temos o arquivo base para implementação, o GreeterService.

-O arquivo appSettings.json contém os dados de configuração da aplicação.

-O arquivo Program.cs contém tanto o ponto de entrada para o serviço gRPC e o código que configura o comportamento do aplicativo.

> _A partir do .net 6 a classe Program.cs substitui o comportamento da classe startup.cs, para mais informações leia:_
> 
> [Code samples migrated to the new minimal hosting model in ASP.NET Core 6.0](https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0)

**· Para que tenhamos acesso as informações do contrato greet.prot você deve recompilar a aplicação para que o deploy possa gerar as referências necessárias.**

**· Agora crie um outro projeto, dentro da solução, do tipo Aplicativo de Console e chame ele de: GrpcCommunicationClient**

**· Após o projeto criado adicione a referência aos seguintes pacotes:**

-   Grpc.Net.Client
-   Google.Protobuf
-   Grpc.Tools
-   **Nesse momento, se você for verificar o arquivo csproj do projeto você terá algo semelhante a esse abaixo:**

![](https://miro.medium.com/max/700/1*nPQX03M5WW16Qt-IhNyd-g.png)

· **Dentro do projeto cliente crie uma pasta com o nome: Protos**

**· Copie o arquivo Protos\greet.proto do serviço greeter gRPC para a pasta Protos no projeto de cliente.**

**· Dentro do arquivo copiado greet.proto atualize na linha 3 o namespace do projeto:**

    option csharp_namespace = "GrpcCommunicationClient";

**· Volte ao arquivo GrpcCommunicationClient.csproj e substitua o ItemGroup Protobuf criado como Server por:**

    <ItemGroup> 
    	<Protobuf Include="Protos\greet.proto" GrpcServices="Client" />
    	</ItemGroup>

Antes de criarmos o código com a lógica da aplicação execute o projeto server_(ctrl + F5)_, isso fará tanto um Build na nossa aplicação como dará ao compilador os tipos de GrpcCommuncationClient como também poderemos ver na janela de Output a porta que o server irá ouvir e responder como no exemplo abaixo:

![](https://miro.medium.com/max/700/1*wHeARw-3quBRyp8jmp7AMA.png)

Feito isso iremos criar a lógica da comunicação no arquivo  **Program.cs**  com o código a seguir:

**· A primeira coisa a se fazer são os imports, para esse exemplo vamos precisar dos seguintes imports:**

    using Grpc.Net.Client;using GrpcCommunicationClient;

· **Na sessão seguinte criaremos uma instância do GrpcChannel que contém as informações para criar a conexão com o serviço gRPC:**

    using var channel = GrpcChannel.ForAddress(“https://localhost:7106");
    var client = new Greeter.GreeterClient(channel);

**· Iremos agora chamar o método SayHello de forma assíncrona:**

    var repply = await client.SayHelloAsync(new HelloRequest { Name = 5“Communication test client Bliss Applications” });

**· Logo abaixo exibiremos a mensagem enviada no Console:**

    Console.WriteLine(“Greeting: “ + repply.Message);
    Console.WriteLine(“Press any key to exit…”);
    Console.ReadKey();

O resultado final do nosso código será esse:

![](https://miro.medium.com/max/700/1*w_lx-v7jUiuwWIRR0rukxw.png)

Antes de executarmos o projeto devemos ir as propriedades da solução informarmos que iremos executar os dois projetos ao mesmo tempo e teremos a configuração dessa forma:

![](https://miro.medium.com/max/700/1*GSm6LZYMPWr0aNvrtPAAGQ.png)

Agora temos o resultado final da nossa comunicação, o nosso client enviou a mensagem que foi recebida pelo server e retornada ao Client utilizando o gRPC.

![](https://miro.medium.com/max/700/1*IvvWl8McqmSuHRQHK8iyAw.png)

Agora vamos tentar um desafio um pouco maior, nessa segunda parte desse tutorial, vamos criar um exemplo mais completo para demostrar a capacidade de trabalhar com dados em streaming.

**· A primeira coisa a fazer é voltar as propriedades da solução e informar o projeto será inicializado apenas com o GrpcCommunication.**

**· Dentro da pasta Protos, crie um arquivo chamado: chat.proto.**

· **No arquivo criado altere o namespace para:**

    option csharp_namespace = “GrpcCommunication.Server”;

· **Faça a seguir esse import para que possamos trabalhar com o tipo timestamp**

    import “google/protobuf/timestamp.proto”;

**· Logo após iremos criar um pacote com o nome chat e em seguida criar o serviço Chat que vai trabalhar de forma “Multiplex” e irá tanto receber uma mensagem de stream do cliente para o servidor e retornar um stream do Server para o cliente.**

    package chat;
    service Chat{ 
    	rpc SendMessage(stream ClientToServerMessage) 
    returns (stream               ServerToClientMessage);
    }

**· Agora vamos criar duas mensagens, uma do Cliente para o servidor e outra do Servidor para o cliente. Na mensagem que parte do servidor colocaremos um timestamp para sabermos o momento que está sendo enviada.**

    message ClientToServerMessage{ 
    	string text = 1;
    }
    message ServerToClientMessage{ 
    	string text = 1; 
    	google.protobuf.Timestamp timestamp = 2;
    }

Note que a sintax do arquivo  ***.proto** exige que após a propriedade um número com a ordem da propriedade dentro da mensagem. Essa numeração auxilia para que novas versões que utilizem corretamente o padrão adotado não sobrescrevam uma propriedade já usada, garantindo não utilizar a mesma numeração dentro da mensagem em novas versões, mesmo que não vá utilizar do lado do cliente ou do servidor os antigos campos, garantindo assim que códigos antigos não se quebrem e também não engessando novas versões.

O resultado final do nosso contrato será o seguinte:

![](https://miro.medium.com/max/700/1*KL2CNDlCwIM1REYIAu_UcQ.png)

**· Vá ao arquivo: GrpcCommunication.csproj e adicione o seguinte ItemGroup:**

    <Protobuf Include=”Protos\chat.proto” GrpcServices=”Server” />

**· Faça um Rebuild da solution para os tipos possam ser reconhecidos dentro do projeto.**

**· Agora dentro da pasta Services crie a classe: ChatService.cs**

**· Inicie inserindo os seguintes usings na classe ChatService.cs:**

    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using GrpcCommunication.Server;

**· Agora extenda a classe para a classe base: Chat.ChatBase**

![](https://miro.medium.com/max/700/1*Yts8xNvLamHhoHug5VVfig.png)

-   **Feito isso crie a referência ao Ilogger e traga ele via injeção de dependência no construtor:**

    private readonly ILogger<ChatService> _logger;
    public ChatService(ILogger<ChatService> logger){
    	 _logger = logger;
     }

Agora iremos criar dois métodos privados na classe

-   **O primeiro é ServerToClientPingAsync que será o responsável por enviar mensagens do Server para o Client, nesse método iremos utilizar um contador para que ele possa exibir quantas mensagens já enviou. Utilizaremos também um While para que ele possa ficar repetindo esse envio até que a task seja cancelada e colocaremos um Delay de 5 segundos para que possamos ver o que está acontecendo nesse trecho do código.**

![](https://miro.medium.com/max/700/1*XgvWBKhZUfVfHArWOj9VlA.png)

**· O segundo método irá rodar enquanto houverem mensagens, utilizando o comando MoveNext() para avançar para o próximo item e exibirá essas mensagens enviadas do cliente para o servidor.**

![](https://miro.medium.com/max/700/1*hriLPVAjzVeUeQZ_JZTIEQ.png)

**· Agora que temos os dois métodos que usaremos para fazer o stream de mensagens, vamos criar o nosso método principal. Será necessário dar um OverView na classe SendMessage que ficará assim:**

    public override async Task SendMessage(IAsyncStreamReader<ClientToServerMessage> requestStream,  
    IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context){}

**· Agora iremos atribuir os dois métodos a variáveis para utilizarmos na chamada:**

    var clientToServerTask = ClientToServerPingHandlingAsync(requestStream, context);
    var serverToClientTask = ServerToClientPingAsync(responseStream, context);

**· Agora basta chamar esses dois métodos com o comando Task.WhenAll:**

    await Task.WhenAll(clientToServerTask, serverToClientTask);

O resultado final da nossa classe será:
![](https://miro.medium.com/max/700/1*Kp5glDVuRt8xz3hgF893GQ.png)

Já estamos quase finalizando nosso código, agora só teremos que fazer uma pequena alteração na classe  **Program.cs** em uma configuração que até as versões anteriores estaria no arquivo startup.cs:

**· Dentro de Program.cs insira o mapeamento do GrpcService para o chat através do código:**

    app.MapGrpcService<ChatService>();

![](https://miro.medium.com/max/700/1*VnVPBGamLrFOpiaILnPtWw.png)

Para executarmos e vermos como se comporta nosso exemplo utilizaremos o: BloomRPC que irá facilitar as nossas chamadas e acompanhamento desse exemplo.

Para baixar o  [BloomRPC](https://github.com/bloomrpc/bloomrpc/releases) acesse esse  [link](https://github.com/bloomrpc/bloomrpc/releases).

Uma vez instalado e aberto pela primeira vez você terá a seguinte aparência:

![](https://miro.medium.com/max/700/1*QIa_pMar9WTYeVZgCMMG3Q.png)

**· Clique no botão (+) para adicionar um arquivo e busque o arquivo chat.proto criado em nosso exemplo.**

**· Clique no nó SendMessage.**

**· Insira a porta correta para a chamada no endereço do localhost.**

**· No editor insira uma mensagem inicial para ser enviada.**

![](https://miro.medium.com/max/700/1*nSszT9zyO2VPxVRMWdOXFQ.png)

**· Antes de executar a chamada você deve iniciar seu projeto no Visual Studio ou IDE escolhida e garantir que esse esteja rodando.**

**· Após executar o projeto você pode clicar no Play que está no centro da tela do BloomRPC e ver que a mensagem do cliente será enviada e as mensagens do servidor serão enviadas a cada 5 segundos.**

![](https://miro.medium.com/max/700/1*aD0vXIVLBs9GA8V1lxHXpw.png)

![](https://miro.medium.com/max/700/1*VFX1zZrFiwIYS-OTvwCNlA.png)

Nesse artigo eu fiz uma abordagem inicial do gRPC e sua implementação em ASP.Net Core. As possibilidades de uso do gRPC são enormes e suas aplicações podem trazer uma grande diferença a um ambiente com o envio de muitas mensagens e/ou a necessidade do acompanhamento em tempo real de streaming.
