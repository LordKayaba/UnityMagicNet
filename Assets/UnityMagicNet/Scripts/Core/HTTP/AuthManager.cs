using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public class AuthManager
{
    private static Dictionary<string, DateTime> activeTokens = new Dictionary<string, DateTime>();
    private static TimeSpan tokenValidityDuration = TimeSpan.FromMinutes(30); // مدت اعتبار توکن (30 دقیقه)

    // تابع برای تولید یک توکن جدید
    public static string GenerateAuthToken()
    {
        string token = Guid.NewGuid().ToString();
        activeTokens[token] = DateTime.Now.Add(tokenValidityDuration);
        return token;
    }

    // تابع برای بررسی اعتبار توکن
    public static bool IsTokenValid(string token)
    {
        if (activeTokens.ContainsKey(token))
        {
            // بررسی اینکه آیا توکن هنوز معتبر است
            if (activeTokens[token] > DateTime.Now)
            {
                return true;
            }
            else
            {
                // حذف توکن منقضی شده
                activeTokens.Remove(token);
            }
        }
        return false;
    }

    // تابع برای تنظیم مدت اعتبار توکن
    public static void SetTokenValidityDuration(TimeSpan duration)
    {
        tokenValidityDuration = duration;
    }

    // تابع برای حذف توکن (به عنوان مثال هنگام لاگ‌اوت)
    public static void InvalidateToken(string token)
    {
        if (activeTokens.ContainsKey(token))
        {
            activeTokens.Remove(token);
        }
    }
}
