using System.Runtime.InteropServices;

namespace Capillume
{
    internal sealed class GlobalHotkeyManager : IDisposable
    {
        private const int HotkeyId = 0xCA11;
        private const int AvailabilityProbeId = 0xCA12;
        private const uint ModAlt = 0x0001;
        private const uint ModControl = 0x0002;
        private const uint ModShift = 0x0004;
        private const uint ModWin = 0x0008;
        private const int WmHotkey = 0x0312;

        private readonly IntPtr _windowHandle;
        private bool _isRegistered;
        private bool _disposed;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public GlobalHotkeyManager(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public bool IsRegistered => _isRegistered;
        public int HotkeyMessageId => HotkeyId;

        public bool TryRegister(HotkeySettings settings, out string error)
        {
            error = string.Empty;
            Unregister();

            if (!settings.Enabled)
            {
                return true;
            }

            if (!TryGetModifierFlags(settings.Modifiers, out uint modifierFlags)
                || settings.Key == Keys.None)
            {
                error = "Choose a shortcut with at least one modifier and a key.";
                return false;
            }

            if (!RegisterHotKey(_windowHandle, HotkeyId, modifierFlags, (uint)(settings.Key & Keys.KeyCode)))
            {
                error = "This shortcut is already in use by Windows or another application.";
                return false;
            }

            _isRegistered = true;
            return true;
        }

        public void Unregister()
        {
            if (!_isRegistered)
            {
                return;
            }

            UnregisterHotKey(_windowHandle, HotkeyId);
            _isRegistered = false;
        }

        public bool IsHotkeyMessage(ref Message message)
        {
            return message.Msg == WmHotkey && message.WParam == (IntPtr)HotkeyId;
        }

        public static bool IsAvailable(IntPtr windowHandle, Keys modifiers, Keys key)
        {
            if (!TryGetModifierFlags(modifiers, out uint modifierFlags) || key == Keys.None)
            {
                return false;
            }

            if (!RegisterHotKey(windowHandle, AvailabilityProbeId, modifierFlags, (uint)(key & Keys.KeyCode)))
            {
                return false;
            }

            UnregisterHotKey(windowHandle, AvailabilityProbeId);
            return true;
        }

        private static bool TryGetModifierFlags(Keys modifiers, out uint flags)
        {
            flags = 0;
            Keys normalizedModifiers = modifiers & Keys.Modifiers;

            if (normalizedModifiers == Keys.None)
            {
                return false;
            }

            if ((normalizedModifiers & Keys.Alt) != Keys.None)
            {
                flags |= ModAlt;
            }

            if ((normalizedModifiers & Keys.Control) != Keys.None)
            {
                flags |= ModControl;
            }

            if ((normalizedModifiers & Keys.Shift) != Keys.None)
            {
                flags |= ModShift;
            }

            if ((normalizedModifiers & Keys.LWin) != Keys.None)
            {
                flags |= ModWin;
            }

            return flags != 0;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Unregister();
            _disposed = true;
        }
    }
}
