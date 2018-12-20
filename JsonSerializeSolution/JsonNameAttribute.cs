using System;
using System.Collections.Generic;
using System.Text;

namespace JsonSerializeSolution
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class MyJsonNameAttribute : Attribute
    {
        readonly string jsonName;
        
        public MyJsonNameAttribute(string jsonName)
        {
            this.jsonName = jsonName;
        }

        public string JSonName
        {
            get { return jsonName; }
        }
    }
}
