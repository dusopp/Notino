using Notino.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Factories
{
    public class CommonFactory
    {
        public static T Get<T>(string className) 
        {
            Type type = typeof(T);
            Object obj = Activator.CreateInstance(type);
            return (T)obj;
        }
    }
}
