using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class MyException : Exception
    {
        public MyException() : base("Размер экрана некоректный") { }
        public MyException(string message) : base(message) { }
    }
}
