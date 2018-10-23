/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "pointCount",
			"LABEL": "Point Count",
			"TYPE": "float",
			"MIN": 3.0,
			"MAX": 90.0,
			"DEFAULT": 5.0
		},
		{
			"NAME": "randomSeed",
			"LABEL": "Random Seed",
			"TYPE": "float",
			"MIN": 0.01,
			"MAX": 1.0,
			"DEFAULT": 0.125
		},
		{
			"NAME": "colorSaturation",
			"LABEL": "Saturation",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.0
		},
		{
			"NAME": "randomizeBrightness",
			"LABEL": "Random Shades",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
			"NAME": "randomizeAlpha",
			"LABEL": "Random Alpha",
			"TYPE": "bool",
			"DEFAULT": false
		},
		{
			"NAME": "randomizeAllPoints",
			"LABEL": "Triangle Shapes",
			"TYPE": "bool",
			"DEFAULT": false
		}
	]
}*/



vec3 rgb2hsv(vec3 c)	{
	vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	//vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
	//vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));
	vec4 p = c.g < c.b ? vec4(c.bg, K.wz) : vec4(c.gb, K.xy);
	vec4 q = c.r < p.x ? vec4(p.xyw, c.r) : vec4(c.r, p.yzx);
	
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)	{
	vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}


float sign(vec2 p1, vec2 p2, vec2 p3)	{
	return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
}

bool PointInTriangle(vec2 pt, vec2 v1, vec2 v2, vec2 v3)	{
	bool b1, b2, b3;

	b1 = sign(pt, v1, v2) < 0.0;
	b2 = sign(pt, v2, v3) < 0.0;
	b3 = sign(pt, v3, v1) < 0.0;

	return ((b1 == b2) && (b2 == b3));
}


void main() {
	vec4		result = vec4(0.0);
	vec2		thisPoint = isf_FragNormCoord;
	vec3		colorHSL;
	vec2		pt1, pt2, pt3;
	
	colorHSL.x = rand(vec2(floor(pointCount)+randomSeed, 1.0));
	colorHSL.y = colorSaturation;
	colorHSL.z = 1.0;
	if (randomizeBrightness)	{
		colorHSL.z = rand(vec2(floor(pointCount)+randomSeed * 3.72, randomSeed + pointCount * 0.649));
	}
	
	pt1 = vec2(rand(vec2(floor(pointCount)+randomSeed*1.123,randomSeed*1.321)),rand(vec2(randomSeed*2.123,randomSeed*3.325)));
	pt2 = vec2(rand(vec2(floor(pointCount)+randomSeed*5.317,randomSeed*2.591)),rand(vec2(randomSeed*1.833,randomSeed*4.916)));
	pt3 = vec2(rand(vec2(floor(pointCount)+randomSeed*3.573,randomSeed*6.273)),rand(vec2(randomSeed*9.253,randomSeed*7.782)));
	
	if (PointInTriangle(thisPoint,pt1,pt2,pt3))	{
		float newAlpha = 1.0;
		
		if (randomizeAlpha)	{
			newAlpha = 0.5 + 0.5 * rand(vec2(1.0 + floor(pointCount)+randomSeed * 1.938, randomSeed * pointCount * 1.541));
		}
		
		result.rgb = hsv2rgb(colorHSL);
		result.a = result.a + newAlpha;
	}
	
	for (float i = 0.0; i < 60.0; ++i)	{
		if (result.a > 0.75)
			break;
		if (i > pointCount - 3.0)
			break;
		if (randomizeAllPoints)	{
			pt1 = vec2(rand(vec2(i+randomSeed*1.123,i*floor(pointCount)+randomSeed*1.321)),rand(vec2(i*floor(pointCount)+randomSeed*2.123,i+randomSeed*1.325)));
			pt2 = vec2(rand(vec2(i*floor(pointCount)+randomSeed*5.317,randomSeed*2.591)),rand(vec2(i+randomSeed*1.833,i*floor(pointCount)+randomSeed*4.916)));
		}
		else	{
			pt1 = pt2;
			pt2 = pt3;
		}
		pt3 = vec2(rand(vec2(i*floor(pointCount)+randomSeed*3.573,i+randomSeed*6.273)),rand(vec2(i+randomSeed*9.253,i+randomSeed*7.782)));
		if (PointInTriangle(thisPoint,pt1,pt2,pt3))	{
			//result = vec4(1.0);
			float newAlpha = 1.0;
			
			if (randomizeAlpha)	{
				newAlpha = 0.1 + 0.25 * rand(vec2(i + floor(pointCount)+randomSeed * 1.938, randomSeed * pointCount * 1.541));
			}
			
			colorHSL.x = rand(vec2(floor(pointCount)+randomSeed, i));
			if (randomizeBrightness)	{
				colorHSL.z = 0.15 + 0.85 * rand(vec2(i + floor(pointCount)+randomSeed * 2.78, randomSeed + pointCount * 0.249));
			}
			result.rgb = hsv2rgb(colorHSL);
			result.a = result.a + newAlpha;
		}
	}
		
	//result.rgb = result.rgb * hsv2rgb(colorHSL);
	
	gl_FragColor = result;
}

	