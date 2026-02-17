namespace ProjectEmailWithIdentity.Helpers
{
    public static class DateHelper
    {
        public static string ToRelativeDate(DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return "just now";

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes + " mins ago";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours + " hours ago";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days + " days ago";

            if (timeSpan <= TimeSpan.FromDays(365))
                return (timeSpan.Days / 30) + " months ago";

            return (timeSpan.Days / 365) + " years ago";
        }
    }
}

