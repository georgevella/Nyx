using System.Reflection;

namespace Nyx.AppSupport.Wpf
{
    /// <summary>
    /// </summary>
    public static class In
    {
        public static Assembly ThisAssembly
        {
            get { return Assembly.GetCallingAssembly(); }
        }
    }
}