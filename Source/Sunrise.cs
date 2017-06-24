/*
	Sunrise.cs

	Improved warp-to-morning. "sun" rise and set calculations.

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

using KSP.UI;

namespace EarlyBird {

	[KSPAddon (KSPAddon.Startup.FlightAndKSC, false)]
	public static class Sunrise
	{
		// returns the lenght of "daylight" in terms of body rotation period,
		// ranging from 0 (no daylight) to 1 (no night).
		// really, it's the amount of time the "sun" body is above the
		// spherical horizon of body at the given latitude.
		public static double GetDayLength (double lat, CelestialBody body, CelestialBody sun)
		{
			// cos w = -tan p * tan d
			// w = hour angle, p = latitude, d = sun declination
			Vector3d sunPos = body.GetRelSurfacePosition (sun.position);
			double sunY = sunPos.y;
			sunPos.y = 0;
			double sunX = sunPos.magnitude;
			double tand = sunY / sunX;
			double tanp = Math.Tan (lat);
			double cosw = -tanp * tand;
			if (cosw < -1) {
				return 1;
			} else if (cosw > 1) {
				return 0;
			}
			// however, acos is nasty, so...
			double thalf = Math.Sqrt ((1 - cosw) / (1 + cosw));
			// the basic (acos) formula gives the angle of either sunrise
			// or sunset relative to noon, so need twice the angle to get
			// the angle swept by the sun through the day, but to avoid
			// acos, the half-angle was computed, so need 4x. Then to get
			// 0-1, divide by 2pi, so...
			return 2 * Math.Atan (thalf) / Math.PI;
		}

		// See GetDayLength for details
		public static double GetNightLength (double lat, CelestialBody body,
							   CelestialBody sun)
		{
			return 1 - GetDayLength (lat, body, sun);
		}

		public static double GetLocalTime (double lon, CelestialBody body, CelestialBody sun)
		{
			// latitude does not affect local time of day (it does affect sun
			// visibility, though)
			Vector3d zenith_ra = body.GetRelSurfaceNVector (0, lon);
			Vector3d sunPos = body.GetRelSurfacePosition (sun.position);
			sunPos.y = 0;	// not interested in declination
			Vector3d sunPos_ra = sunPos.normalized;
			double sign = Vector3d.Cross(zenith_ra, sunPos_ra).y >= 0 ? 1 : -1;
			return sign * Angle (sunPos_ra, zenith_ra) / (2 * Math.PI) + 0.5;
		}

		// NOTE: loses sign, so only 0-pi
		public static double Angle (Vector3d a, Vector3d b)
		{
			Vector3d amb = a * b.magnitude;
			Vector3d bma = b * a.magnitude;
			double y = (amb - bma).magnitude;
			double x = (amb + bma).magnitude;
			return 2 * Math.Atan2 (y, x);
		}
	}
}
