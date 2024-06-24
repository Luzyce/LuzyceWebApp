using System.Net.Http.Json;
using Luzyce.Core.Models.Order;

namespace LuzyceWebApp.Services;

public class OrderService(HttpClient httpClient)
{
    public async Task<GetOrdersResponseDto> GetOrdersAsync(int pageNumber, GetOrdersDto getOrdersDto, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/order/{pageNumber}", getOrdersDto, cancellationToken);
        return await response.Content.ReadFromJsonAsync<GetOrdersResponseDto>(cancellationToken) ?? new GetOrdersResponseDto();
    }
}
