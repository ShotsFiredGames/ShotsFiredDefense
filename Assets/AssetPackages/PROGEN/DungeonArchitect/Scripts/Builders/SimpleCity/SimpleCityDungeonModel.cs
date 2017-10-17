using UnityEngine;
using System.Collections;
using DungeonArchitect;
using DungeonArchitect.Utils;

namespace DungeonArchitect.Builders.SimpleCity
{

    public enum SimpleCityCellType
    {
        Road,
        House,
        House2X,
        Park,
        CityWallPadding,
        Empty
    }

    public class SimpleCityCell
    {
        public IntVector Position;
        public SimpleCityCellType CellType;
        public Quaternion Rotation;
    }

    public class SimpleCityDungeonModel : DungeonModel
    {
        [HideInInspector]
        public SimpleCityCell[,] Cells = new SimpleCityCell[0, 0];

        [HideInInspector]
        public SimpleCityCell[] WallPaddingCells;

        [HideInInspector]
        public SimpleCityDungeonConfig Config;

        [HideInInspector]
        public int CityWidth;

        [HideInInspector]
        public int CityHeight;

    }

}