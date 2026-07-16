using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixture
{
    public interface IFixture
    {

    }
    public class FixtureInstance
    {
        public string Name { get; set; }
        public FixtureDefinition Definition { get; }
        public GenericDmxMode ActiveMode { get; private set; }

        // Patch Properties
        public int Universe { get; set; }      // 0-based or 1-based universe
        public int StartAddress { get; set; }    // 1-based DMX address (1 to 512)

        // Current high-level attribute states (Normalized 0.0 to 1.0)
        private readonly Dictionary<string, float> _attributeValues = new(StringComparer.OrdinalIgnoreCase);

        public FixtureInstance(FixtureDefinition definition)
        {
            
            this.Definition = definition;
        }

        public void SetMode(string modeName)
        {
            ActiveMode = Definition.Modes.Find(m => m.Name.Equals(modeName, StringComparison.OrdinalIgnoreCase))
                ?? throw new ArgumentException($"Mode '{modeName}' not found in definition.");

            // Initialize default values
            _attributeValues.Clear();
            foreach (var channel in ActiveMode.Channels)
            {
                _attributeValues[channel.AttributeName] = channel.DefaultValue;
            }
        }

        // High-level API to assign values automatically (0.0 to 1.0)
        public void SetAttribute(string attributeName, float normalizedValue)
        {
            _attributeValues[attributeName] = Math.Clamp(normalizedValue, 0f, 1f);
        }

        public float GetAttribute(string attributeName)
        {
            return _attributeValues.TryGetValue(attributeName, out var val) ? val : 0f;
        }

        /// <summary>
        /// Automatically translates the internal state and writes raw bytes to the DMX universe buffer.
        /// </summary>
        public void WriteToDmxBuffer(byte[] universeBuffer)
        {
            
            if (ActiveMode == null) return;

            foreach (var channel in ActiveMode.Channels)
            {
                float value = GetAttribute(channel.AttributeName);

                // Calculate absolute start byte index in the universe array (0-based)
                int dmxStartIndex = (StartAddress - 1) + channel.RelativeOffset;

                // Ensure we don't overflow the physical universe limit (512 bytes)
                if (dmxStartIndex + (int)channel.Resolution > universeBuffer.Length) continue;

                // Map the normalized value to the byte resolution
                uint maxDmxValue = (1U << ((int)channel.Resolution * 8)) - 1;
                uint dmxRawValue = (uint)Math.Round(value * maxDmxValue);

                // Write bytes in Big-Endian (Coarse first, Fine second)
                for (int i = 0; i < (int)channel.Resolution; i++)
                {
                    int shift = ((int)channel.Resolution - 1 - i) * 8;
                    universeBuffer[dmxStartIndex + i] = (byte)((dmxRawValue >> shift) & 0xFF);
                }
            }
        }

        public void ReadFromDmxBuffer(Spread<byte> universeBytes)
        {
            var size = this.ActiveMode?.ChannelCount;
            var offset = this.StartAddress;
            if (size.HasValue)
            {
                var fixtureBytes = universeBytes.Skip(offset).Take((int)size);
                foreach(var channel in ActiveMode.Channels)
                {
                    var chanres = channel.Resolution;
                    var chanoffset = channel.RelativeOffset;


                }

            }



        }

    }
}
