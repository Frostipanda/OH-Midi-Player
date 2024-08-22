using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;

namespace OHMidiPlayer035
{
    public class VirtualKeys
    {
        private readonly Dictionary<VirtualKeyCode, Button> keyButtonMap;
        private readonly Dictionary<VirtualKeyCode, (Color BackColor, Color ForeColor)> originalColors;
        private readonly Color highlightColor = Color.SkyBlue;

        public VirtualKeys(Form1 form)
        {
            // Map VirtualKeyCodes to the corresponding buttons
            keyButtonMap = new Dictionary<VirtualKeyCode, Button>
            {
                { VirtualKeyCode.CONTROL, form.ControlButton },
                { VirtualKeyCode.SHIFT, form.ShiftButton },
                { VirtualKeyCode.VK_Q, form.Q },
                { VirtualKeyCode.VK_W, form.W },
                { VirtualKeyCode.VK_E, form.E },
                { VirtualKeyCode.VK_R, form.R },
                { VirtualKeyCode.VK_T, form.T },
                { VirtualKeyCode.VK_Y, form.Y },
                { VirtualKeyCode.VK_U, form.U },
                { VirtualKeyCode.VK_2, form.TWO },
                { VirtualKeyCode.VK_3, form.THREE },
                { VirtualKeyCode.VK_5, form.FIVE },
                { VirtualKeyCode.VK_6, form.SIX },
                { VirtualKeyCode.VK_7, form.SEVEN }
            };

            // Store the original colors
            originalColors = new Dictionary<VirtualKeyCode, (Color BackColor, Color ForeColor)>();
            foreach (var kvp in keyButtonMap)
            {
                originalColors[kvp.Key] = (kvp.Value.BackColor, kvp.Value.ForeColor);
            }
        }

        public async void HighlightKey(VirtualKeyCode key)
        {
            if (keyButtonMap.TryGetValue(key, out Button button))
            {
                button.BackColor = highlightColor;
                button.ForeColor = Color.Black; // Ensure text is readable

                // Keep the key highlighted for 500ms
                await Task.Delay(500);

                ResetKey(key); // Reset after delay
            }
        }

        public void ResetKey(VirtualKeyCode key)
        {
            if (keyButtonMap.TryGetValue(key, out Button button))
            {
                if (originalColors.TryGetValue(key, out var colors))
                {
                    button.BackColor = colors.BackColor;
                    button.ForeColor = colors.ForeColor;
                }
            }
        }

        public void ResetAllKeys()
        {
            foreach (var kvp in keyButtonMap)
            {
                if (originalColors.TryGetValue(kvp.Key, out var colors))
                {
                    kvp.Value.BackColor = colors.BackColor;
                    kvp.Value.ForeColor = colors.ForeColor;
                }
            }
        }
    }
}
