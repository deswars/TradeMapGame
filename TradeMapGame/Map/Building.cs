using System.Linq;

namespace TradeMapGame.Map
{
    public class Building
    {
        public Settlement Owner { get; }
        public BuildingType Type { get; }

        public Building(Settlement owner, BuildingType type)
        {
            Owner = owner;
            Type = type;
        }

        public void Produce()
        {
            var resources = Owner.Resources;
            if (Type.Input.All(res => res.Value < resources[res.Key]))
            {
                foreach (var input in Type.Input)
                {
                    resources[input.Key] += input.Value;
                }
                foreach (var output in Type.Output)
                {
                    resources[output.Key] += output.Value;
                }
            }
        }
    }
}
