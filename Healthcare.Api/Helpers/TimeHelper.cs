
namespace Healthcare.Api.Helpers;

public static class TimeHelper
{
    public static DateTime RoundToFiveMinutes(DateTime time)
    {
        var minutes = time.Minute - (time.Minute % 5);
        return new DateTime(time.Year, time.Month, time.Day, time.Hour, minutes, 0, DateTimeKind.Utc);
    }
}
