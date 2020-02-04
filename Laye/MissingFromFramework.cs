#if NET472

using System.Collections.Generic;
using System.Text;

namespace System
{
    public readonly struct ReadOnlySpan<T>
    {
        // TODO(local): this needs to actually have start/length fields to avoid extra copies but for now this is fine
        private readonly T[] m_values;

        public ReadOnlySpan(T[] array)
        {
            m_values = new T[array.Length];
            Array.Copy(array, m_values, array.Length);
        }

        public ReadOnlySpan(T[] array, int start, int length)
        {
            m_values = new T[length];
            Array.Copy(array, start, m_values, 0, length);
        }

        public ref readonly T this[int index] => ref m_values[index];

        public int Length => m_values.Length;
        public bool IsEmpty => m_values.Length == 0;

        [Obsolete("Equals() on ReadOnlySpan will always throw an exception.")]
        public override bool Equals(object obj) => throw new NotImplementedException();
        [Obsolete("GetHashCode() on ReadOnlySpan will always throw an exception.")]
        public override int GetHashCode() => throw new NotImplementedException();
        public ReadOnlySpan<T> Slice(int start) => new ReadOnlySpan<T>(m_values, start, Length - start);
        public ReadOnlySpan<T> Slice(int start, int length) => new ReadOnlySpan<T>(m_values, start, length);
        public T[] ToArray()
        {
            var result = new T[m_values.Length];
            Array.Copy(m_values, result, m_values.Length);
            return result;
        }
        public override string ToString() => throw new NotImplementedException();
        //public bool TryCopyTo(Span<T> destination);

        public static bool operator ==(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
        {
            if (left.Length != right.Length) return false;
            for (int i = 0; i < left.Length; i++)
            {
                if (!object.Equals(left[i], right[i]))
                    return false;
            }
            return true;
        }
        public static bool operator !=(ReadOnlySpan<T> left, ReadOnlySpan<T> right) => !(left == right);

        public static implicit operator ReadOnlySpan<T>(T[] array) => new ReadOnlySpan<T>(array);
        //public static implicit operator ReadOnlySpan<T>(ArraySegment<T> segment);
    }
}

#endif