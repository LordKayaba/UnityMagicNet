using System.Net;

public static class DashboardController
{
    public static bool HandleDashboard(HttpListenerContext context)
    {
        HttpHandlers.ServeFile(context, "/dashboard.html");
        return true;
    }
}
