/*{
  "DESCRIPTION": "Full-featured scope test shader with rotating patterns, animated luma, and time-controlled generative visuals",
  "CREDIT": "Cornelius // ProjectileObjects",
  "CATEGORIES": [ "Test Pattern", "Utility" ],
  "INPUTS": [
    {
      "NAME": "pattern",
      "TYPE": "long",
      "DEFAULT": 0,
      "LABEL": "Pattern Type",
      "VALUES": [ 0, 1, 2, 3, 4, 5, 6, 7, 8 ],
      "LABELS": [ "Color Bars", "Luma Gradient", "Hue Ramp", "Saturation Rings", "RGB Ramps", "Noise", "Moiré Grid", "RGB Flow", "Audio Flow" ]
    },
    {
      "NAME": "brightness",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 0.0,
      "MAX": 2.0
    },
    {
      "NAME": "contrast",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 0.0,
      "MAX": 2.0
    },
    {
      "NAME": "saturation",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 0.0,
      "MAX": 2.0
    },
    {
      "NAME": "hue",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": -1.0,
      "MAX": 1.0
    },
    {
      "NAME": "rotation",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": 0.0,
      "MAX": 1.0,
      "LABEL": "Pattern Rotation"
    },
    {
      "NAME": "timeSpeed",
      "TYPE": "float",
      "DEFAULT": 0.0,
      "MIN": -2.0,
      "MAX": 2.0,
      "LABEL": "Animation Speed"
    },
    {
      "NAME" : "audioFlow",
      "TYPE" : "audioFFT",
      "LABEL": "Audio Flow Source"
    }
  ]
}*/



vec3 hsv2rgb(vec3 c) {
	vec4 K = vec4(1., 2./3., 1./3., 3.);
	vec3 p = abs(fract(c.xxx + K.xyz)*6. - K.www);
	return c.z * mix(K.xxx, clamp(p - K.xxx, 0., 1.), c.y);
}

vec3 adjust(vec3 color) {
	float maxC = max(max(color.r, color.g), color.b);
	float minC = min(min(color.r, color.g), color.b);
	float delta = maxC - minC;
	float h = 0.0;
	if (delta > 0.0) {
		if (maxC == color.r)	{
			h = mod((color.g - color.b) / delta, 6.0);
		}
		else if (maxC == color.g)	{
			h = ((color.b - color.r) / delta) + 2.0;
		}
		else	{
			h = ((color.r - color.g) / delta) + 4.0;
		}
		h /= 6.0;
	}
	
	float s = (maxC == 0.0) ? 0.0 : delta / maxC;
	float v = maxC;
	
	h = mod(h + hue, 1.0);
	s = clamp(s * saturation, 0.0, 1.0);
	v = clamp((v - 0.5) * contrast + 0.5, 0.0, 1.0) * brightness;
	
	return hsv2rgb(vec3(h, s, v));
}

float rand(vec2 co) {
	return fract(sin(dot(co.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

vec2 rotateUV(vec2 uv, float angle) {
	vec2 centered = uv - 0.5;
	float s = sin(angle);
	float c = cos(angle);
	mat2 rot = mat2(c, -s, s, c);
	return rot * centered + 0.5;
}

vec3 patternColor(vec2 uv, int mode, float t) {
	if (mode == 0) { // Color Bars
		float x = floor(uv.x * 7.0);
		if (x == 0.0) return vec3(1.0, 1.0, 1.0);
		if (x == 1.0) return vec3(1.0, 1.0, 0.0);
		if (x == 2.0) return vec3(0.0, 1.0, 1.0);
		if (x == 3.0) return vec3(0.0, 1.0, 0.0);
		if (x == 4.0) return vec3(1.0, 0.0, 1.0);
		if (x == 5.0) return vec3(1.0, 0.0, 0.0);
		return vec3(0.0, 0.0, 1.0);
	}
	else if (mode == 1) {
		// Luma Gradient with animation
		float y = fract(uv.y + t);
		return vec3(y);
	}
	else if (mode == 2) {
		return hsv2rgb(vec3(fract(uv.x + t * 0.05), 1.0, 1.0)); // Hue Ramp
	}
	else if (mode == 3) {
		vec2 center = uv - 0.5;
		float dist = length(center) * 2.0;
		float angle = atan(center.y, center.x)/(2.0*3.14159)+0.5;
		return hsv2rgb(vec3(fract(angle + t * 0.1), clamp(dist, 0.0, 1.0), 1.0));
	}
	else if (mode == 4)	{
		return vec3(uv.x, uv.y, 1.0 - uv.x); // RGB Ramps
	}
	else if (mode == 5)	{
		return vec3(rand(uv + t)); // Noise
	}
	else if (mode == 6) {
		float lines = sin((uv.x + uv.y + t) * 300.0) * 0.5 + 0.5;
		return vec3(lines);
	}
	else if (mode == 7) {
		float r = 0.5 + 0.5*sin(t + uv.y * 10.0);
		float g = 0.5 + 0.5*sin(t + uv.x * 10.0);
		float b = 0.5 + 0.5*sin(t + uv.y * 10.0 + 3.14);
		return vec3(r, g, b);
	}
	else {
		float level = IMG_NORM_PIXEL(audioFlow,uv).r;
		return hsv2rgb(vec3(fract(uv.x + t * 0.05), 1.0, clamp(level, 0.0, 1.0)));
	}
}

void main() {
	float t = TIME * timeSpeed;
	float rotRad = rotation * 6.2831853; // 0–2π
	vec2 uv = rotateUV(isf_FragNormCoord, rotRad);
	vec3 col = patternColor(uv, int(pattern), t);
	col = adjust(col);
	gl_FragColor = vec4(col, 1.0);
}