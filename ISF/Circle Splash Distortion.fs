/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Distortion Effect"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 0.75,
			"DEFAULT": 0.125
		},
		{
			"NAME": "streaks",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "center",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			]
		}
	]
}*/




//	Pretty simple – if we're inside the radius, draw as normal
//	If we're outside the circle grab the last color along the angle

#ifndef GL_ES
float distance (vec2 center, vec2 pt)
{
	float tmp = pow(center.x-pt.x,2.0)+pow(center.y-pt.y,2.0);
	return pow(tmp,0.5);
}
#endif

void main() {
	vec2 uv = vec2(isf_FragNormCoord[0],isf_FragNormCoord[1]);
	vec2 texSize = RENDERSIZE;
	vec2 tc = uv * texSize;
	vec2 tc2 = uv * texSize;
	vec2 modifiedCenter = center;
	float r = distance(modifiedCenter, tc);
	float render_length = length(RENDERSIZE);
	float a = atan ((tc.y-modifiedCenter.y),(tc.x-modifiedCenter.x));
	float radius_sized = clamp(radius * render_length, 1.0, render_length);
	
	tc -= modifiedCenter;
	tc2 -= modifiedCenter;

	if (r < radius_sized) 	{
		tc.x = r * cos(a);
		tc.y = r * sin(a);
		tc2 = tc;
	}
	else	{
		tc.x = radius_sized * cos(a);
		tc.y = radius_sized * sin(a);
		tc2.x = (radius_sized + streaks * render_length) * cos(a);
		tc2.y = (radius_sized + streaks * render_length) * sin(a); 
	}
	tc += modifiedCenter;
	tc2 += modifiedCenter;
	vec2 loc = tc / texSize;

	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		vec4 result = IMG_NORM_PIXEL(inputImage, loc);
		if (streaks > 0.0)	{
			vec2 loc2 = tc2 / texSize;
			vec4 mixColor = IMG_NORM_PIXEL(inputImage, loc2);
			result = mix(result, mixColor, clamp(2.0*((r - radius_sized)/(render_length))*streaks,0.0,1.0));
		}
		gl_FragColor = result;
	}
}