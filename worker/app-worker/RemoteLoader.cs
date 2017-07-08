using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace agentProxy
{
    public class RemoteLoader : MarshalByRefObject
    {
        private Assembly assembly;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void LoadAssembly(string fullName)
        {
            assembly = Assembly.LoadFrom(fullName);
        }
        public Object Invoke(string type, string method, params Object[] args)
        {
            Type tp = assembly.GetType(type);
            Object obj = Activator.CreateInstance(tp);
            MethodInfo meth = tp.GetMethod(method);
            return meth.Invoke(obj, args);
        }

    }

    public class LocalLoader
    {
        private AppDomain appDomain;
        private RemoteLoader remoteLoader;

        public LocalLoader()
        {
            AppDomainSetup setup = new AppDomainSetup();
            appDomain = AppDomain.CreateDomain("TestDomain", null, setup);
            string name = Assembly.GetExecutingAssembly().GetName().FullName;
            remoteLoader = (RemoteLoader)appDomain.CreateInstanceAndUnwrap(name, typeof(RemoteLoader).FullName);
        }

        public void LoadAssembly(string fullName)
        {
            remoteLoader.LoadAssembly(fullName);
        }

        public void Unload()
        {
            AppDomain.Unload(appDomain);
            appDomain = null;
        }

        public string Invoke(string type, string method, params Object[] args)
        {
            return remoteLoader.Invoke(type, method, args).ToString();
        }

    }
}
