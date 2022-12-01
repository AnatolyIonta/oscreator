using System;
using Xunit;
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

            var controllers = service.GetCommand();
            
            return;
        }
    }

    [Controller("test")]
    public class TestController
    {
        [Command("t1")]
        public void Test1()
        {
            Console.WriteLine("++");
        }
        [Command("t2")]
        public void Test2()
        {
            Console.WriteLine("--");
        }
    }
}