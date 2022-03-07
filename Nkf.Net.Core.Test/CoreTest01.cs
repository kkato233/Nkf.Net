using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Nkf.Net.Core.Test
{
    [TestClass]
    public class CoreTest01
    {
        [TestMethod]
        public void GetVersion()
        {
            string ver = Nkf.Net.WrapNkf.GetNkfVersion();
            Console.WriteLine($"ver {ver}");
        }
    }
}
