using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissingDIExtensions;
using StructureMap;

using Algorithms;
using System.Threading;
using WebApiHelpers;
using CM = System.ComponentModel;

namespace AlgoPuzzles
{
    public class Startup
    {
        readonly Container container = new Container();
        readonly AsyncLocal<IContainer> scopeProvider = new AsyncLocal<IContainer>();

        private T GetInstance<T>() => scopeProvider.Value.GetInstance<T>();
        private object GetInstance(Type type) => scopeProvider.Value.GetInstance(type);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddMvc();

            services.AddRequestScopingMiddleware(() => scopeProvider.Value = container.GetNestedContainer());
            services.AddCustomControllerActivation(GetInstance);
            services.AddCustomViewComponentActivation(GetInstance);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            RegisterTypeConverters();
            RegisterApplicationComponents(app);            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            WebApiErrorHandlingMiddleware.UseWebApiJsonErrorResponse(app)       
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }

        private void RegisterTypeConverters()
        {
            CM.TypeDescriptor.AddAttributes(typeof(int[]), new CM.TypeConverterAttribute(typeof(SemicolonSeparatedArrayConvertor<int>)));
            CM.TypeDescriptor.AddAttributes(typeof(ListNode<int>), new CM.TypeConverterAttribute(typeof(SemicolonSeparatedSingleListConvertor<int>)));
        }

        private void RegisterApplicationComponents(IApplicationBuilder app)
        {
            container.Configure(c =>
            {

                // Register application services
                //c.For<IUserService>().Use<AspNetUserService>().ContainerScoped();
                
                c.Scan(scanner => {
                    scanner.AssemblyContainingType<IAlgo>();
                    //scanner.ConnectImplementationsToTypesClosing(typeof(IAlgo)).OnAddedPluginTypes(cpt => cpt.ContainerScoped());
                    //becams transient
                    //For deeper customization use a custom Convention: https://stackoverflow.com/questions/28896117/registering-types-with-structuremap-via-scan-method
                    scanner.AddAllTypesOf(typeof(IAlgo)); 
                });

                
                // Cross-wire required framework services
                //c.For<ILoggerFactory>().Use(loggerFactory);
                c.For<IViewBufferScope>().Use(_ => app.GetRequestService<IViewBufferScope>());
            });
        }

    }
}
