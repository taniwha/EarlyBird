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
	[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] {
			GameScenes.SPACECENTER,
			GameScenes.FLIGHT,
			GameScenes.TRACKSTATION,
		})
	]
	public class EB_Settings : ScenarioModule
	{
		static Rect windowpos;
		private static bool gui_enabled;

		public static EB_Settings current
		{
			get {
				var game = HighLogic.CurrentGame;
				return game.scenarios.Select (s => s.moduleRef).OfType<EB_Settings> ().SingleOrDefault ();
			}
		}

		public override void OnLoad (ConfigNode config)
		{
			//Debug.Log (String.Format ("[EarlyBird] Settings load"));
			var settings = config.GetNode ("Settings");
			if (settings == null) {
				settings = new ConfigNode ("Settings");
				gui_enabled = true; // Show settings window on first startup
			}

			if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
				enabled = true;
			}
		}

		public override void OnSave(ConfigNode config)
		{
			//Debug.Log (String.Format ("[EarlyBird] Settings save: {0}", config));
			var settings = new ConfigNode ("Settings");
			config.AddNode (settings);
		}
		
		public override void OnAwake ()
		{
			enabled = false;
		}

		public static void ToggleGUI ()
		{
			gui_enabled = !gui_enabled;
		}

		void WindowGUI (int windowID)
		{
			GUILayout.BeginVertical ();

			if (GUILayout.Button ("OK")) {
				gui_enabled = false;
				InputLockManager.RemoveControlLock ("EB_Settings_window_lock");
			}
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
						InputLockManager.SetControlLock ("EB_Settings_window_lock");
					} else {
						InputLockManager.RemoveControlLock ("EB_Settings_window_lock");
					}
				} else {
					InputLockManager.RemoveControlLock ("EB_Settings_window_lock");
				}
			}
		}
	}
}
