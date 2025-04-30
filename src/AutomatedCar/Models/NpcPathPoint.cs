using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{

    public class NpcPathPoint

    {
        public int X { get; set; }

        public int Y { get; set; }

        public double Rotation { get; set; }

        public double Speed { get; set; }

        public NpcPathPoint(NpcPointJsonObject pointJson)
        {
            this.X = pointJson.X;
            this.Y = pointJson.Y;
            this.Rotation = pointJson.Rotation;
            this.Speed = pointJson.Speed;
        }
    }
}
