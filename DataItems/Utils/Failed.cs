namespace TrueStoryMVC.DataItems.Utils
{
    public static class Failed
    {
        public static string ToFindUser() => "User not found";
        public static string ToFindPost(int id) => $"Post {id} not found";
    }
}
