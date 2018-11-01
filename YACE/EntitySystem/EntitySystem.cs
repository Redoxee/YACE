
namespace AMG.Framework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class EntitySystem
    {
        private static uint NextId = 1;

        private static List<Entity> all;
        private static Dictionary<uint, Entity> allDictionaty;

        private static Dictionary<int, HashSet<uint>> tagPools;

        static EntitySystem()
        {
            all = new List<Entity>();
            allDictionaty = new Dictionary<uint, Entity>();
        }

        internal static void Register(Entity entity)
        {
            uint id = NextId++;
            entity.Id = id;
            all.Add(entity);
            allDictionaty[id] = entity;
        }

        internal static void RegisterTag(int tag, Entity target)
        {
            tagPools[tag].Add(target.Id);
        }

        internal static void UnregisterTag(int tag, Entity target)
        {
            tagPools[tag].Remove(target.Id);
        }

        public static void Initialize(int[] allTags)
        {
            tagPools.Clear();
            foreach (int tag in allTags)
            {
                tagPools[tag] = new HashSet<uint>();
            }
        }

        public static EntityCollection Select(int tag)
        {
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
                    yield return EntitySystem.allDictionaty[id];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public EntityCollection Select(int tag)
            {
                HashSet<uint> intersect = new HashSet<uint>(this.ids);
                intersect.IntersectWith(EntitySystem.tagPools[tag]);
                return new EntityCollection(intersect);
            }
        }
    }
}
