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

        private HotKeyManager? _hotKeyManager;

        public RegisterGlobalHotkey(GlobalHotKeys.Native.Types.VirtualKeyCode key, GlobalHotKeys.Native.Types.Modifiers modifiers, Action func)
        {
            _hotKeyManager = new HotKeyManager();

            s_hotkey = _hotKeyManager.Register(GlobalHotKeys.Native.Types.VirtualKeyCode.KEY_Q, GlobalHotKeys.Native.Types.Modifiers.Control);

            s_subscription = _hotKeyManager.HotKeyPressed
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(hotkey => func());
        }
    }
}