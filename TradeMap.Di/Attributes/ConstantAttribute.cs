using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConstantAttribute : Attribute
    {
        public string Name { get; }
        public Type Parser { get; }
        public string[] ParserArgs { get; }
        public Type? Validator { get; }
        public string[]? ValidatorArgs { get; }


        public ConstantAttribute(string name, Type parser, string[] parserArgs, Type validator, string[] validatorArgs)
        {
            Name = name;
            Parser = parser;
            ParserArgs = parserArgs;
            Validator = validator;
            ValidatorArgs = validatorArgs;
        }

        public ConstantAttribute(string name, Type parser, string[] parserArgs)
        {
            Name = name;
            Parser = parser;
            ParserArgs = parserArgs;
        }
    }
}
