namespace App.Extensions;

public static class StringExtensions
{
    public static bool IsValidYoutubeId(this string str)
    {
        return str.Length == 11 && str.All(char.IsLetterOrDigit);
    }
}
