using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class HttpServer
{
    private HttpListener listener;
    private bool isRunning;

    public HttpServer(string prefix)
    {
        listener = new HttpListener();
        listener.Prefixes.Add(prefix); // مثال: "http://localhost:8080/"
    }

    public void Start()
    {
        if (isRunning)
            return;
        HttpHandlers.InitializeRoutes();
        isRunning = true;
        listener.Start();
        _ = ListenAsync(); // اجرای غیرهمزمان بدون انتظار
        Debug.Log("HTTP Server started, listening on " + listener.Prefixes);
    }

    public void Stop()
    {
        if (!isRunning)
            return;

        isRunning = false;
        listener.Stop();
    }

    private async Task ListenAsync()
    {
        try
        {
            while (isRunning)
            {
                var context = await listener.GetContextAsync();
                await ProcessRequestAsync(context); // اضافه کردن await به این خط
            }
        }
        catch (Exception ex)
        {
            if (isRunning)
            {
                Debug.LogError("HTTP Server encountered an error: " + ex.Message);
            }
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            await Task.Run(() => HttpHandlers.HandleRequest(context));
        }
        catch (Exception ex)
        {
            Debug.LogError("Error processing request: " + ex.Message);
        }
    }
}
