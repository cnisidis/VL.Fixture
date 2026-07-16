using Fixtures.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures
{
    public class PatchManager
    {
        // Dictionary of Universe Index -> 512-byte DMX Buffer
        private readonly Dictionary<int, byte[]> _universes = new();
        private readonly List<FixtureInstance> _patchedFixtures = new();

        public IReadOnlyList<FixtureInstance> PatchedFixtures => _patchedFixtures;

        public void PatchFixture(FixtureInstance fixture, int universe, int startAddress)
        {
            // 1. Collision check: make sure this address block is free in the universe
            int neededChannels = fixture.ActiveMode.ChannelCount;
            foreach (var existing in _patchedFixtures)
            {
                if (existing.Universe == universe)
                {
                    bool overlaps = (startAddress <= existing.StartAddress + existing.ActiveMode.ChannelCount - 1) &&
                                    (existing.StartAddress <= startAddress + neededChannels - 1);
                    if (overlaps)
                    {
                        throw new InvalidOperationException($"Patch collision! Cannot patch {fixture.Name} to address {startAddress}. Overlaps with {existing.Name}.");
                    }
                }
            }

            fixture.Universe = universe;
            fixture.StartAddress = startAddress;
            _patchedFixtures.Add(fixture);

            if (!_universes.ContainsKey(universe))
            {
                _universes[universe] = new byte[512];
            }
        }

        /// <summary>
        /// Processes all patched fixtures and updates the physical DMX universe arrays.
        /// </summary>
        public void UpdateDmxBuffers()
        {
            // Clear all universe buffers
            foreach (var buffer in _universes.Values)
            {
                Array.Clear(buffer, 0, buffer.Length);
            }

            // Bake the active fixture attribute states into raw bytes
            foreach (var fixture in _patchedFixtures)
            {
                if (_universes.TryGetValue(fixture.Universe, out var buffer))
                {
                    fixture.WriteToDmxBuffer(buffer);
                }
            }
        }

        public byte[] GetUniverseBuffer(int universe)
        {
            return _universes.TryGetValue(universe, out var buffer) ? buffer : null;
        }
    }
}
