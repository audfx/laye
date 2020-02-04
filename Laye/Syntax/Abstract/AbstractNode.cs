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
    }
}
