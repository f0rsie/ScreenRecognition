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
        public static IDisposable? s_hotkey;
        public static IDisposable? s_subscription;

        private static HotKeyManager? s_hotKeyManager { get; set; }

        public RegisterGlobalHotkey(GlobalHotKeys.Native.Types.VirtualKeyCode key, GlobalHotKeys.Native.Types.Modifiers modifiers, Action func)
        {
            s_hotKeyManager = new HotKeyManager();

            s_Hotkey = s_hotKeyManager.Register(GlobalHotKeys.Native.Types.VirtualKeyCode.KEY_Q, GlobalHotKeys.Native.Types.Modifiers.Control);

            s_Subscription = s_hotKeyManager.HotKeyPressed
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(hotkey => func());
        }

        public static void Dispose()
        {
            RegisterGlobalHotkey.s_Hotkey?.Dispose();
            RegisterGlobalHotkey.s_Subscription?.Dispose();
            RegisterGlobalHotkey.s_hotKeyManager?.Dispose();
        }
    }
}