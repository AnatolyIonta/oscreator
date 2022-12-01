using AssemblyLoader.Loader;

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
        [Command("t1")]
        public Response TestMethod(Body body)
        {
            return new Response() { text = "Hello, " + body.Text };
        }

        [Command("t2")]
        public Response TestMethod2(Body body)
        {
            return new Response() { text = body.Text + " " + body.Text +" = "+ body.Age*2};
        }
    }
    
    [Controller("user")]
    public class Test2Controller
    {
        [Command("t1")]
        public Response TestMethod(Body body)
        {
            return new Response() { text = "Hello, " + body.Text };
        }

        [Command("t2")]
        public Response TestMethod2(Body body)
        {
            return new Response() { text = body.Text + " " + body.Text +" = "+ body.Age*4};
        }
    }
}