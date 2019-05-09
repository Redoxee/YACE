using System.Collections.Generic;

namespace YACE
{
    class YACE
    {

    }

    struct RessourcePool
    {
        public RessourceDefinition Definition;
    }

    public struct RessourceDefinition
    {
        public string Name;
        public int MinValue = int.MinValue;
        public int MaxValue = int.MaxValue;
        public int BaseValue = 0;

        public bool IsBoundToPlayer = true;
        public bool ConsumeOnUse = true;

        string[] ResetOnPhase = { };
    }
}
