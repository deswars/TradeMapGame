using System.Collections.Generic;

namespace MapGame.Core
{
    public class Cell
    {
        public TerrainType Terrain { get; private set; }

        public MovementBlockType MovementBlock { get; private set; }

        public double MovementDifficulty { get; private set; }

        public IReadOnlyList<IMapEntity> Entities
        {
            get
            {
                return _entities.AsReadOnly();
            }
        }

        public Cell(TerrainType terrain)
        {
            Terrain = terrain;
            _entities = new List<IMapEntity>();
            UpdateMovementDifficulty();
        }

        public void ChangeTerrain(TerrainType newTerrain)
        {
            Terrain = newTerrain;
            UpdateMovementDifficulty();
        }

        public void AddEntity(IMapEntity entity)
        {
            _entities.Add(entity);
            UpdateMovementDifficulty();
        }

        public void RemoveEntity(IMapEntity entity)
        {
            _entities.Remove(entity);
            UpdateMovementDifficulty();
        }


        private readonly List<IMapEntity> _entities;

        private void UpdateMovementBlock()
        {
            MovementBlock = Terrain.MovementBlock;
            foreach (var entity in _entities)
            {
                MovementBlock |= entity.MovementBlock;
            }
        }

        private void UpdateMovementDifficulty()
        {
            UpdateMovementBlock();
            MovementDifficulty = Terrain.GetMovementDifficulty();
            if (MovementBlock != MovementBlockType.None)
            {
                MovementDifficulty = double.PositiveInfinity;
            }
            else
            {
                foreach (var entity in Entities)
                {
                    MovementDifficulty += entity.GetMovementDifficulty();
                }

                if (MovementDifficulty < MapConfig.MinMovementDifficultyModifier)
                {
                    MovementDifficulty = MapConfig.MinMovementDifficultyModifier;
                }
            }
        }

    }
}
