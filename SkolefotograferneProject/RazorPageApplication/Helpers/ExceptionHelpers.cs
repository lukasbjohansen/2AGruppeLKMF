namespace RazorPageApplication.Helpers
{
    public static class ExceptionHelpers
    {
        public static void PrintWithType(this Exception ex)
        {
            Console.WriteLine(ex.GetType() + ": " + ex.Message);
        }

        public static string GetFullMessage(this Exception ex)
        {
            return ex.GetType() + ": " + ex.Message;
        }
    }
}
