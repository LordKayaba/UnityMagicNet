using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public class AuthManager
{
    static Dictionary<string, DateTime> activeTokens = new Dictionary<string, DateTime>();
    static TimeSpan tokenValidityDuration = TimeSpan.FromMinutes(30);

    public static string GenerateAuthToken()
    {
        string token = Guid.NewGuid().ToString();
        activeTokens[token] = DateTime.Now.Add(tokenValidityDuration);
        return token;
    }

    public static bool IsTokenValid(string token)
    {
        if (activeTokens.ContainsKey(token))
        {
            if (activeTokens[token] > DateTime.Now)
            {
                return true;
            }
            else
            {
                activeTokens.Remove(token);
            }
        }
        return false;
    }

    public static void SetTokenValidityDuration(TimeSpan duration)
    {
        tokenValidityDuration = duration;
    }

    public static void InvalidateToken(string token)
    {
        if (activeTokens.ContainsKey(token))
        {
            activeTokens.Remove(token);
        }
    }
}
