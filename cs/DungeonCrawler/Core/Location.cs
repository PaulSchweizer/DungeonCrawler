
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DungeonCrawler.Core
{

    public class Cell
    {
        public static float GridSize = 3;
        public string Type;
        public string[] Tags;
        public int[] Position;

        public Cell()
        {
            Tags = new string[] { };
        }
    }

    public class Location
    {
        public string Name;
        public string Description;
        public string[] Tags;
        public Cell[] Cells;

        private int[] _bbox;

        public static Location DeserializeFromJson(string json)
        {
            Location location = JsonConvert.DeserializeObject<Location>(json);
            foreach (Cell cell in location.Cells)
            {
                List<string> tags = new List<string>();
                tags.AddRange(cell.Tags);
                foreach (string tag in location.Tags)
                {
                    if (!tags.Contains(tag))
                    {
                        tags.Add(tag);
                    }
                }
                cell.Tags = tags.ToArray();
            }
            return location;
        }

        public int[] BBox
        {
            get
            {
                if (_bbox == null)
                {
                    _bbox = new int[] { 0, 0, 0, 0 };
                    foreach (Cell cell in Cells)
                    {
                        if (cell.Position[0] < _bbox[0]) _bbox[0] = cell.Position[0];
                        else if (cell.Position[0] > _bbox[2]) _bbox[2] = cell.Position[0];
                        if (cell.Position[1] > _bbox[1]) _bbox[1] = cell.Position[1];
                        else if (cell.Position[1] < _bbox[3]) _bbox[3] = cell.Position[1];
                    }
                }
                return _bbox;
            }
        }

        public Cell CellAt(int x, int y)
        {
            x = (int)Math.Floor(x / Cell.GridSize);
            y = (int)Math.Ceiling(y / Cell.GridSize);
            foreach (Cell cell in Cells) {
                if (cell.Position[0] == x && cell.Position[1] == y)
                {
                    return cell;
                }
            }
            return null;
        }

        public override string ToString()
        {
            string cellText = "[{0}]";
            string empty = "   ";
            string repr = string.Format("{0}\n", Name);

            for (int y = BBox[1] * (int)Cell.GridSize; y >= (BBox[3]-1) * Cell.GridSize; y--)
            {
                for (int x = BBox[0] * (int)Cell.GridSize; x <= (BBox[2]+1) * Cell.GridSize; x++)
                {
                    Cell cell = CellAt(x, y);
                    if (cell != null)
                    {
                        repr += string.Format(cellText, cell.Type[0]);
                    }
                    else
                    {
                        repr += empty;
                    }
                }
                repr += "\n";
            }
            return repr;
        }
    }
}