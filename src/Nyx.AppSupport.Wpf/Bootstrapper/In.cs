﻿using System.Reflection;

// ReSharper disable once CheckNamespace
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