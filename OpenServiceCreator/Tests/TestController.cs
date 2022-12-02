using AssemblyLoader.Loader;
using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.ToolKit.Store;

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
        [Post("t1")]
        public Response TestMethod(Body body)
        {
            return new Response() { text = "Hello, " + body.Text };
        }

        [Post("t2")]
        public Response TestMethod2(Body body)
        {
            return new Response() { text = body.Text + " " + body.Text +" = "+ body.Age*4};
        }
    }

    public class Data551 : BaseEntity
    {
        public string Name { get; set; }
        public string Company { get; set; }
    }
}