namespace RazorPageApplication;

public static class Secret
{
	// INSERT CONNECTION STRING HERE...
	private static string _connectionString = "";
	public static string ConnectionString { get { return _connectionString; } }
}
