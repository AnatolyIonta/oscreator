using AssemblyLoader.Loader;
using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.ToolKit.Store;
using Ionta.OSC.ToolKit.ServiceProvider;
using System.Linq;

namespace OpenServiceCreator.Tests
{
    public class Response
    {
        public string text { get; set; }
    }

    public class Body
    {
        public string Text { get; set; }
        public int Age { get; set; }
    }
    
    [Controller("test")]
    public class TestController
    {
        [Post("t1")]
        public Response TestMethod(Body body)
        {
            return new Response() { text = "Hello, " + body.Text };
        }

        [Post("t2")]
        public Response TestMethod2(Body body)
        {
            return new Response() { text = body.Text + " " + body.Text +" = "+ body.Age*2};
        }
    }
    
    [Controller("user")]
    public class Test2Controller
    {
        private readonly ITestService _service;
        private readonly IDataStore _store;
        public Test2Controller(ITestService service, IDataStore store)
        {
            _service = service;
            _store = store;
        }

        [Post("t1")]
        public Response TestMethod(Body body)
        {
            return new Response() { text = "Hello, " + body.Text };
        }

        [Post("t2")]
        public Response TestMethod2(Body body)
        {
            return new Response() { text = _store.GetEntity<test8>().Count().ToString()};
        }
    }

    public interface ITestService
    {
        public string GetText();
    }

    [Service(typeof(ITestService), ServiceType.Scoped)]
    public class TestService : ITestService
    {
        public string GetText()
        {
            return "Hello world! I am Custom service!";
        }
    }

    public class Data551 : BaseEntity
    {
        public string Name { get; set; }
        public string Company { get; set; }
    }
    
    public class test6 : BaseEntity
    {
        public string Name { get; set; }
        public string Company { get; set; }
    }
    public class test7 : BaseEntity
    {
        public string Name { get; set; }
        public string X { get; set; }
    }
    public class test8 : BaseEntity
    {
        public string Name { get; set; }
        public string X { get; set; }
    }

}