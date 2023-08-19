using MuonRoiSocialNetwork.StartupConfig;

await WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .CustomMidleWare()
    .RunAsync();
