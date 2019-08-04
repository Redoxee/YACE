namespace AMG.Entity
{
    using System.Collections.Generic;

    public abstract class Entity
    {
        private Dictionary<string, int> values;

        public uint Id
        {
            get;
            internal set;
        }

        protected HashSet<Tag> Tags;

        public Entity()
        {
            EntitySystem.Register(this);
        }

        public void AddTag(string name)
        {
            Tag tag = new Tag { Name = name };
            this.Tags.Add(tag);
            EntitySystem.RegisterTag(tag, this);
        }

        public void AddTag(string name, int value)
        {
            this.AddTag(name);
            this.values[name] = value;
        }

        public void RemoveTag(string tagName)
        {
            Tag tempTag = new Tag { Name = tagName };
            this.Tags.Remove(tempTag);
            EntitySystem.UnregisterTag(tempTag, this);
        }
    }
}
