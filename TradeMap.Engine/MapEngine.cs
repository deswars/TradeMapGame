using System.Collections.Generic;
using TradeMap.Core.Action;
using TradeMap.Di.Attributes;
using TradeMap.Engine.Map;
using TradeMap.GameLog;

namespace TradeMap.Core
{
    public class MapEngine : ITurnManager
    {
        public SquareMap Map { get; }
        public int Turn { get; private set; }
        public IGameLog Log { get; }
        public int SubstepCount { get; }


        private readonly List<StartUpSubstep> _startUpSubstep = new();
        private readonly List<SettlementSubstep>[] _settlementSubsteps;
        private readonly List<GlobalSubstep>[] _globalSubsteps;


        public MapEngine(SquareMap map, IGameLog log, int subTurnCount)
        {
            Map = map;

            Turn = 0;
            Log = log;
            _settlementSubsteps = new List<SettlementSubstep>[subTurnCount];
            _globalSubsteps = new List<GlobalSubstep>[subTurnCount];
            for (int i = 0; i < subTurnCount; i++)
            {
                _settlementSubsteps[i] = new List<SettlementSubstep>();
                _globalSubsteps[i] = new List<GlobalSubstep>();
            }
            SubstepCount = subTurnCount;
        }

        public IReadOnlyList<IMapAction> NextTurn()
        {
            List<IMapAction> result = new();
            if (Turn == 0)
            {
                foreach (var substep in _startUpSubstep)
                {
                    substep(Map);
                }
            }
            else
            {
                for (int i = 0; i < SubstepCount; i++)
                {
                    var stepSettlement = _settlementSubsteps[i];
                    foreach (var settlement in Map.SettlementsMutable)
                    {
                        foreach (var step in stepSettlement)
                        {
                            var action = step(Turn, settlement);
                            if (action != null)
                            {
                                action.Execute(settlement);
                                result.Add(action);
                            }
                        }
                    }

                    var stepGlobal = _globalSubsteps[i];
                    foreach (var step in stepGlobal)
                    {
                        var action = step(Turn, Map);
                        if (action != null)
                        {
                            action.Execute(Map);
                            result.Add(action);
                        }
                    }
                }
            }
            Turn++;
            return result;
        }

        [SubturnType(SubturnTypes.StartUp)]
        public void RegisterStartUpSubstep(StartUpSubstep feautre)
        {
            _startUpSubstep.Add(feautre);
        }

        [SubturnType(SubturnTypes.Settlement)]
        public void RegisterSettlementSubstep(SettlementSubstep feautre, int substep)
        {
            if (substep < SubstepCount)
            {
                _settlementSubsteps[substep].Add(feautre);
            }
        }

        [SubturnType(SubturnTypes.Global)]
        public void RegisterGlobalSubstep(GlobalSubstep feautre, int substep)
        {
            if (substep < SubstepCount)
            {
                _globalSubsteps[substep].Add(feautre);
            }
        }
    }
}
