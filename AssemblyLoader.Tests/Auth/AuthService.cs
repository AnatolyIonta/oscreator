using Xunit;
using Ionta.OSC.Core.Auth;
using Microsoft.VisualBasic;
using System;

namespace AssemblyLoader.Tests.Auth
{
    
    public class AuthService
    {
        [Fact]
        public void TestGenerateToken()
        {
            var service = new AuthenticationService();
            var result = service.GenerateToken();
            Assert.False(String.IsNullOrEmpty(result));
        }

        [Fact]
        public void TestValidateToken()
        {
            var service = new AuthenticationService();
            var token = service.GenerateToken();

            Assert.True(service.ValidateToken(token));
            Assert.False(service.ValidateToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6IkFkbWluQE9TQy5ydSIsIlN1YiI6IjEiLCJleHAiOjE2NzQ0MTQzNjYsImlzcyI6Ik9TQyIsImF1ZCI6Ik9TQyJ9.w1lv6-ZIo4uQy_GLqqeW5l_waQdZ38K6BcAfuFtZZFg"));
        }
    }
}