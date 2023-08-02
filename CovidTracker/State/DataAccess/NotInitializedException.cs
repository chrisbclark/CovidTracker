using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidTracker.State.DataAccess
{
    public class NotInitializedException : Exception
    {
        public NotInitializedException(string message) : base(message)
        {
        }
    }
}
