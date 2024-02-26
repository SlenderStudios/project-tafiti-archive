// Copyright 2007 Microsoft Corp. All Rights Reserved.

function Vertex(wx, wy, wz)
{
	var o = this;
	
	// World coordinates
	o.wx = wx;
	o.wy = wy;
	o.wz = wz;

	// Transform coordinates
	o.rx = wx;
	o.ry = wy;
	o.rz = wz;

	// Screen coordinates
	o.sx = 0;
	o.sy = 0;

	o.toString = function()
	{
		var o = this;
		return("Vertex wxyz: " + o.wx + ", " + o.wy + ", " + o.wz + "\n" +
					  "rxyz: " + o.rx + ", " + o.ry + ", " + o.rz + "\n" +
					   "sxy: " + o.sx + ", " + o.sy);
	}
}