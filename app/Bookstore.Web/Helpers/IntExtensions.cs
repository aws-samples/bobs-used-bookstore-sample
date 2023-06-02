namespace Bookstore.Web.Helpers
{
    public static class IntExtensions
    {
        public static string ToStorageSize(this int value)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            var order = 0;

            while (value >= 1024 && order < sizes.Length - 1)
            {
                order++;
                value /= 1024;
            }

            return string.Format("{0:0.##} {1}", value, sizes[order]);
        }
    }
}
