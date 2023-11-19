namespace Aspire.Test.Blazor.UI;


public record EventApiModel(
    string Description,
    string AdditionalDescription,
    bool IsHoliday,
    bool IsReligious
);

public class CalendarApiClientService(HttpClient client)
{
    public async Task<List<EventApiModel>?> GetEvents(int year, int month, int day)
    {
        return await client.GetFromJsonAsync<List<EventApiModel>>($"/api/events/{year}/{month}/{day}");
    }
}