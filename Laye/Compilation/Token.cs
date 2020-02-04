namespace Laye.Compilation
{
    public abstract class Token
    {
        public (int Line, int Column) Location;

        protected Token((int, int) location)
        {
            Location = location;
        }
    }
}
