namespace YACE.Path
{
    using System.Collections;

    public struct ContextPath
    {
        public PathNode[] PathNodes;
        
        public IEnumerable Evaluate(ITaggableHolder root)
        {
            PathNode currentNode = this.PathNodes[0];
            
            for (int nodeIndex = 0; nodeIndex < this.PathNodes.Length; ++nodeIndex)
            yield return null;
        }
    }

    public struct PathNode
    {

        public NodeType NodeType;
        public string Target;
    }

    public enum NodeType
    {
        HasTag,
        RessourceAdd,
        RessourceSet,
    }
}
