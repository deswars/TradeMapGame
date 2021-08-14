using Newtonsoft.Json.Linq;
using TradeMap.Core;

namespace TradeMap.Di
{
    public enum TrySetError
    {
        None = 0,
        ParseError = 1,
        ValidationError = 2
    }

    public class Constant
    {
        public string ServiceName { get; }
        public string Name { get; }
        public object? Value { get; private set; }
        public IValidator? Validator { get; }
        public IParserJson Parser { get; }


        public Constant(string serviceName, string name, IParserJson parser, IValidator? validator)
        {
            ServiceName = serviceName;
            Name = name;
            Validator = validator;
            Parser = parser;
        }


        public TrySetError TrySetValue(JToken token, ITypeRepository repository)
        {
            var converted = Parser.TryParse(token, repository, out var newValue);
            if (!converted)
            {
                return TrySetError.ParseError;
            }
            if ((Validator != null) && (!Validator.Validate(newValue!, repository)))
            {
                return TrySetError.ValidationError;
            }
            Value = newValue!;
            return TrySetError.None;
        }
    }
}
