using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class HttpHandlers
{
    private static Dictionary<string, Func<HttpListenerContext, bool>> routeTable;

    public static void InitializeRoutes()
    {
        routeTable = new Dictionary<string, Func<HttpListenerContext, bool>>();
        routeTable.Add("/", LoginController.HandleLogin);
        routeTable.Add("/login", LoginController.HandleLogin);
        routeTable.Add("/dashboard", DashboardController.HandleDashboard);
    }

    public static void HandleRequest(HttpListenerContext context)
    {
        string url = context.Request.Url.AbsolutePath.ToLower();

        if (routeTable.TryGetValue(url, out Func<HttpListenerContext, bool> handler))
        {
            handler.Invoke(context);
        }
        else
        {
            ServeFile(context, url);
        }
    }

    public static void ServeFile(HttpListenerContext context, string fileName)
    {
        string filePath = $"{Path.Combine(Application.streamingAssetsPath)}/WebViews{fileName}";
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if(fileName == "/login.html" || extension != ".html" || IsAuthToken(context))
            {
                try
                {
                    byte[] buffer = File.ReadAllBytes(filePath);
                    context.Response.ContentType = GetMimeType(fileName);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error serving file: " + ex.Message);
                    SendErrorResponse(context, 500);
                }
                context.Response.OutputStream.Close();
            }
            else
            {
                ServeFile(context, "/login.html");
            }
        }
        else
        {
            SendErrorResponse(context, 404);
        }
    }

    static bool IsAuthToken(HttpListenerContext context)
    {
        CookieCollection cookies = context.Request.Cookies;
        for (int i = 0; i < cookies.Count; i++)
        {
            if(cookies[i].Name == "AuthToken" && AuthManager.IsTokenValid(cookies[i].Value))
            {
                return true;
            }
        }
        return false;
    }


    public static void SendErrorResponse(HttpListenerContext context, int statusCode)
    {
        string filePath = $"{Path.Combine(Application.streamingAssetsPath)}/WebViews/{statusCode}.html";
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            try
            {
                byte[] buffer = File.ReadAllBytes(filePath);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "text/html";
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error serving file: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("ErrorCode - Not Found");
        }

        context.Response.OutputStream.Close();
    }

    static string GetMimeType(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        switch (extension)
        {
            case ".html":
            case ".htm": return "text/html";
            case ".css": return "text/css";
            case ".js": return "application/javascript";
            case ".png": return "image/png";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            case ".gif": return "image/gif";
            default: return "application/octet-stream";
        }
    }
}
