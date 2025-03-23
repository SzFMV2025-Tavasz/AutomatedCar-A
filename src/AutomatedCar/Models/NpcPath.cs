using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{
    class NpcPath
    {
        public bool Loop { get; set; }

        public List<NpcPathPoint> Points { get; set; }

        public NpcType Type { get; set; }

        public string Filename { get; set; }
        public string World { get; set; }

        public NpcPath(NpcJsonObject pathJson)
        {
            this.Loop = pathJson.Loop;
            this.Points = pathJson.Points.Select(p => new NpcPathPoint(p)).ToList();
            this.Type = pathJson.Type;
            this.Filename = pathJson.Filename;
            this.World = pathJson.World;
        }
    }
}
