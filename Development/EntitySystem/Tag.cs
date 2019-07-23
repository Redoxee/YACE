
namespace AMG.Entity
{
    public struct Tag 
    {
        public string Name;
        public int Value;

        public override bool Equals(object obj)
        {
            if (obj is Tag)
            {
                return this.Name == ((Tag)obj).Name;
            }
            else if (obj is string)
            {
                return this.Name == (string)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
