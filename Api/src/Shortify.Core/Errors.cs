namespace Shortify.Core;

public static class Errors
{
	public static Error MissingCreatedBy => new("Missing_value", "Created by is required");
}