namespace TrueStoryMVC.DataItems.Utils
{
    public static class Error
    {
        public static string UnknownEnum(string enumName) => $"Unknown {enumName} field";
        public static string InvalideRequest(string dataName) => $"Invalid {dataName} value";
    }
}
