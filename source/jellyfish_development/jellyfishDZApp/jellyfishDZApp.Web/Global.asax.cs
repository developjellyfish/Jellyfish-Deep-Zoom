using System;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

/// <summary>
/// Global Class
/// </summary>
public class Global : HttpApplication
{
    /// <summary>
    /// Handles the Start event of the Application control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Application_Start(Object sender, EventArgs e)
    {
        PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        object o = p.GetValue(null, null);
        FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
        object monitor = f.GetValue(o);
        MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
        m.Invoke(monitor, new object[] { });
    }

    /// <summary>
    /// Handles the BeginRequest event of the Application control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Handles the Start event of the Session control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Session_Start(Object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Handles the End event of the Session control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Session_End(Object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Handles the EndRequest event of the Application control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Application_EndRequest(Object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Handles the End event of the Application control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Application_End(Object sender, EventArgs e)
    {
    }
}