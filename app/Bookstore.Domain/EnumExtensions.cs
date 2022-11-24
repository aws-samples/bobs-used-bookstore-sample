using System.ComponentModel;
using System.Reflection;

namespace Bookstore.Domain
{
    public static class EnumExtensions
    {
        // https://stackoverflow.com/a/55338553
        public static string GetDescription(this Enum value)
        {
            return value.GetType()
                   .GetMember(value.ToString())
                   .First()
                   .GetCustomAttribute<DescriptionAttribute>()?
                   .Description ?? value.ToString();
        }
    }
}
