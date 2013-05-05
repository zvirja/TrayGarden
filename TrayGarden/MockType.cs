using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Configuration;

namespace TrayGarden
{
    public class MockType 
    {
        public bool Initialized { get; set; }
        public int IntValue { get; set; }
        public bool BoolValue { get; set; }
        public string StrValue { get; set; }

        public object ObjValue { get; set; }

        public List<int> IntList { get; set; }
        public List<string> StrList { get; set; }
        public List<MockType> ObjList { get; set; }

        public List<MockType> NullList { get; set; }

        public string Calculated { get; set; }

        public void Initialize()
        {
            Initialized = true;
        }

        public MockType()
        {
            IntList = new List<int>();
            StrList = new List<string>();
            Calculated = string.Empty;
            ObjList = new List<MockType>();
        }

        public void MethodInt(int val)
        {
            Calculated += val;
        }

        public void MethodStr(string str)
        {
            Calculated += str;
        }

        public bool Method1(string someValue, object obj)
        {
            Calculated += string.Format("{{{0} - {1}}}", someValue, obj.ToString());
            return true;
        }

        public bool Method2(int someValue,object obj)
        {
            Calculated += string.Format("{{int{0} - {1}}}",someValue,obj.ToString());
            return true;
        }


    }
}