using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.Abstractions.Enums;
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
            var covidRulesRepository = services.GetService<ICovidRulesRepository>() ?? throw new InvalidOperationException($"Could not resolve type {nameof(ICovidRulesRepository)}");
            var warnMessageRepository = services.GetService<IWarnMessageRepository>() ?? throw new InvalidOperationException($"Could not resolve type {nameof(IWarnMessageRepository)}");
            
            var areaCodes = await areaCodeRepository!.GetAreaCodeSetAsync();

            var erfurtAreaCodes = areaCodes.Where(p => p.Name.StartsWith("Erfurt", StringComparison.CurrentCultureIgnoreCase));
            foreach (var areaCode in erfurtAreaCodes)
            {
                Console.WriteLine("Meldungen für {0}", areaCode.Name);
                
                var dashboard = await warningsRepository.GetDashboardAsync(areaCode);
                var rules = await covidRulesRepository.GetCovidRulesAsync(areaCode);

                foreach (var warning in dashboard.Messages)
                {
                    Console.WriteLine($"{warning.Date:g} Uhr: {warning.Title} ({warning.Type})");
                }

                Console.WriteLine();
                Console.WriteLine("Derzeit gültige Corona-Regeln:");
                Console.WriteLine("Aktueller Corona-Status: {0} ({1})", rules.CurrentStatus.Title, rules.CurrentStatus.IncidencesText.Replace('\n', ' '));
                Console.WriteLine("Generelle Verhaltenshinweise: {0}", rules.GeneralHintHtml);
               
                foreach (var rule in rules.OrderByDescending(p => p.Regulation.Source))
                {
                    Console.WriteLine("Regel: {0} (Quelle: {1})", rule.Title, rule.Regulation.Source.ToDescription());
                }
            }

            Console.WriteLine();
            Console.WriteLine("Derzeit aktuelle bundesweite Katastrophenmeldungen:");
            await foreach (var warnMessage in warnMessageRepository.GetWarnMessagesAsync(true, WarnMessageType.Dwd, WarnMessageType.Lhp))
            {
                Console.WriteLine("{2:g} Uhr: ({0} / {1}) {3}: {4}", warnMessage.ContentType.ToDescription(), warnMessage.Severity.ToDescription(), warnMessage.StartDate, warnMessage.Content, warnMessage.Details);
            }

        }
    }
}