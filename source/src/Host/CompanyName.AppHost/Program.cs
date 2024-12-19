using CompanyName.AppHost;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

const string
    yarpKey = "yarp",
    keycloakKey = "keycloak",
    seqKey = "seq",
    pgServerKey = "postgres-server",
    redisKey = "redis",
    rabbitmqKey = "rabbitmq",
    pgProductServiceDb = "pg-productservice",
    pgCategoryServiceDb = "pg-categoryservice",
    productServiceKey = "productservice",
    categoryServiceKey = "categoryservice";

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak(keycloakKey)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHealthCheck()
    .WithDataVolume();


IResourceBuilder<SeqResource> seq = builder
    .AddSeq(seqKey)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .ExcludeFromManifest();

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres(pgServerKey)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithHealthCheck()
    .WithPgAdmin();

IResourceBuilder<RedisResource> cache = builder
    .AddRedis(redisKey)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithHealthCheck()
    .WithRedisInsight();

IResourceBuilder<RabbitMQServerResource> rabbitmq = builder
    .AddRabbitMQ(rabbitmqKey)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHealthCheck()
    .WithDataVolume()
    .WithManagementPlugin(port: 15672);


IResourceBuilder<PostgresDatabaseResource> productServicePostgresDatabase = postgres.AddDatabase(pgProductServiceDb);
IResourceBuilder<ProjectResource> productService = builder
    .AddProject<Projects.CompanyName_Services_ProductService_WebApi>(productServiceKey)
    .WithHttpHealthCheck("/health")
    .WithReference(seq)
    .WaitFor(seq)
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(productServicePostgresDatabase)
    .WaitFor(productServicePostgresDatabase)
    .WithReference(keycloak)
    .WaitFor(keycloak);

IResourceBuilder<PostgresDatabaseResource> categoryServicePostgresDatabase = postgres.AddDatabase(pgCategoryServiceDb);
IResourceBuilder<ProjectResource> categoryService = builder
    .AddProject<Projects.CompanyName_Services_CategoryService_WebApi>(categoryServiceKey)
    .WithHttpHealthCheck("/health")
    .WithReference(seq)
    .WaitFor(seq)
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(categoryServicePostgresDatabase)
    .WaitFor(categoryServicePostgresDatabase)
    .WithReference(keycloak)
    .WaitFor(keycloak);


productService
    .WithReference(categoryService)
    .WaitFor(categoryService);



builder.AddProject<Projects.Yarp_ProxyService>(yarpKey)
    .WithReference(productService)
    .WithReference(categoryService)
    .WithReference(keycloak)
    .WithReference(seq)
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();
