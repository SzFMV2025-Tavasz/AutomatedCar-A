using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.Models
{

    public class NpcJsonObject
    {
        public bool Loop { get; set; }

        public string World { get; set; }

        public List<NpcPointJsonObject> Points { get; set; }

        public NpcType Type { get; set; }

        public string Filename { get; set; }

    }
}
