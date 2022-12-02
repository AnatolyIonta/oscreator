using System;
using Xunit;
using Ionta.OSC.ToolKit.Controllers;
using AssemblyLoader.Loader;

namespace AssemblyLoader.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var service = new Loader.AssemblyManager();
            service.InitAssembly(this.GetType().Assembly);

            var controllers = service.GetControllers();
            
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