﻿using DrSharp.Web.Raven;
using Microsoft.AspNet.SignalR;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Session;
using Nancy.TinyIoc;
using Raven.Client;

namespace DrSharp.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var store = RavenSessionProvider.DocumentStore;
            container.Register<IDocumentStore>(store);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var store = container.Resolve<IDocumentStore>();
            var documentSesion = store.OpenSession();

            container.Register(documentSesion);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                var documentSession = container.Resolve<IDocumentSession>();

                if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    documentSession.SaveChanges();
                }

                documentSession.Dispose();
            }
            );
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            CookieBasedSessions.Enable(pipelines);
            container.Register(GlobalHost.ConnectionManager.GetHubContext<SharpHub>());
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Scripts"));
        }
    }
}