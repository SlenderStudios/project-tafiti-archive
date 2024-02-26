// Copyright 2007 Microsoft Corp. All Rights Reserved.

function Projector(args)
{
	var o = this;

	// Properties
	o.width = args.w;
	o.height = args.h;

	o.r = args.r;		// camera rotation (yaw)
	o.x = args.x;		// camera in/out
	o.y = args.y;		// camera left/right
	o.z = args.z;		// camera high/low
	o.fov = args.fov;	// camera field of view (horizontal viewplane angle)
	o.roll = args.roll;	// camera roll (roll)

	// Methods
	o.SetEntities = Projector_SetEntities;
	o.Render = Projector_Render;
	o.UpdateRotationMatrix = Projector_UpdateRotationMatrix;
	o.Update = Projector_Update;
	
	// Initialize
	o.Update();
}

function Projector_SetEntities(ents)
{
	var o = this;

	if(ents != null)
	{
		o.aEntities = ents;
		o._maxLevel = g_maxLevel;//100 * ents.length;
	}
}

function Projector_Render()
{
	var o = this;
	var a, i, u, v, ps;

	// render planes
	var vx, vertices;
	var j = o.aEntities.length;
	var ent;
	
	while(--j >= 0)
	{
		e = o.aEntities[j];

		// Layering
		if(e.zRot_n != null)
		{
			a = e.zRot_n + o.r;
			
			// Clamp between -PI/PI
			if(a < -Math.PI) a += g_2PI;
			else if(a > Math.PI) a -= g_2PI;
			
			if(a > 0) u = (1 - a / Math.PI);	// right quadrant
			else u = (a / Math.PI);				// left quadrant

			e._currScale = ((u >= 0) ? u : (1 + u));
		}

		verts = e.vertices;
		i = verts.length;
		
		// Update each registered plane
		while(--i > -1)
		{
			vx = verts[i];

			u = vx.rx - o.x;
			v = vx.ry - o.y;

			ps = o.hded / ((u * o.cs + v * o.sn) + o.z2);
//			vx.sx = Math.round(o.w2 + (v * o.cs - u * o.sn) * ps);
//			vx.sy = Math.round(o.hz - (vx.rz - o.z2) * ps);
			vx.sx = o.w2 + (v * o.cs - u * o.sn) * ps;
			vx.sy = o.hz - (vx.rz - o.z2) * ps;

			// Debug
			/*
			if(i == 0 && j == 0)
			{
				var t = stage.root.findName("txtDebug0");
				if(t != null)
				{
					t.Text = ("ps = " + ps);
				}
			}*/
		}
	}
	
	return;
}

function Projector_UpdateRotationMatrix()
{
	var o = this;

	if(o.r > Math.PI) o.r -= g_2PI;
	else if(o.r < -Math.PI) o.r += g_2PI;

	o.sn = Math.sin(o.r);
	o.cs = Math.cos(o.r);
}

// Update the world, based on view settings
function Projector_Update()
{
	var o = this;

	if(o.z < 1)
	{
		o.z = 1;
	}

	o.w2 = o.width / 2;
	o.h2 = o.height / 2;
	o.z2 = o.z * 2;
	o.hz = o.h2 - o.roll;
	o.hded = o.w2 / Math.tan(o.fov / 2) + o.roll;
}