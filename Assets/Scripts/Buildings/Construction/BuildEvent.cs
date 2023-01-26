namespace Simpolony.Buildings
{
    public struct BuildEvent
    {
        public BuildingData Data { get; private set; }
        public Building Building { get; private set; }
        public BuildingConstruction Construction { get; private set; }


        public BuildEvent(BuildingData data, Building building, BuildingConstruction construction)
        {
            this.Data = data;
            this.Building = building;
            this.Construction = construction;
        }
    }
}