using System.Text.Json.Serialization;

namespace Aspire.Test.Calendar.Api.CalendarService;

public record Event(
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("additional_description")] string AdditionalDescription,
    [property: JsonPropertyName("is_holiday")] bool IsHoliday,
    [property: JsonPropertyName("is_religious")] bool IsReligious
);

public record CalendarApiResponseModel(
    [property: JsonPropertyName("is_holiday")] bool IsHoliday,
    [property: JsonPropertyName("events")] IReadOnlyList<Event> Events
);
