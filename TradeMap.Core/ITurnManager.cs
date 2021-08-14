namespace TradeMap.Core
{
    public interface ITurnManager
    {
        public int SubstepCount { get; }


        void RegisterGlobalSubstep(GlobalSubstep feautre, int substep);
        void RegisterSettlementSubstep(SettlementSubstep feautre, int substep);
        void RegisterStartUpSubstep(StartUpSubstep feautre);
    }
}