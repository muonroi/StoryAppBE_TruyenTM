using Autofac;
using BaseConfig.Infrashtructure.Interface;
using BaseConfig.Infrashtructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BaseConfig.Infrashtructure.Filter;
using Newtonsoft.Json;

namespace BaseConfig.BaseStartUp
{
    public class AuthContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AmqpContext>().As<IAmqpContext>().InstancePerLifetimeScope();
            builder.Register(delegate (IComponentContext context)
            {
                ILogger<AuthHeaderHandler> logger2 = context.Resolve<ILogger<AuthHeaderHandler>>();
                AuthContext authContext2 = context.Resolve<AuthContext>();
                return new AuthHeaderHandler(logger2, authContext2);
            }).InstancePerLifetimeScope();
            _ = builder.Register(delegate (IComponentContext context)
            {
                ILogger<AuthContext> logger = context.Resolve<ILogger<AuthContext>>();
                IHttpContextAccessor httpContextAccessor = context.Resolve<IHttpContextAccessor>();
                AuthContext authContext;
                if (httpContextAccessor?.HttpContext != null)
                {
                    authContext = new AuthContext(httpContextAccessor);
                    logger.LogTrace($"Init AuthContext by IHttpContextAccessor: {authContext}");
                }
                else
                {
                    IAmqpContext? amqpContext = ResolutionExtensions.ResolveOptional<IAmqpContext>(context);
                    authContext = new AuthContext(amqpContext);
                    logger.LogInformation("Init AuthContext by IAmqpContext: " + JsonConvert.SerializeObject(authContext));
                }

                return authContext;
            }).InstancePerLifetimeScope();
        }
    }
}
