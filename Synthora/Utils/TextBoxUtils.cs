using System.Linq;
using Avalonia;
using Avalonia.Input;

namespace Synthora.Utils
{
    internal class TextBoxUtils
    {
        public static KeyGesture? SelectAllGesture => Application.Current?.PlatformSettings?.HotkeyConfiguration.SelectAll.FirstOrDefault();
    }
}