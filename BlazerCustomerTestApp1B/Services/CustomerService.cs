using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace BlazerCustomerTestApp1B.Services;

public class CustomerService {
  private readonly HttpClient _http;

  public CustomerService(IHttpClientFactory factory) {
    _http = factory.CreateClient("CustomerTestApp1");
  }

  public async Task<(List<CustomerResponseDto> data, string? error)> GetAllAsync() {
    try {
      var result = await _http.GetFromJsonAsync<ApiResponse<List<CustomerResponseDto>>>(
        "api/Customers"
      );
      return (result?.Data ?? new List<CustomerResponseDto>(), null);
    } catch (Exception ex) {
      return (new List<CustomerResponseDto>(), $"Could not reach the APi: {ex.Message} ");
    }
  }

  public async Task<CustomerResponseDto?> GetByIdAsync(int id) {
    var result = await _http.GetFromJsonAsync<ApiResponse<CustomerResponseDto>>(
      $"api/Customers/{id}"
    );
    return result?.Data;
  }

  public async Task<(bool success, string? error)> CreateAsync(CustomerCreationDto customer) {
    try {
      var response = await _http.PostAsJsonAsync("api/Customers", customer);
      if (!response.IsSuccessStatusCode) {
        var body = await response.Content.ReadAsStringAsync();
        return (false, $"API returned {(int)response.StatusCode}: {body}");
      }

      return (true, null);
    } catch (Exception ex) {
      return (false, $"Could not reach Api : {ex.Message}");
    }
  }

  public async Task<(bool success, string? error)> UpdateAsync(int id, CustomerUpdateDto customer) {
    var response = await _http.PutAsJsonAsync($"api/Customers/{id}", customer);

    return (true, null);
  }

  public async Task<(bool success, string? error)> DeleteAsync(int id) {
    try {
      var response = await _http.DeleteAsync($"api/Customers/{id}");
      if (response.IsSuccessStatusCode) {
        var body = await response.Content.ReadAsStringAsync();
        return (false, $"Api returned {(int)response.StatusCode}: {body} ");
      }

      return (true, null);
    } catch (Exception ex) {
      return (false, $"Could not reach Api: {ex.Message}");
    }
  }
}