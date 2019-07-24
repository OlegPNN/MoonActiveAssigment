using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using MoonActivePrint2Concole.Domain.Infrustructure;
using MoonActivePrint2Concole.Domain.Infrustructure.Impl;

namespace MoonActivePrint2Concole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IPrintHandler printerHandler = new PrintHandler();
            Thread thread = new Thread(new ThreadStart(printerHandler.LoopForMessages))
            {
                IsBackground = true,
                Name = "Check for Messages to print thread"
            };
            thread.Start();

            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(
                    (builderContext, configurationBuilder) =>
                    {
                        configurationBuilder
                            .AddJsonFile(
                                $"appsettings.json",
                                optional: true,
                                reloadOnChange: true);
                    });

    }
}
