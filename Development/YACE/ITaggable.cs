namespace YACE
{
    using System.Collections.Generic;

    public interface ITaggable
    {
        bool HasTag(string tag);
        bool AddTag(string tag);
        bool RemoveTag(string tag);
    }

    public interface ITaggableHolder
    {
        IEnumerable<ITaggable> Select(string tag);
    }
}
