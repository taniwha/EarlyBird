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

using EarlyBird_KACWrapper;

namespace EarlyBird {

	[KSPAddon (KSPAddon.Startup.Flight, false)]
	public class EB_FlightWindow : MonoBehaviour
	{
		static Rect windowpos;
		private static bool gui_enabled;
		private static bool hide_ui;

		static EB_FlightWindow instance;
		public static double DawnOffset;

		private static void SetInputLock ()
		{
			InputLockManager.SetControlLock ("EB_Flight_window_lock");
		}

		private static void RemoveInputLock ()
		{
			InputLockManager.RemoveControlLock ("EB_Flight_window_lock");
		}

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
			if (!enabled) {
				RemoveInputLock ();
			}
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
		}

		void OnDestroy ()
		{
			instance = null;
			GameEvents.onHideUI.Remove (onHideUI);
			GameEvents.onShowUI.Remove (onShowUI);
		}

		static bool KACpresent = false;
		static bool KACinited = false;
		
		void Start ()
		{
			if (!KACinited) {
				KACinited = true;
				KACpresent = KACWrapper.InitKACWrapper();
			}
			UpdateGUIState ();
		}

		void SetKACAlarm (Vessel vessel, double offset)
		{
			double timeToDawn = EarlyBird.TimeToDaylight (vessel.latitude,
														  vessel.longitude,
														  vessel.mainBody,
														  DawnOffset);
			double alarmTime = Planetarium.GetUniversalTime () + timeToDawn;
			string alarmMessage = "Wake-up call for " + vessel.vesselName;
			KACWrapper.KAC.CreateAlarm (KACWrapper.KACAPI.AlarmTypeEnum.Raw,
										alarmMessage, alarmTime);
		}

		void WindowGUI (int windowID)
		{
			GUILayout.BeginVertical ();

			Vessel vessel = FlightGlobals.ActiveVessel;
			if (vessel.situation != Vessel.Situations.LANDED
				&& vessel.situation != Vessel.Situations.SPLASHED
				&& vessel.situation != Vessel.Situations.PRELAUNCH) {
				GUILayout.Label("mobile warp-to-morning not yet implemented");
				GUILayout.Label("Someday ;)");
			} else {
				EarlyBird_Settings.DawnOffset (ref DawnOffset);
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Warp to Morning")) {
					EarlyBird.WarpToMorning (vessel.latitude,
											 vessel.longitude,
											 vessel.mainBody,
											 DawnOffset);
				}
				if (KACpresent) {
					if (GUILayout.Button ("Add Alarm")) {
						SetKACAlarm (vessel, DawnOffset);
					}
				}
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Close")) {
					HideGUI ();
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.EndVertical ();
			GUI.DragWindow (new Rect (0, 0, 10000, 20));
		}

		void OnGUI ()
		{
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
					SetInputLock ();
				} else {
					RemoveInputLock ();
				}
			} else {
				RemoveInputLock ();
			}
		}
	}
}
