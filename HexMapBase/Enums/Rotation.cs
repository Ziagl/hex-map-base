﻿namespace com.hexagonsimulations.HexMapBase.Enums;

/// <summary>
/// Represents a rotation either clockwise or counter-clockwise around a central point. 
/// Rotations must occur in exact 60 degree intervals, so each 60 degree interval from 
/// 60 through 300 degrees is represented in either direction (0 and 360 are not as they
/// represent no rotation).
/// </summary>
public enum Rotation
{
	CW_60,
	CW_120,
	CW_180,
	CW_240,
	CW_300,
	CCW_60,
	CCW_120,
	CCW_180,
	CCW_240,
	CCW_300,
}
