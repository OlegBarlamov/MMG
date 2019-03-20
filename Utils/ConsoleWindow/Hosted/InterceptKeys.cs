﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ConsoleWindow.Hosted
{
	internal class InterceptKeys : IDisposable
	{
		public event EventHandler<Key> KeyPressed;

		private const int WH_KEYBOARD_LL = 13;
		private const int WM_KEYDOWN = 0x0100;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly LowLevelKeyboardProc _callBack;
		private readonly IntPtr _hookID;

		public InterceptKeys()
		{
			_callBack = HookCallback;
			_hookID = SetHook(_callBack);
		}

		public void Dispose()
		{
			UnhookWindowsHookEx(_hookID);
		}

		private IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
				return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
					GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr) WM_KEYDOWN)
			{
				int vkCode = Marshal.ReadInt32(lParam);
				KeyPressed?.Invoke(this, (Key) vkCode);
			}

			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod,
			uint dwThreadId);


		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);


		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);
	}
}