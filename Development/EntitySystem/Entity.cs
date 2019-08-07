namespace AMG.Entity
{
    using System.Collections.Generic;

    public abstract class Entity
    {
        private HashSet<string> tags;
        private Dictionary<string, int> values;

        public uint Id
        {
            get;
            internal set;
        }

        public Entity()
        {
            this.tags = new HashSet<string>();
            this.values = new Dictionary<string, int>();
            EntitySystem.Register(this);
        }

        public void AddTag(string tag)
        {
            this.tags.Add(tag);
            EntitySystem.RegisterTag(tag, this);
        }

        public void AddTag(string tag, int value)
        {
            if (!this.tags.Contains(tag))
            {
                this.AddTag(tag);
                this.values[tag] = value;
            }
            else
            {
                if (this.values.ContainsKey(tag))
                {
                    this.values[tag] += value;
                }
                else
                {
                    this.values[tag] = value;
                }
            }
        }

        public int GetValue(string tag)
        {
            if (this.values.ContainsKey(tag))
            {
                return this.values[tag];
            }

            return int.MinValue;
        }

        public void RemoveTag(string tag)
        {
            this.tags.Remove(tag);
            EntitySystem.UnregisterTag(tag, this);
        }

        public bool HasTag(string tag)
        {
            return this.tags.Contains(tag);
        }

        public string GetTagsString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach (string tag in this.tags)
            {
                stringBuilder.Append(tag);
                if (this.values.ContainsKey(tag))
                {
                    stringBuilder.Append("= ").Append(this.values[tag]);
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
