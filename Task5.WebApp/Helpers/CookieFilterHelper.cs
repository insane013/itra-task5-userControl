using System.Text.Json;
using Task5.Models;
using Task5.Models.User;

namespace Task5.WebApp.Helpers;

public class CookieFilterHelper
{
    public static void SaveFilter(string filterName, UserFilter filter, HttpResponse response)
    {
        var json = JsonSerializer.Serialize(filter);
        response.Cookies.Append(filterName, json, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,
            Secure = true,
        });
    }

    public static T LoadFilter<T>(string filterName, HttpRequest request) where T : BaseFilter, new()
    {
        if (request.Cookies.TryGetValue(filterName, out var json))
        {
            return JsonSerializer.Deserialize<T>(json) ?? new T();
        }

        return new T();
    }
}
