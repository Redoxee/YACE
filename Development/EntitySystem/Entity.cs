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

        public Entity()
        {
            this.values = new Dictionary<string, int>();
            EntitySystem.Register(this);
        }

        public void AddTag(string tag, int value = int.MinValue)
        {
            if (this.values.ContainsKey(tag))
            {
                this.values[tag] += value;
            }
            else
            {
                this.values[tag] = value;
                EntitySystem.RegisterTag(tag, this);
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
            if (this.values.ContainsKey(tag))
            {
                this.values.Remove(tag);
                EntitySystem.UnregisterTag(tag, this);
            }
        }

        public bool HasTag(string tag)
        {
            return this.values.ContainsKey(tag);
        }

        public string GetTagsString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach(var kvp in this.values)
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
