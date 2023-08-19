namespace Core.SharedKernel.AuditLogs;

public static class Keywords
{
    public static string For(FeatureFlag flag) => For(flag.Key, flag.Name);

    public static string For(Segment segment) => For(segment.Name);

    private static string For(params string[] keywords) => string.Join(',', keywords);
}
