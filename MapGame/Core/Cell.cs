using System.Collections.Generic;

namespace MapGame.Core
{
    public class Cell
    {
        public TerraitType Terrain { get; private set; }
        public MoveClasses MoveClass { get; private set; }
        public IReadOnlyList<IMapEntity> Entities
        {
            get
            {
                return _entities.AsReadOnly();
            }
        }
        public double MoveModifier
        {
            get
            {
                double result = Terrain.MoveSpeed;
                if ((MoveClass & MoveClasses.GroundBlocked) != MoveClasses.FreeMovement)
                {
                    return double.PositiveInfinity;
                }

                foreach (var entity in Entities)
                {
                    result += entity.MoveSpeed;
                }
                if (result < MapConfig.MinSpeedModifier)
                {
                    result = MapConfig.MinSpeedModifier;
                }
                return result;
            }
        }

        public Cell(TerraitType terrain)
        {
            Terrain = terrain;
            MoveClass = Terrain.MoveClass;
            _entities = new List<IMapEntity>();
        }

        public void ChangeTerrain(TerraitType newTerrain)
        {
            Terrain = newTerrain;
            UpdateMoveClass();
        }

        public void AddEntity(IMapEntity entity)
        {
            _entities.Add(entity);
            UpdateMoveClass();
        }

        public void RemoveEntity(IMapEntity entity)
        {
            _entities.Remove(entity);
            UpdateMoveClass();
        }

        private readonly List<IMapEntity> _entities;

        private void UpdateMoveClass()
        {
            MoveClass = Terrain.MoveClass;
            foreach (var entity in _entities)
            {
                MoveClass &= entity.MoveClass;
            }
        }
    }
}
