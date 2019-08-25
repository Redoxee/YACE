namespace AMG.Entity
{
    using System.Collections.Generic;

    public abstract class Entity
    {
        protected Dictionary<string, int> Tags;

        public uint Id
        {
            get;
            internal set;
        }

        public Entity()
        {
            this.Tags = new Dictionary<string, int>();
            EntitySystem.Register(this);
        }

        public void AddTag(string tag, int value = int.MinValue)
        {
            if (this.Tags.ContainsKey(tag))
            {
                this.Tags[tag] += value;
            }
            else
            {
                this.Tags[tag] = value;
                EntitySystem.RegisterTag(tag, this);
            }
        }

        public int GetValue(string tag)
        {
            if (this.Tags.ContainsKey(tag))
            {
                return this.Tags[tag];
            }

            return int.MinValue;
        }

        public void RemoveTag(string tag)
        {
            if (this.Tags.ContainsKey(tag))
            {
                this.Tags.Remove(tag);
                EntitySystem.UnregisterTag(tag, this);
            }
        }

        public bool HasTag(string tag)
        {
            return this.Tags.ContainsKey(tag);
        }

        public string GetTagsString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach(var kvp in this.Tags)
            {
                stringBuilder.Append(kvp.Key);
                if (kvp.Value != int.MinValue)
                {
                    stringBuilder.Append("= ").Append(kvp.Value);
                }
                stringBuilder.Append(",");
            }

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append("[").Append(this.GetTagsString()).Append("]");

            return stringBuilder.ToString();
        }
    }
}
