using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Configuration;

namespace TrayGarden
{
    public class MockType : IRequireInitialization
    {
        public bool Initialized { get; set; }
        public int IntValue { get; set; }
        public bool BoolValue { get; set; }
        public string StrValue { get; set; }

        public object ObjValue { get; set; }

        public List<int> IntList { get; set; }
        public List<string> StrList { get; set; }
        public List<MockType> ObjList { get; set; }

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

        public bool MethodObj(object obj)
        {
            Calculated += obj.ToString();
            return true;
        }
    }
}