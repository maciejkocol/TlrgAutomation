using System;

namespace Tlrg.Tests
{
    internal class TestCategoryAttribute : Attribute
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private string v;
#pragma warning restore IDE0044 // Add readonly modifier

        public TestCategoryAttribute(string v)
        {
            this.v = v;
        }
    }
}
