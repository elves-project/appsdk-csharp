using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElvesApp
{
    public class Worker
    {
        public string flag = "true";

        public string helloword(Dictionary<string,string> param)
        {
            string result = "";
            flag = "true";
            try
            {
                result = param["my"];
            }
            catch (Exception e) {
                flag = "false";
                result = e.ToString();
            }
            return result;
        }

    }
}
