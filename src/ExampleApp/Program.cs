using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Configuration;
using Platero.Nina.Core.Extensions;

namespace ExampleApp
{
    class Program
    {
        static async Task Main()
        {
            IOptions<NinaConfiguration> options = new OptionsWrapper<NinaConfiguration>(new());
            var serviceCollection = new ServiceCollection()
                .AddNinaRepositoryClients(options);

            var services = serviceCollection.BuildServiceProvider();

            var areaCodeRepository = services.GetService<IAreaCodeRepository>() ?? throw new InvalidOperationException($"Could not resolve type {nameof(IAreaCodeRepository)}");
            var areaCodes = await areaCodeRepository!.GetAreaCodeCollectionAsync();
            Console.WriteLine(areaCodes.Count);
        }
    }
}