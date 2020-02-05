using System;
using System.Collections.Generic;

namespace Laye.Syntax.Abstract
{
    public sealed class AbstractTreePrinter : AbstractSyntaxVisitor
    {
        private int m_tabs = 0;

        private void WriteTabs()
        {
            for (int i = 0; i < m_tabs; i++)
                Console.Write("    ");
        }

        public override void VisitDefault(AbstractNode node)
        {
            WriteTabs();
            Console.WriteLine(node.GetType().Name);
        }

        public override void Visit(ANodeListType node)
        {
            WriteTabs();
            Console.WriteLine("List Type:");

            m_tabs++;
            node.ElementType.Accept(this);
            m_tabs--;
        }

        public override void Visit(ANodePointerType node)
        {
            WriteTabs();
            Console.WriteLine("Pointer Type:");

            m_tabs++;
            node.Pointee.Accept(this);
            m_tabs--;
        }

        public override void Visit(ANodeBuiltInType node)
        {
            WriteTabs();
            Console.WriteLine($"BuiltIn Type: {node.BuiltInType}");
        }

        public override void Visit(ANodeBindingDecl node)
        {
            WriteTabs();
            Console.WriteLine("Binding Declaration:");

            m_tabs++;
            node.DeclType.Accept(this);
            WriteTabs();
            Console.WriteLine(node.Name.Image);
            node.Value?.Accept(this);
            m_tabs--;
        }
    }
}
