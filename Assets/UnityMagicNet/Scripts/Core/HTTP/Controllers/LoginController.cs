using System;
using System.IO;
using System.Net;
using System.Text;

public class LoginController
{
    public static bool HandleLogin(HttpListenerContext context)
    {
        var request = context.Request;

        if (request.HttpMethod == "POST")
        {
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                string body = reader.ReadToEnd();
                // منطق احراز هویت کاربر
                string token = AuthManager.GenerateAuthToken();

                // ارسال توکن به عنوان کوکی
                context.Response.AppendCookie(new Cookie("AuthToken", token));
            }
        }
        else
        {
            HttpHandlers.ServeFile(context, "/login.html");
        }

        return true;
    }
}
