using Faces.WebMvc.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faces.WebMvc.RestClient
{
    public class OrderManagementApi : IOrderManagementApi
    {
        private IOrderManagementApi _restClient;
        private readonly IOptions<AppSettings> _settings;

        public OrderManagementApi(/*IConfiguration config,*/ HttpClient httpClient, IOptions<AppSettings> settings)
        {
            string apiHostAndPort = _settings.Value.OrdersApiUrl;
                //config.GetSection("ApiServiceLocations").GetValue<string>("OrdersApiLocation");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
            _restClient = RestService.For<IOrderManagementApi>(httpClient);
            _settings = settings;

        }

        public async Task<OrderViewModel> GetOrderById(Guid orderId)
        {
            try
            {
                return await _restClient.GetOrderById(orderId);
            }
            catch(ApiException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            return await _restClient.GetOrders();
        }
    }
}
