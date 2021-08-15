﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Platero.Nina.Core.Abstractions;
using Platero.Nina.Core.AreaCodes;
using Platero.Nina.Core.Configuration;
using Platero.Nina.Core.Dashboards;

namespace Platero.Nina.Core.Extensions
{
    /// <summary>
    /// Gestattet die Initialisierung der Dependency Injection in .NET-Projekten.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private const string URI_DEFAULT_AREACODES = "https://www.xrepository.de/api/xrepository/urn:de:bund:destatis:bevoelkerungsstatistik:schluessel:rs_2021-07-31/download/Regionalschl_ssel_2021-07-31.json";
        private const string URI_DEFAULT_DASHBOARDS = "https://warnung.bund.de/api31/dashboard";
        
        /// <summary>
        /// Konfiguriert die HTTP-Clients für die Benutzung in den Repositories.
        /// </summary>
        /// <param name="services"> Die initialisierende <see cref="IServiceCollection"/>.</param>
        /// <param name="options"> Die anzuwendende Konfiguration. </param>
        public static IServiceCollection AddNinaRepositoryClients(this IServiceCollection services, IOptions<NinaConfiguration> options)
        {
            var configuration = options.Value;
            
            return services
                .AddHttpClient<IAreaCodeRepository, XoevAreaCodeRepository>(c => c.BaseAddress = configuration.Urls?.AreaCodes ?? new Uri(URI_DEFAULT_AREACODES)).Services
                .AddHttpClient<IDashboardRepository, NinaDashboardRepository>(c => c.BaseAddress = configuration.Urls?.NinaDashboard ?? new Uri(URI_DEFAULT_DASHBOARDS)).Services;
        } 
    }
}