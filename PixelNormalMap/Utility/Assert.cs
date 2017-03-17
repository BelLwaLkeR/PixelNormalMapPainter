using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrameWork
{
    public class Assertion
    {
        public static void Assert(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition, message);
        }
    }
}
