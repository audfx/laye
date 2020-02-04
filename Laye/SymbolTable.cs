using System.Collections;
using System.Collections.Generic;

namespace Laye
{
    public interface ISymbolTableEntry { }

    public sealed class SymbolTable : ISymbolTableEntry, IEnumerable<ISymbolTableEntry>
    {
        public readonly string ScopeName;

        // stored in order, that's important
        private readonly List<ISymbolTableEntry> m_entries = new List<ISymbolTableEntry>();

        public SymbolTable(string scopeName = "<global>")
        {
            ScopeName = scopeName;
        }

        public void AddEntry(ISymbolTableEntry entry)
        {
            m_entries.Add(entry);
        }

        public IEnumerator<ISymbolTableEntry> GetEnumerator() => ((IEnumerable<ISymbolTableEntry>)m_entries).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ISymbolTableEntry>)m_entries).GetEnumerator();
    }
}
