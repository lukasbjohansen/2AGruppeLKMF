namespace RazorPageApplication.Helpers
{
    public static class ExceptionHelpers
    {
        public static void PrintWithType(this Exception ex)
        {
            Console.WriteLine(ex.GetType() + ": " + ex.Message);
        }
    }
}
