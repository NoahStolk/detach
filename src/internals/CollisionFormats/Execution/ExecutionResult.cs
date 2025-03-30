namespace CollisionFormats.Execution;

public sealed record ExecutionResult(object? ReturnValue, List<object> OutArguments);
