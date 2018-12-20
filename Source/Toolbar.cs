/*
This file is part of Early Bird.

Early Bird is free software: you can redistribute it and/or
modify it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Early Bird is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Early Bird.  If not, see
<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;

namespace EarlyBird {

	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class EB_AppButton : MonoBehaviour
	{
		const ApplicationLauncher.AppScenes buttonScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.TRACKSTATION;
		private static ApplicationLauncherButton button = null;

		public static Callback Toggle = delegate {};

		static bool buttonVisible
		{
			get {
				return true;
			}
		}

		public static void UpdateVisibility ()
		{
			if (button != null) {
				button.VisibleInScenes = buttonVisible ? buttonScenes : 0;
			}
		}

		private void onToggle ()
		{
			Toggle();
		}

		public void Start()
		{
			GameObject.DontDestroyOnLoad(this);
			GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);
		}

		void OnDestroy()
		{
			GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIAppLauncherReady);
		}

		void OnGUIAppLauncherReady ()
		{
			if (ApplicationLauncher.Ready && button == null) {
				var tex = GameDatabase.Instance.GetTexture("EarlyBird/Textures/EarlyBird_icon", false);
				button = ApplicationLauncher.Instance.AddModApplication(onToggle, onToggle, null, null, null, null, buttonScenes, tex);
				UpdateVisibility ();
			}
		}
	}

	[KSPAddon (KSPAddon.Startup.TrackingStation, false)]
	public class EBToolbar_TrackingStation : MonoBehaviour
	{
		public void Awake ()
		{
			EB_AppButton.Toggle += EarlyBird_Settings.ToggleGUI;
		}

		void OnDestroy()
		{
			EB_AppButton.Toggle -= EarlyBird_Settings.ToggleGUI;
		}
	}

	[KSPAddon (KSPAddon.Startup.Flight, false)]
	public class EBToolbar_Flight : MonoBehaviour
	{
		public void Awake ()
		{
			EB_AppButton.Toggle += EB_FlightWindow.ToggleGUI;
		}

		void OnDestroy()
		{
			EB_AppButton.Toggle -= EB_FlightWindow.ToggleGUI;
		}
	}

	[KSPAddon (KSPAddon.Startup.SpaceCentre, false)]
	public class EBToolbar_SpaceCenter : MonoBehaviour
	{
		public void Awake ()
		{
			EB_AppButton.Toggle += EarlyBird_Settings.ToggleGUI;
		}

		void OnDestroy()
		{
			EB_AppButton.Toggle -= EarlyBird_Settings.ToggleGUI;
		}
	}
}
