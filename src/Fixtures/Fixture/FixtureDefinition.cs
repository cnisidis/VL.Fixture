using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures.Fixture
{
    public class FixtureDefinition
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public List<GenericDmxMode> Modes { get; } = new();
    }

    public class GenericDmxMode
    {
        public string Name { get; set; }
        public int ChannelCount { get; set; }
        public List<GenericChannel> Channels { get; } = new();
    }

    public enum DmxResolution
    {
        EightBit = 1,      // 1 DMX Channel (0-255)
        SixteenBit = 2,    // 2 DMX Channels (Coarse/Fine, 0-65535)
        TwentyFourBit = 3  // 3 DMX Channels (0-16777215)
    }

    public class GenericChannel
    {
        public string AttributeName { get; set; } // e.g., "Pan", "Tilt", "Dimmer", "ColorAdd_R"
        public int RelativeOffset { get; set; }    // 0-based index within the mode
        public DmxResolution Resolution { get; set; } = DmxResolution.EightBit;

        // Default physical ranges to map normalized 0.0 - 1.0 values
        public float PhysicalMin { get; set; } = 0f;
        public float PhysicalMax { get; set; } = 1f;
        public float DefaultValue { get; set; } = 0f;
    }
}
