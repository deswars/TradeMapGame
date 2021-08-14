using System;
using System.Globalization;
using System.IO;
using TradeMap.Configuration;
using TradeMap.Core;
using TradeMap.Di;
using TradeMap.Engine.Map;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var localizer = new BasicTextLocalizer(CultureInfo.InvariantCulture);
            var log = new GameLogInMemory();
            log.SetInfoLevel(InfoLevels.All);
            var conf = new ConfigurationLoader(log);
            var map = new SquareMap(100, 100, conf.TypeRepository.TerrainTypes["t_null"]);
            var eng = new MapEngine(map, log, 2);
            var di = new Manager(log, conf.TypeRepository, eng);

            di.FindAllAvailableServices();

            var feautres = conf.LoadFeautres(new FileInfo("C:\\Users\\deswars\\source\\repos\\MapGame\\TradeMap.TestConsole\\Examples\\feautres.json"));
            Console.WriteLine("###FEAUTRES###");
            foreach (var x in feautres)
            {
                Console.Write($"[{x.Key}]=");
                foreach (var y in x.Value)
                {
                    var registered = di.TryRegisterTurnAction(y.SubturnType, y.ServiceName, y.FeautreName, x.Key);
                    Console.Write($"{y} {registered},");
                }
                Console.WriteLine();
            }


            conf.AddConfRoot(new DirectoryInfo("C:\\Users\\deswars\\source\\repos\\MapGame\\TradeMap.TestConsole\\Examples"));
            conf.LoadPackages();

            var repository = conf.TypeRepository;
            Console.WriteLine("###RESOURCES###");
            foreach (var resource in repository.ResourceTypes)
            {
                Console.WriteLine(resource);
            }
            Console.WriteLine("###TERRAINS###");
            foreach (var terrain in repository.TerrainTypes)
            {
                Console.WriteLine(terrain);
            }
            Console.WriteLine("###FEAUTRES###");
            foreach (var feautre in repository.TerrainFeautreTypes)
            {
                Console.WriteLine(feautre);
            }
            Console.WriteLine("###COLLECTORS###");
            foreach (var collector in repository.CollectorTypes)
            {
                Console.WriteLine(collector);
            }
            Console.WriteLine("###BUILDINGS###");
            foreach (var building in repository.BuildingTypes)
            {
                Console.WriteLine(building);
            }
            Console.WriteLine("###DEMANDS###");
            foreach (var demandTier in repository.PopulationDemands)
            {
                Console.Write(demandTier.Key + ":");
                foreach (var demand in demandTier.Value)
                {
                    Console.Write(demand + ", ");
                }
                Console.WriteLine();
            }


            var constants = di.CollectConstantDemands();
            conf.SetupConstants(constants);
            di.CreateServicesAndSubscribeActions(constants);

            eng.NextTurn();
            eng.NextTurn();

            Console.WriteLine("###ERRORS###");
            foreach (var entry in log.Entries)
            {
                Console.WriteLine(entry.ToText(localizer));
            }
            Console.WriteLine("#####END#####");
        }
    }
}
