using System.Runtime.InteropServices;
using System.Windows.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using HandyControl.Controls;
using System.Reflection.PortableExecutable;
using GlobalHotKeys;
using System.Reactive.Linq;
using System.Threading;

namespace ScreenRecognition.Desktop.Core
{
    /// <summary>
    /// Это класс для добавления глобального хоткея
    /// </summary>
    public class RegisterGlobalHotkey
    {
        private static IDisposable? s_hotkey;
        private static IDisposable? s_subscription;

        private static HotKeyManager? s_hotKeyManager = new HotKeyManager();

        public RegisterGlobalHotkey(GlobalHotKeys.Native.Types.VirtualKeyCode key, GlobalHotKeys.Native.Types.Modifiers modifiers, Action func)
        {
            s_hotkey = s_hotKeyManager?.Register(GlobalHotKeys.Native.Types.VirtualKeyCode.KEY_Q, GlobalHotKeys.Native.Types.Modifiers.Control);

            s_subscription = s_hotKeyManager?.HotKeyPressed
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(hotkey => func());
        }

        public static void Dispose()
        {
            s_hotkey?.Dispose();
            s_subscription?.Dispose();
            s_hotKeyManager?.Dispose();
        }
    }
}