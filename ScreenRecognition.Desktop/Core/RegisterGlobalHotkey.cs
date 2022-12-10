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
        public static IDisposable? s_Hotkey { get; set; }
        public static IDisposable? s_Subscription { get; set; }

        public static HotKeyManager? s_HotKeyManager { get; set; }

        public RegisterGlobalHotkey(GlobalHotKeys.Native.Types.VirtualKeyCode key, GlobalHotKeys.Native.Types.Modifiers modifiers, Action func)
        {
            s_HotKeyManager = new HotKeyManager();

            s_Hotkey = s_HotKeyManager.Register(GlobalHotKeys.Native.Types.VirtualKeyCode.KEY_Q, GlobalHotKeys.Native.Types.Modifiers.Control);

            s_Subscription = s_HotKeyManager.HotKeyPressed
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(hotkey => func());
        }
    }
}