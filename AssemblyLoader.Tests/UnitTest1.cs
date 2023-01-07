using System;
using Xunit;
using Ionta.OSC.ToolKit.Controllers;

namespace AssemblyLoader.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            return;
        }
    }

    [Controller("test")]
    public class TestController
    {
        [Post("t1")]
        public void Test1()
        {
            Console.WriteLine("++");
        }
        [Post("t2")]
        public void Test2()
        {
            Console.WriteLine("--");
        }
    }
}