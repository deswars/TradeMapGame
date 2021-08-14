using TradeMap.Core;
using TradeMap.Di.Attributes;

namespace TradeMap.Di.Validators
{
    [SupportedType(typeof(string))]
    public class ValidatorId : IValidator
    {
        private readonly string _type;


        public ValidatorId(string[] args)
        {
            _type = args[0];
        }


        public bool Validate(object value, ITypeRepository repo)
        {
            string id = (string)value;
            return _type switch
            {
                "resource" => repo.ResourceTypes.ContainsKey(id),
                "terrain" => repo.TerrainTypes.ContainsKey(id),
                "feautre" => repo.TerrainFeautreTypes.ContainsKey(id),
                "collector" => repo.CollectorTypes.ContainsKey(id),
                "building" => repo.BuildingTypes.ContainsKey(id),
                _ => false,
            };
        }

        public override string ToString()
        {
            return $"{nameof(ValidatorId)}+{_type}";
        }
    }
}
