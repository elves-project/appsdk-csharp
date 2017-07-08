using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace agentProxy
{
    class Program
    {

        public static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                agentExec(args[0], args[1]);
            }
            else if (args.Length == 3)
            {
                agentExec(args[0], args[1], args[2]);
            }
            else
            {
                agentExec("param error", "false");
            }
        }

        public static void agentExec(string app, string func, string jsonParam)
        {
            string returnValue = "";
            string flag = "false";
            try
            {
                //string WantedPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Substring(0, System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Length - 5);
                Assembly ass = Assembly.LoadFrom(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + app + ".dll");
                Type t = ass.GetType("ElvesApp.Worker");
                var obj = Activator.CreateInstance(t);
                Dictionary<string, string> at = new Dictionary<string, string>();
                byte[] bpath = Convert.FromBase64String(jsonParam);
                jsonParam = System.Text.ASCIIEncoding.Default.GetString(bpath);
                Dictionary<string, string> param = (Dictionary<string, string>)JsonConvert.DeserializeObject(jsonParam, at.GetType());
                returnValue = t.InvokeMember(func, BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, obj, new object[] { param }) as string;
                flag = t.InvokeMember("flag", BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance, null, obj, new object[] { }) as string;
            }
            catch (Exception e)
            {
                returnValue = e.ToString();
            }
            elvesPrint(flag, returnValue);
        }

        public static void agentExec(string app, string func)
        {
            string returnValue = "";
            string flag = "false";
            try
            {
                Assembly ass = Assembly.LoadFrom(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + app + ".dll");
                Type t = ass.GetType("ElvesApp.Worker");
                var obj = Activator.CreateInstance(t);
                returnValue = t.InvokeMember(func, BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, obj, new object[] { null }) as string;
                flag = t.InvokeMember("flag", BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance, null, obj, new object[] { }) as string;
            }
            catch (Exception e)
            {
                returnValue = e.ToString();
            }
            elvesPrint(flag, returnValue);
        }

        public static void elvesPrint(string flag, string result)
        {
            Console.WriteLine("<ElvesWFlag>" + flag + "</ElvesWFlag> <ElvesWResult>" + result + "</ElvesWResult>");
        }

    }
}
