// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Transformation matrix class
function PMatrix()
{
	var o = this;

	// Methods
	o.Clone			= PMatrix_clone;
	o.Concat		= PMatrix_concat;
	o.Determinant	= PMatrix_determinant;
	o.Identity		= PMatrix_identity;
	o.Invert		= PMatrix_invert;
	o.RotateAxis	= PMatrix_rotateAxis;
	o.Scale			= PMatrix_scale;
	o.TransformVert = PMatrix_transformVert;
	o.Translate		= PMatrix_translate;
	o.toString		= PMatrix_toString;

	// Initialize properly
	o.Identity();
}
	
function PMatrix_identity()
{
	var o = this;

	o.m11 = 1;	o.m12 = 0;	o.m13 = 0;
	o.m21 = 0;	o.m22 = 1;	o.m23 = 0;
	o.m31 = 0;	o.m32 = 0; 	o.m33 = 1;
	o.tx  = 0;	o.ty  = 0;	o.tz  = 0;
}

function PMatrix_translate(x, y, z)
{
	var o = this;
	var r = new PMatrix();

	r.tx = x;
	r.ty = y;
	r.tz = z;

	o.Concat(r);
}

function PMatrix_scale(sx, sy, sz)
{
	var o = this;
	var r = new PMatrix();

	r.m11 = sx;
	r.m22 = sy;
	r.m33 = sz;

	o.Concat(r);
}

function PMatrix_rotateAxis(axis, theta)
{
	var o = this;
	var s = Math.sin(theta);
	var c = Math.cos(theta);
	var r = new PMatrix();

	switch(axis)
	{
		case 0:
			//-- rotate about x-axis
			r.m21 = 0;	r.m22 = c;	r.m23 = s;
			r.m31 = 0;	r.m32 = -s;	r.m33 = c;
			break;

		case 1:
			//-- rotate about y-axis
			r.m11 = c;	r.m12 = 0;	r.m13 = -s;
			r.m31 = s;	r.m32 = 0;	r.m33 = c;
			break;

		case 2:
			//-- rotate about z-axis
			r.m11 = c;	r.m12 = s;	r.m13 = 0;
			r.m21 = -s;	r.m22 = c;	r.m23 = 0;
			break;
	}

	o.Concat(r);
}

function PMatrix_concat(b)
{
	var o = this;
	var a = o.Clone();

	o.m11 = a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31;
	o.m12 = a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32;
	o.m13 = a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33;

	o.m21 = a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31;
	o.m22 = a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32;
	o.m23 = a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33;

	o.m31 = a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31;
	o.m32 = a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32;
	o.m33 = a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33;

	o.tx = a.tx * b.m11 + a.ty * b.m21 + a.tz * b.m31 + b.tx;
	o.ty = a.tx * b.m12 + a.ty * b.m22 + a.tz * b.m32 + b.ty;
	o.tz = a.tx * b.m13 + a.ty * b.m23 + a.tz * b.m33 + b.tz;
}

function PMatrix_invert(m)
{
	var det = m.Determinant();

	if(det == 0)
	{
		return(null);
	}

	var r = new PMatrix();

	r.m11 = (m.m22 * m.m33 - m.m23 * m.m32) / det;
	r.m12 = (m.m13 * m.m32 - m.m12 * m.m33) / det;
	r.m13 = (m.m12 * m.m23 - m.m13 * m.m22) / det;

	r.m21 = (m.m23 * m.m31 - m.m21 * m.m33) / det;
	r.m22 = (m.m11 * m.m33 - m.m13 * m.m31) / det;
	r.m23 = (m.m13 * m.m21 - m.m11 * m.m23) / det;

	r.m31 = (m.m21 * m.m32 - m.m22 * m.m31) / det;
	r.m32 = (m.m12 * m.m31 - m.m11 * m.m32) / det;
	r.m33 = (m.m11 * m.m22 - m.m12 * m.m21) / det;

	r.tx = -(m.tx * r.m11 + m.ty * r.m21 + m.tz * r.m31);
	r.ty = -(m.tx * r.m12 + m.ty * r.m22 + m.tz * r.m32);
	r.tz = -(m.tx * r.m13 + m.ty * r.m23 + m.tz * r.m33);

	return(r);
}

function PMatrix_determinant()
{
	var o = this;

	return(o.m11 * (o.m22 * o.m33 - o.m23 * o.m32) +
		   o.m12 * (o.m23 * o.m31 - o.m21 * o.m33) +
		   o.m13 * (o.m21 * o.m32 - o.m22 * o.m31));
}

function PMatrix_transformVert(v)
{
	var o = this;
	var x = v.wx;
	var y = v.wy;
	var z = v.wz;

	v.rx = x * o.m11 + y * o.m21 + z * o.m31 + o.tx;
	v.ry = x * o.m12 + y * o.m22 + z * o.m32 + o.ty;
	v.rz = x * o.m13 + y * o.m23 + z * o.m33 + o.tz;
}

function PMatrix_clone()
{
	var o = this;
	var m = new PMatrix();

	m.m11 = o.m11;	m.m12 = o.m12;	m.m13 = o.m13;
	m.m21 = o.m21;	m.m22 = o.m22;	m.m23 = o.m23;
	m.m31 = o.m31;	m.m32 = o.m32;	m.m33 = o.m33;
	m.tx  = o.tx;	m.ty  = o.ty;	m.tz  = o.tz;

	return(m);
}

function PMatrix_toString()
{
	var o = this;
	var a = [ [ Math.round(o.m11*1000)/1000, Math.round(o.m12*1000)/1000, Math.round(o.m13*1000)/1000 ],
			  [ Math.round(o.m21*1000)/1000, Math.round(o.m22*1000)/1000, Math.round(o.m23*1000)/1000 ],
			  [ Math.round(o.m31*1000)/1000, Math.round(o.m32*1000)/1000, Math.round(o.m33*1000)/1000 ],
			  [ Math.round(o.tx *1000)/1000, Math.round(o.ty *1000)/1000, Math.round(o.tz *1000)/1000 ] ]; 

	return(a.join("\n"));
}