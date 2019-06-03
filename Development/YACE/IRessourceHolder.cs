namespace YACE
{
    interface IRessourceHolder
    {
        bool HasRessource(string ressource);
        int GetRessourcevalue(string ressource);
        void SetRessource(string ressource, int value);
        void AddRessource(string ressource, int delta);
    }
}
