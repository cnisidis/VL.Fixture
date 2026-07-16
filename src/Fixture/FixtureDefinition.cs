using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fixture
{
    public class FixtureDefinition
    {
        public FixtureDefinition()
        {

        }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }
        public List<GenericDmxMode> Modes { get; } = new();
    }

    public class GenericDmxMode
    {
        public string Name { get; set; }
        public int ChannelCount { get; set; }

        public int Size { get; set; }
        public List<GenericChannel> Channels { get; } = new();
    }

    public enum DmxResolution
    {
        bit8 = 1,      // 1 DMX Channel (0-255)
        bit16 = 2,    // 2 DMX Channels (Coarse/Fine, 0-65535)
        bit24 = 3,  // 3 DMX Channels (0-16777215)
        bit32 =4
    }

    public class GenericChannel
    {
        public string AttributeName { get; set; } // e.g., "Pan", "Tilt", "Dimmer", "ColorAdd_R"
        /// <summary>
        /// In GDTF context, this corresponds to Geometry, it is the most accurate and valid way to name and address different logical channels sharing the same Attribute. 
        /// </summary>
        public string TaregtComponent { get; set; }
        /// <summary>
        /// Channel's relative offset (1, 2, 4, etc)
        /// </summary>
        public int RelativeOffset { get; set; }    // 0-based index within the mode
        /// <summary>
        /// DMX Precision in bits 8 / 16 / 24 /32
        /// </summary>
        public DmxResolution Resolution { get; set; } = DmxResolution.bit8;

        // Default physical ranges to map normalized 0.0 - 1.0 values
        public float PhysicalMin { get; set; } = 0f;
        public float PhysicalMax { get; set; } = 1f;
        public float DefaultValue { get; set; } = 0f;
        /// <summary>
        /// Physical unit (ie angle, colour etc)
        /// </summary>
        public string PhysicalUnit { get; set; } = string.Empty;

        public Dictionary<string, int> Functions { get; set; } = new();


    }
}
