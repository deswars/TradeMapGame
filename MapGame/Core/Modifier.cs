namespace MapGame.Core
{
    public class Modifier
    {
        public ModifierTypes Type { get; }
        public double Value { get; }

        public Modifier(ModifierTypes type, double value)
        {
            Type = type;
            Value = value;
        }
    }
}
