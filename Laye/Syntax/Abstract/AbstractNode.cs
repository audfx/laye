using System;
using System.Collections.Generic;
using System.Text;

namespace Laye.Syntax.Abstract
{
    public abstract class AbstractNode
    {
        public (int Line, int Column) Location;

        protected AbstractNode((int, int) location)
        {
            Location = location;
        }

        public abstract void Accept(AbstractSyntaxVisitor visitor);
        public abstract T Accept<T>(AbstractSyntaxVisitor<T> visitor);
    }
}
