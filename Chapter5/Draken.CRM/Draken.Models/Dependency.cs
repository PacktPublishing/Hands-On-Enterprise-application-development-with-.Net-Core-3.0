using System;
using System.Collections.Generic;
using System.Text;

namespace Draken.Models
{
    public class A
    {
        public B B
        {
            get => default;
            set
            {
            }
        }

        public string GetX()
        {
            B b = new B();
            return b.X();
        }
    }

    public class B
    {
        public string X()
        {
            return "x";
        }
    }
}
