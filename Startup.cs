using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using Umbraco.Extensions;

namespace Koben.IpRestrictor
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _config;

		/// <summary>
		/// Initializes a new instance of the <see cref="Startup" /> class.
		/// </summary>
		/// <param name="webHostEnvironment">The web hosting environment.</param>
		/// <param name="config">The configuration.</param>
		/// <remarks>
		/// Only a few services are possible to be injected here https://github.com/dotnet/aspnetcore/issues/9337
		/// </remarks>
		public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
		{
			_env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
			_config = config ?? throw new ArgumentNullException(nameof(config));
		}

		/// <summary>
		/// Configures the services.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <remarks>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		/// </remarks>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(10);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			var config = services
			.AddUmbraco(_env, _config)
			.AddBackOffice()
			.AddWebsite()
			.AddComposers();

			config.Build();
		}

		/// <summary>
		/// Configures the application.
		/// </summary>
		/// <param name="app">The application builder.</param>
		/// <param name="env">The web hosting environment.</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsEnvironment("Development"))
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error-500/");
			}

			app.UseHttpsRedirection();

			app.UseSession();

			app.UseRewriter(new RewriteOptions().AddIISUrlRewrite(env.ContentRootFileProvider, "IISUrlRewrite.xml"));

			app.UseUmbraco()
			.WithMiddleware(u =>
			{
				u.UseBackOffice();
				u.UseWebsite();
			})
			.WithEndpoints(u =>
			{
				u.UseInstallerEndpoints();
				u.UseBackOfficeEndpoints();
				u.UseWebsiteEndpoints();
			});
		}
	}
}