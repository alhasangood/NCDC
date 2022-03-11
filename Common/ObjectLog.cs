using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common
{
    public class Myobj
    {
        public List<int> Arr { get; }
    }
    public static class ObjectLog
    {
        public static string PrintPropreties(object obj)
        {
            try
            {
                string result = "";
                if (obj != null)
                {
                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
                    {
                        string name = descriptor.Name;
                        var value = descriptor.GetValue(obj);

                        if (value == null)
                        {
                            result = result + $", {name}= NULL";
                        }
                        else if (value is Base64FormattingOptions)
                        {
                            result = result + $", {name}=Base 64 String";
                        }
                        else
                        {
                            result = result + $", {name}={value}";
                        }
                    }
                    return result;
                }
                else
                {
                    return "Object Null : Can Not Log The Object";
                }
            }
            catch (Exception)
            {
                return "Exception : Can Not Log The Object";
            }
        }

    }
}
