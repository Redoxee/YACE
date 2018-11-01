namespace AMG.Framework
{
    using System.Collections.Generic;

    public abstract class Entity
    {
        public uint Id
        {
            get;
            internal set;
        }

        protected HashSet<int> Tags;

        public Entity()
        {
            EntitySystem.Register(this);
        }

        public void AddTag(int tag)
        {
            this.Tags.Add(tag);
            EntitySystem.RegisterTag(tag, this);
        }

        public void RemoveTag(int tag)
        {
            this.Tags.Remove(tag);
            EntitySystem.UnregisterTag(tag, this);
        }
    }
}
