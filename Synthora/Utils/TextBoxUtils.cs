using System.Linq;
using Avalonia;
using Avalonia.Input;

namespace Synthora.Utils
{
    public class TextBoxUtils
    {
        public static KeyGesture? SelectAllGesture => Application.Current?.PlatformSettings?.HotkeyConfiguration.SelectAll.FirstOrDefault();
    }
}