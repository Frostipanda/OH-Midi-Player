using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Native;

namespace OHMidiPlayer035
{
    public static class MidiKeyMap
    {
        private static readonly Dictionary<int, List<VirtualKeyCode>> _qwertyMap = new Dictionary<int, List<VirtualKeyCode>>
        {
            { 48, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_Q } },  // C3
            { 49, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_2 } },  // C#3
            { 50, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_W } },  // D3
            { 51, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_3 } },  // D#3
            { 52, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_E } },  // E3
            { 53, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_R } },  // F3
            { 54, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_5 } },  // F#3
            { 55, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_T } },  // G3
            { 56, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_6 } },  // G#3
            { 57, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_Y } },  // A3
            { 58, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_7 } },  // A#3
            { 59, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_U } },  // B3

            { 60, new List<VirtualKeyCode> { VirtualKeyCode.VK_Q } },  // C4
            { 61, new List<VirtualKeyCode> { VirtualKeyCode.VK_2 } },  // C#4
            { 62, new List<VirtualKeyCode> { VirtualKeyCode.VK_W } },  // D4
            { 63, new List<VirtualKeyCode> { VirtualKeyCode.VK_3 } },  // D#4
            { 64, new List<VirtualKeyCode> { VirtualKeyCode.VK_E } },  // E4
            { 65, new List<VirtualKeyCode> { VirtualKeyCode.VK_R } },  // F4
            { 66, new List<VirtualKeyCode> { VirtualKeyCode.VK_5 } },  // F#4
            { 67, new List<VirtualKeyCode> { VirtualKeyCode.VK_T } },  // G4
            { 68, new List<VirtualKeyCode> { VirtualKeyCode.VK_6 } },  // G#4
            { 69, new List<VirtualKeyCode> { VirtualKeyCode.VK_Y } },  // A4
            { 70, new List<VirtualKeyCode> { VirtualKeyCode.VK_7 } },  // A#4
            { 71, new List<VirtualKeyCode> { VirtualKeyCode.VK_U } },  // B4

            { 72, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_Q } },  // C5
            { 73, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_2 } },  // C#5
            { 74, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_W } },  // D5
            { 75, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_3 } },  // D#5
            { 76, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_E } },  // E5
            { 77, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_R } },  // F5
            { 78, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_5 } },  // F#5
            { 79, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_T } },  // G5
            { 80, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_6 } },  // G#5
            { 81, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_Y } },  // A5
            { 82, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_7 } },  // A#5
            { 83, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_U } },  // B5
        };

        private static readonly Dictionary<int, List<VirtualKeyCode>> _azertyMap = new Dictionary<int, List<VirtualKeyCode>>
        {
            { 48, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_A } },  // C3
            { 49, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.OEM_2 } },  // C#3 - Shift + 2 = é
            { 50, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_Z } },  // D3
            { 51, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.OEM_3 } },  // D#3 - Shift + 3 = "
            { 52, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_E } },  // E3
            { 53, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.OEM_4 } },  // F3 - Shift + 4 = '
            { 54, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.OEM_6 } },  // F#3 - Shift + 6 = -
            { 55, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_T } },  // G3
            { 56, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.OEM_7 } },  // G#3 - Shift + 7 = è
            { 57, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_Y } },  // A3
            { 58, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.OEM_8 } },  // A#3 - Shift + 8 = _
            { 59, new List<VirtualKeyCode> { VirtualKeyCode.LCONTROL, VirtualKeyCode.VK_U } },  // B3

            { 60, new List<VirtualKeyCode> { VirtualKeyCode.VK_A } },  // C4
            { 61, new List<VirtualKeyCode> { VirtualKeyCode.OEM_2 } },  // C#4 - Shift + 2 = é
            { 62, new List<VirtualKeyCode> { VirtualKeyCode.VK_Z } },  // D4
            { 63, new List<VirtualKeyCode> { VirtualKeyCode.OEM_3 } },  // D#4 - Shift + 3 = "
            { 64, new List<VirtualKeyCode> { VirtualKeyCode.VK_E } },  // E4
            { 65, new List<VirtualKeyCode> { VirtualKeyCode.OEM_4 } },  // F4 - Shift + 4 = '
            { 66, new List<VirtualKeyCode> { VirtualKeyCode.OEM_6 } },  // F#4 - Shift + 6 = -
            { 67, new List<VirtualKeyCode> { VirtualKeyCode.VK_T } },  // G4
            { 68, new List<VirtualKeyCode> { VirtualKeyCode.OEM_7 } },  // G#4 - Shift + 7 = è
            { 69, new List<VirtualKeyCode> { VirtualKeyCode.VK_Y } },  // A4
            { 70, new List<VirtualKeyCode> { VirtualKeyCode.OEM_8 } },  // A#4 - Shift + 8 = _
            { 71, new List<VirtualKeyCode> { VirtualKeyCode.VK_U } },  // B4

            { 72, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_A } },  // C5
            { 73, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_2 } },  // C#5 - Shift + 2 = é
            { 74, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_Z } },  // D5
            { 75, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_3 } },  // D#5 - Shift + 3 = "
            { 76, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_E } },  // E5
            { 77, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_4 } },  // F5 - Shift + 4 = '
            { 78, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_6 } },  // F#5 - Shift + 6 = -
            { 79, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_T } },  // G5
            { 80, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_7 } },  // G#5 - Shift + 7 = è
            { 81, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_Y } },  // A5
            { 82, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.OEM_8 } },  // A#5 - Shift + 8 = _
            { 83, new List<VirtualKeyCode> { VirtualKeyCode.LSHIFT, VirtualKeyCode.VK_U } },  // B5
        };

        private static Dictionary<int, List<VirtualKeyCode>> _currentMap = _qwertyMap;

        public static void SetLayout(string layout)
        {
            if (layout == "QWERTY")
            {
                _currentMap = _qwertyMap;
            }
            else if (layout == "AZERTY")
            {
                _currentMap = _azertyMap;
            }
        }

        public static IEnumerable<KeyValuePair<int, List<VirtualKeyCode>>> MidiToKeyMapEnumerable => _currentMap;
        public static Dictionary<int, List<VirtualKeyCode>> MidiToKey => _currentMap;
        public static bool ContainsKey(int midiKey) => _currentMap.ContainsKey(midiKey);
    }
}
