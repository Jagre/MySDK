using MySDK.MongoDB.Models;
using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace MySDK.MongoDB.Test
{
    public class Fee : MongoEntityBase
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class ExpressionParserTest
    {
        [Fact]
        public void Test1()
        {
            var member = new Stack<string>();
            var method = new Stack<int>();

            var repo = new SortExpressionParser();
            repo.Parsing<Fee>(i => i.OrderBy(j => j.Name).ThenBy(j => j.Age).ThenByDescending(j => j.Email), member, method);

            Assert.Equal(method.Count, (int)3);
        }

        [Fact]
        public void Test2()
        {
            var member = new Stack<string>();
            var method = new Stack<int>();

            var repo = new SortExpressionParser();
            repo.Parsing<Fee>(i => i.OrderBy(j => new { j.Name, j.Age, j.Email }), member, method);

            Assert.Equal(method.Count, (int)3);

        }
    }
}
