using PapajVZ.Interfaces;

namespace PapajVZ.Helpers
{
    public static class ExtensionsMethods
    {
        public static string UppercaseFirst(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : char.ToUpper(value[0]) + value.Substring(1);
        }


    }
}