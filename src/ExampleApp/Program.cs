using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Configuration;
using Platero.Nina.Core.Extensions;

namespace ExampleApp
{
    internal static class Program
    {
        private static async Task Main()
        {
            IOptions<NinaConfiguration> options = new OptionsWrapper<NinaConfiguration>(new NinaConfiguration());
            var serviceCollection = new ServiceCollection()
                .AddNinaRepositoryClients(options);

            var services = serviceCollection.BuildServiceProvider();

            var areaCodeRepository = services.GetService<IAreaCodeRepository>() ?? throw new InvalidOperationException($"Could not resolve type {nameof(IAreaCodeRepository)}");
            var warningsRepository = services.GetService<IDashboardRepository>() ?? throw new InvalidOperationException($"Could not resolve type {nameof(IDashboardRepository)}");

            var areaCodes = await areaCodeRepository!.GetAreaCodeSetAsync();

            var erfurtAreaCodes = areaCodes.Where(p => p.Name.Contains("Erfurt", StringComparison.CurrentCultureIgnoreCase));
            foreach (var areaCode in erfurtAreaCodes)
            {
                var dashboard = await warningsRepository.GetDashboardAsync(areaCode);
                foreach (var warning in dashboard.Messages)
                {
                    Console.WriteLine($"{dashboard.AreaCode.Name}, {warning.Date:dd.MM.yyyy, HH:mm} Uhr: {warning.Title} ({warning.Type})");
                }
            }
        }
    }
}