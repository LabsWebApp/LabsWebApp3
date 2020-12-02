using System;

namespace LabsWebApp3.Areas.Chat.Controllers
{
    public enum TimesForBlock : long
    {
        Forever = -1,
        Minute = TimeSpan.TicksPerMinute,
        Hour = TimeSpan.TicksPerHour,
        ThreeHours = Hour * 3,
        Day = TimeSpan.TicksPerDay,
        Week = Day * 7,
        Month = Day * 30
    }
}
