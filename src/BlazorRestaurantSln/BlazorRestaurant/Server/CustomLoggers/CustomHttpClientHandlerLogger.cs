using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PTI.Microservices.Library.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static PTI.Microservices.Library.Interceptors.CustomHttpClientHandler;

namespace BlazorRestaurant.Server.CustomLoggers
{
    /// <summary>
    /// Custom logger for <see cref="CustomHttpClientHandler"/>
    /// </summary>
    public class CustomHttpClientHandlerLogger : ILogger<CustomHttpClientHandler>
    {
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CustomHttpClientHandlerLogger"/>
        /// </summary>
        public CustomHttpClientHandlerLogger(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            this.ServiceScopeFactory = serviceScopeFactory;
            this.Mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            if (logLevel == LogLevel.Information)
            {
                CustomHttpClientHandlerRequestResponseModel model =
                    JsonSerializer.Deserialize<CustomHttpClientHandlerRequestResponseModel>(message);
                if (model != null)
                {
                    using var scope = this.ServiceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BlazorRestaurantDbContext>();
                    var entity = this.Mapper
                        .Map<CustomHttpClientHandlerRequestResponseModel, ExternalRequestTracking>(model);
                    await dbContext.ExternalRequestTracking.AddAsync(entity);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
