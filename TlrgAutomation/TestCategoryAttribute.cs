using System;

namespace Tlrg.Tests
{
    internal class CategoryAttribute : Attribute
    {
        private string v;

        public CategoryAttribute(string v)
        {
            this.v = v;
        }
    }
}
