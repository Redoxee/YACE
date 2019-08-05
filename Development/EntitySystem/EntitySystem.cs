
namespace AMG.Entity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class EntitySystem
    {
        private static uint NextId = 1;

        private static List<Entity> all;
        private static Dictionary<uint, Entity> allEntities;

        private static Dictionary<string, HashSet<uint>> tagPools;

        static EntitySystem()
        {
            all = new List<Entity>();
            allEntities = new Dictionary<uint, Entity>();
            tagPools = new Dictionary<string, HashSet<uint>>();
        }

        internal static void Register(Entity entity)
        {
            uint id = NextId++;
            entity.Id = id;
            all.Add(entity);
            allEntities[id] = entity;
        }

        internal static void RegisterTag(string tag, Entity target)
        {
            if (!tagPools.ContainsKey(tag))
            {
                tagPools.Add(tag, new HashSet<uint>());
            }

            tagPools[tag].Add(target.Id);
        }

        internal static void UnregisterTag(string tag, Entity target)
        {
            if (!tagPools.ContainsKey(tag))
            {
                tagPools.Add(tag, new HashSet<uint>());
            }

            tagPools[tag].Remove(target.Id);
        }
        
        public static EntityCollection Select(string tag)
        {

            if (!tagPools.ContainsKey(tag))
            {
                tagPools.Add(tag, new HashSet<uint>());
            }

            return new EntityCollection(tagPools[tag]);
        }

        public class EntityCollection : IEnumerable<Entity>
        {
            private HashSet<uint> ids;
            internal EntityCollection(HashSet<uint> ids)
            {
                this.ids = ids;
            }

            public IEnumerator<Entity> GetEnumerator()
            {
                foreach (uint id in this.ids)
                {
                    yield return EntitySystem.allEntities[id];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public EntityCollection Select(string tag)
            {
                HashSet<uint> intersect = new HashSet<uint>(this.ids);
                intersect.IntersectWith(EntitySystem.Select(tag).ids);
                return new EntityCollection(intersect);
            }
        }
    }
}
