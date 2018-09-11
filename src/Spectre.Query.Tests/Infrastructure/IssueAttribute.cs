using System;

namespace Spectre.Query.Tests.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class IssueAttribute : Attribute
    {
        public string Url { get; }

        public IssueAttribute(string url)
        {
            Url = url;
        }
    }
}