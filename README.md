#Early Bird
Early Bird is a KSP mod for better warp-to-morning functionality.
Currently, it modifies the warp-to-morning feature such that warp ends five
minutes before sunrise, taking the space center latitude into account (not
relevant for stock Kerbin, but relevant for any planet mod that fakes axial
tilt (eg, RSS)).

Note that for certain latitudes at certain times of the year on bodies with
tilt relative to the sun, there is no dawn (due to either midnight sun or
midday dark). At such locations, local "midnight" is used in midnight sun
conditions, and local "noon" is used for midday dark conditions. Also, time
of day is undefined at the exact poles, so you may suffer rhino daemons if
the space center is exactly on a pole.

## planned features
* adjustable time offset for warp-to-morning
* flight mode warp-to-morning: warp to morning for the current landed
  location, anywhere on any planet
* warp to arbitrary time of day
* warp to arbitrary planet on horizon (or any other "hour")
* KAC integration
* orbital warp-to-"morning"
