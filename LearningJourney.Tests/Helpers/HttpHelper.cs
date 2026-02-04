using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace LearningJourney.Tests.Helpers;

/// <summary>
/// Helper class for making HTTP requests in integration tests
/// </summary>
internal class HttpHelper(HttpClient httpClient)
{

    /// <summary>
    /// Creates StringContent from an object for JSON POST/PUT requests
    /// </summary>
    public static StringContent GetJsonHttpContent(object data)
    {
        var jsonContent = JsonSerializer.Serialize(data);
        return new StringContent(jsonContent, Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Sets the Bearer token for authenticated requests
    /// </summary>
    public void SetBearerToken(string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Clears the Bearer token
    /// </summary>
    public void ClearBearerToken()
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    /// <summary>
    /// Makes a POST request and deserializes the response
    /// </summary>
    public async Task<T> PostAsync<T>(string url, object data)
    {
        var content = GetJsonHttpContent(data);
        var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent)!;
    }

    /// <summary>
    /// Makes a POST request and returns the response as a string
    /// </summary>
    public async Task<string> PostAsStringAsync(string url, object data)
    {
        var content = GetJsonHttpContent(data);
        var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Makes a POST request with status code check
    /// </summary>
    public async Task<HttpResponseMessage> PostWithResponseAsync(string url, object data)
    {
        var content = GetJsonHttpContent(data);
        return await httpClient.PostAsync(url, content);
    }

    /// <summary>
    /// Makes a PUT request
    /// </summary>
    public async Task<T> PutAsync<T>(string url, object data)
    {
        var content = GetJsonHttpContent(data);
        var response = await httpClient.PutAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent)!;
    }

    /// <summary>
    /// Makes a GET request and deserializes the response
    /// </summary>
    public async Task<T> GetAsync<T>(string url)
    {
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent)!;
    }

    /// <summary>
    /// Makes a DELETE request
    /// </summary>
    public async Task DeleteAsync(string url)
    {
        var response = await httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Makes a DELETE request with status code check
    /// </summary>
    public async Task<HttpResponseMessage> DeleteWithResponseAsync(string url)
    {
        return await httpClient.DeleteAsync(url);
    }

    /// <summary>
    /// Deserializes JSON response to specified type
    /// </summary>
    public static T DeserializeJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json)!;
    }

    /// <summary>
    /// Gets a property value from a JSON response
    /// </summary>
    public static string? GetJsonProperty(string json, string propertyName)
    {
        var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty(propertyName, out var property))
        {
            return property.GetString();
        }
        return null;
    }

    // API Endpoint URLs
    internal static class Endpoints
    {
        // Auth
        public const string Register = "/api/auth/register";
        public static string Login(Guid idNumber) => $"/api/auth/login/{idNumber}";

        // Hospitals
        public const string GetAllHospitals = "/api/hospitals";
        public static string GetHospital(Guid id) => $"/api/hospitals/{id}";
        public const string CreateHospital = "/api/hospitals";
        public static string UpdateHospital(Guid id) => $"/api/hospitals/{id}";
        public static string DeleteHospital(Guid id) => $"/api/hospitals/{id}";

        // Doctors
        public const string GetAllDoctors = "/api/doctors";
        public static string GetDoctor(Guid id) => $"/api/doctors/{id}";
        public const string CreateDoctor = "/api/doctors";
        public static string UpdateDoctor(Guid id) => $"/api/doctors/{id}";
        public static string DeleteDoctor(Guid id) => $"/api/doctors/{id}";

        // Customers
        public const string GetAllCustomers = "/api/customers";
        public static string GetCustomer(Guid id) => $"/api/customers/{id}";
        public const string CreateCustomer = "/api/customers";
        public static string UpdateCustomer(Guid id) => $"/api/customers/{id}";
        public static string DeleteCustomer(Guid id) => $"/api/customers/{id}";

        // Appointments
        public const string GetAllAppointments = "/api/appointments";
        public static string GetAppointment(Guid id) => $"/api/appointments/{id}";
        public const string CreateAppointment = "/api/appointments";
        public static string UpdateAppointment(Guid id) => $"/api/appointments/{id}";
        public static string DeleteAppointment(Guid id) => $"/api/appointments/{id}";
    }
}
