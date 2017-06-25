/*
This file is part of Extraplanetary Launchpads.

Extraplanetary Launchpads is free software: you can redistribute it and/or
modify it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Extraplanetary Launchpads is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Extraplanetary Launchpads.  If not, see
<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

using KSP.IO;

namespace EarlyBird {

	[KSPAddon (KSPAddon.Startup.Flight, false)]
	public class EB_FlightWindow : MonoBehaviour
	{
		static Rect windowpos;
		private static bool gui_enabled;
		private static bool hide_ui;

		static EB_FlightWindow instance;

		public static void ToggleGUI ()
		{
			gui_enabled = !gui_enabled;
			if (instance != null) {
				instance.UpdateGUIState ();
			}
		}

		public static void HideGUI ()
		{
			gui_enabled = false;
			if (instance != null) {
				instance.UpdateGUIState ();
			}
		}

		public static void ShowGUI ()
		{
			gui_enabled = true;
			if (instance != null) {
				instance.UpdateGUIState ();
			}
		}

		void UpdateGUIState ()
		{
			enabled = !hide_ui && gui_enabled;
		}

		void onHideUI ()
		{
			hide_ui = true;
			UpdateGUIState ();
		}

		void onShowUI ()
		{
			hide_ui = false;
			UpdateGUIState ();
		}

		public void Awake ()
		{
			instance = this;
			GameEvents.onHideUI.Add (onHideUI);
			GameEvents.onShowUI.Add (onShowUI);
			enabled = false;
		}
		
		void Start ()
		{
			enabled = false;
		}

		void WindowGUI (int windowID)
		{
			GUILayout.BeginVertical ();

			GUILayout.EndVertical ();
			GUI.DragWindow (new Rect (0, 0, 10000, 20));
		}

		void OnGUI ()
		{
			if (enabled) { // don't do any work at all unless we're enabled
				if (gui_enabled) { // don't create windows unless we're going to show them
					GUI.skin = HighLogic.Skin;
					if (windowpos.x == 0) {
						windowpos = new Rect (Screen.width / 2 - 250,
							Screen.height / 2 - 30, 0, 0);
					}
					string name = "Early Bird";
					string ver = EarlyBirdVersionReport.GetVersion ();
					windowpos = GUILayout.Window (GetInstanceID (),
						windowpos, WindowGUI,
						name + " " + ver,
						GUILayout.Width (500));
					if (windowpos.Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y))) {
						InputLockManager.SetControlLock ("EB_Flight_window_lock");
					} else {
						InputLockManager.RemoveControlLock ("EB_Flight_window_lock");
					}
				} else {
					InputLockManager.RemoveControlLock ("EB_Flight_window_lock");
				}
			}
		}
	}
}
