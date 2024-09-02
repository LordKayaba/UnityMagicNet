using System.Collections.Generic;
using System;

public static class SessionManager
{
    private static Dictionary<string, string> sessionTokens = new Dictionary<string, string>();

    public static string CreateSession(string username)
    {
        string token = Guid.NewGuid().ToString();
        sessionTokens[token] = username;
        return token;
    }

    public static bool ValidateSession(string token)
    {
        return sessionTokens.ContainsKey(token);
    }

    public static void DestroySession(string token)
    {
        sessionTokens.Remove(token);
    }
}