/*
	EarlyBird.cs

	Improved warp-to-morning.

	Copyright (C) 2017 Bill Currie <bill@taniwha.org>

	Author: Bill Currie <bill@taniwha.org>
	Date: 2017/6/24

	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public License
	as published by the Free Software Foundation; either version 2
	of the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	See the GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to:

		Free Software Foundation, Inc.
		59 Temple Place - Suite 330
		Boston, MA  02111-1307, USA

*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using KSP.UI;

namespace EarlyBird {

	[KSPAddon (KSPAddon.Startup.EveryScene, false)]
	public class EarlyBird : MonoBehaviour
	{
		internal static EarlyBird instance;

		public static double DawnOffset = -5;

		CelestialBody sun;

		void Awake ()
		{
			instance = this;
		}

		void OnDestroy ()
		{
			instance = null;
		}

		public static void WarpToMorning (double lat, double lon,
										  CelestialBody body, double offset)
		{
			if (FlightDriver.Pause) {
			 	return;
			}
			if (instance.sun == null) {
				Debug.LogError ("Cannot warp to next morning due to lack of sun");
				return;
			}

			if (TimeWarp.fetch.CancelAutoWarp (0)) {
				return;
			}

			double rotPeriod, localTime;

			localTime = Sunrise.GetLocalTime (lon, body, instance.sun);
			rotPeriod = body.rotationPeriod;
			if (body.orbit != null) {
				rotPeriod = body.orbit.period * rotPeriod / (body.orbit.period - rotPeriod);
			}

			offset = (offset * 60) / rotPeriod;
			double dayLength = Sunrise.GetDayLength (lat, body, instance.sun);
			double timeOfDawn = 0.5 - dayLength / 2 + offset;;
			double timeToDaylight = rotPeriod * UtilMath.WrapAround(timeOfDawn - localTime, 0, 1);
			Debug.LogFormat("[EarlyBird] WarpToMorning: daylight: {0}({1}), dawn {2}, warpTime: {3}", dayLength, dayLength * rotPeriod, timeOfDawn, timeToDaylight);
			TimeWarp.fetch.WarpTo (Planetarium.GetUniversalTime () + timeToDaylight, 8, 1);
		}

		void WarpToMorning ()
		{

			double lat = -0.0917535863160035;
			double lon = 285.37030688110428;
			CelestialBody body = FlightGlobals.GetHomeBody();
			if (SpaceCenter.Instance != null) {
				body = SpaceCenter.Instance.cb;
				lat = SpaceCenter.Instance.Latitude;
				lon = SpaceCenter.Instance.Longitude;
			}
			WarpToMorning (lat, lon, body, DawnOffset);
		}

/*
		void Window (int windowID)
		{
		}

		static Rect winpos;
		void OnGUI ()
		{
			winpos = GUILayout.Window (GetInstanceID (), winpos, Window,
									   "Early Bird",
									   GUILayout.MinWidth (200));
		}
*/

		IEnumerator HijackWarpToMorning ()
		{
			GameObject go;
			do {
				yield return null;
				go = GameObject.Find ("WarpToMorning");
			} while (go == null);
			var wtm = go.GetComponentInChildren<UIWarpToNextMorning>();
			wtm.button.onClick.RemoveAllListeners ();
			wtm.button.onClick.AddListener (WarpToMorning);
		}

		IEnumerator GetSun ()
		{
			// FIXME figure out how to support multiple suns (kopernicus)
			while (Planetarium.fetch == null) {
				yield return null;
			}
			sun = Planetarium.fetch.Sun;
			for (int i = 0; i < FlightGlobals.Bodies.Count; i++) {
				CelestialBody body = FlightGlobals.Bodies[i];
			}
		}

		void Start ()
		{
			enabled = false;
			switch (HighLogic.LoadedScene) {
				case GameScenes.SPACECENTER:
				case GameScenes.TRACKSTATION:
					StartCoroutine(HijackWarpToMorning ());
					enabled = true;
					break;
				case GameScenes.FLIGHT:
					enabled = true;
					break;
			}
			UIWarpToNextMorning.timeOfDawn = 0.235;
			StartCoroutine (GetSun ());
		}
	}
}
