using Refit;

namespace Aspire.Test.Calendar.Api.CalendarService;

public interface ICalendarApiService
{
    [Get("/jalali/{year}/{month}/{day}")]
    Task<CalendarApiResponseModel> GetGeorgianDate(int year, int month, int day);
}