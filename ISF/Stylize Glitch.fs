/*
{
  "CATEGORIES" : [
    "Glitch"
  ],
  "DESCRIPTION" : "Applies randomized effects to different parts of the image.",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "maxGlitchSize",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "LABEL" : "Glitch Level",
      "MIN" : 0
    },
    {
      "NAME" : "glitchRate",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "LABEL" : "Glitch Rate",
      "MIN" : 0
    },
	{
		"NAME": "stylizeMode",
		"LABEL": "Style Mode",
		"VALUES": [
			0,
			1,
			2,
			3
		],
		"LABELS": [
			"Random",
			"Invert",
			"Dither",
			"Hue Shift"
		],
		"DEFAULT": 0,
		"TYPE": "long"
	}
  ],
  "PASSES" : [
    {
      "TARGET" : "lastState",
      "PERSISTENT" : true
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/



bool pointInRect(vec2 pt, vec4 r)
{
	bool	returnMe = false;
	if ((pt.x >= r.x)&&(pt.y >= r.y)&&(pt.x <= r.x + r.z)&&(pt.y <= r.y + r.w))
		returnMe = true;
	return returnMe;
}



const float pi = 3.14159265359;



float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec4 rand4(vec4 co)	{
	vec4	returnMe = vec4(0.0);
	returnMe.r = rand(co.rg);
	returnMe.g = rand(co.gb);
	returnMe.b = rand(co.ba);
	returnMe.a = rand(co.rb);
	return returnMe;
}

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


//	applies a randomized invert on this pixel
vec4 invert(vec4 col)	{
	vec4	returnMe = col;
	returnMe.rgb = 1.0 - returnMe.rgb;
	return returnMe;
}

//	applies a randomized hue & saturation shift on this pixel
vec4 hueShift(vec4 col)	{
	vec4 returnMe = col;
	returnMe.rgb = rgb2hsv(returnMe.rgb);
	returnMe.r = mod(returnMe.r + 0.333, 1.0);
	returnMe.rgb = hsv2rgb(returnMe.rgb);
	return returnMe;
}


//	based on https://github.com/hughsk/glsl-dither

float luma(vec3 color)	{
	return (color.r + color.g + color.b) / 3.0;	
}

float luma(vec4 color)	{
	return color.a * (color.r + color.g + color.b) / 3.0;	
}

float dither8x8(vec2 position, float brightness) {
  int x = int(mod(position.x, 8.0));
  int y = int(mod(position.y, 8.0));
  int index = x + y * 8;
  float limit = 0.0;

  if (x < 8) {
    if (index == 0) limit = 0.015625;
    if (index == 1) limit = 0.515625;
    if (index == 2) limit = 0.140625;
    if (index == 3) limit = 0.640625;
    if (index == 4) limit = 0.046875;
    if (index == 5) limit = 0.546875;
    if (index == 6) limit = 0.171875;
    if (index == 7) limit = 0.671875;
    if (index == 8) limit = 0.765625;
    if (index == 9) limit = 0.265625;
    if (index == 10) limit = 0.890625;
    if (index == 11) limit = 0.390625;
    if (index == 12) limit = 0.796875;
    if (index == 13) limit = 0.296875;
    if (index == 14) limit = 0.921875;
    if (index == 15) limit = 0.421875;
    if (index == 16) limit = 0.203125;
    if (index == 17) limit = 0.703125;
    if (index == 18) limit = 0.078125;
    if (index == 19) limit = 0.578125;
    if (index == 20) limit = 0.234375;
    if (index == 21) limit = 0.734375;
    if (index == 22) limit = 0.109375;
    if (index == 23) limit = 0.609375;
    if (index == 24) limit = 0.953125;
    if (index == 25) limit = 0.453125;
    if (index == 26) limit = 0.828125;
    if (index == 27) limit = 0.328125;
    if (index == 28) limit = 0.984375;
    if (index == 29) limit = 0.484375;
    if (index == 30) limit = 0.859375;
    if (index == 31) limit = 0.359375;
    if (index == 32) limit = 0.0625;
    if (index == 33) limit = 0.5625;
    if (index == 34) limit = 0.1875;
    if (index == 35) limit = 0.6875;
    if (index == 36) limit = 0.03125;
    if (index == 37) limit = 0.53125;
    if (index == 38) limit = 0.15625;
    if (index == 39) limit = 0.65625;
    if (index == 40) limit = 0.8125;
    if (index == 41) limit = 0.3125;
    if (index == 42) limit = 0.9375;
    if (index == 43) limit = 0.4375;
    if (index == 44) limit = 0.78125;
    if (index == 45) limit = 0.28125;
    if (index == 46) limit = 0.90625;
    if (index == 47) limit = 0.40625;
    if (index == 48) limit = 0.25;
    if (index == 49) limit = 0.75;
    if (index == 50) limit = 0.125;
    if (index == 51) limit = 0.625;
    if (index == 52) limit = 0.21875;
    if (index == 53) limit = 0.71875;
    if (index == 54) limit = 0.09375;
    if (index == 55) limit = 0.59375;
    if (index == 56) limit = 1.0;
    if (index == 57) limit = 0.5;
    if (index == 58) limit = 0.875;
    if (index == 59) limit = 0.375;
    if (index == 60) limit = 0.96875;
    if (index == 61) limit = 0.46875;
    if (index == 62) limit = 0.84375;
    if (index == 63) limit = 0.34375;
  }

  return brightness < limit ? 0.0 : 1.0;
}

vec3 dither8x8(vec2 position, vec3 color) {
  return color * dither8x8(position, luma(color));
}

vec4 dither8x8(vec2 position, vec4 color) {
  return vec4(color.rgb * dither8x8(position, luma(color)), 1.0);
}

float dither4x4(vec2 position, float brightness) {
  int x = int(mod(position.x, 4.0));
  int y = int(mod(position.y, 4.0));
  int index = x + y * 4;
  float limit = 0.0;

  if (x < 8) {
    if (index == 0) limit = 0.0625;
    if (index == 1) limit = 0.5625;
    if (index == 2) limit = 0.1875;
    if (index == 3) limit = 0.6875;
    if (index == 4) limit = 0.8125;
    if (index == 5) limit = 0.3125;
    if (index == 6) limit = 0.9375;
    if (index == 7) limit = 0.4375;
    if (index == 8) limit = 0.25;
    if (index == 9) limit = 0.75;
    if (index == 10) limit = 0.125;
    if (index == 11) limit = 0.625;
    if (index == 12) limit = 1.0;
    if (index == 13) limit = 0.5;
    if (index == 14) limit = 0.875;
    if (index == 15) limit = 0.375;
  }

  return brightness < limit ? 0.0 : 1.0;
}

vec3 dither4x4(vec2 position, vec3 color) {
  return color * dither4x4(position, luma(color));
}

vec4 dither4x4(vec2 position, vec4 color) {
  return vec4(color.rgb * dither4x4(position, luma(color)), 1.0);
}

float dither2x2(vec2 position, float brightness) {
  int x = int(mod(position.x, 2.0));
  int y = int(mod(position.y, 2.0));
  int index = x + y * 2;
  float limit = 0.0;

  if (x < 8) {
    if (index == 0) limit = 0.25;
    if (index == 1) limit = 0.75;
    if (index == 2) limit = 1.00;
    if (index == 3) limit = 0.50;
  }

  return brightness < limit ? 0.0 : 1.0;
}

vec3 dither2x2(vec2 position, vec3 color) {
  return color * dither2x2(position, luma(color));
}

vec4 dither2x2(vec2 position, vec4 color) {
  return vec4(color.rgb * dither2x2(position, luma(color)), 1.0);
}

//	applies a randomized dither on this pixel
vec4 dither(vec4 col)	{
	vec2	loc = gl_FragCoord.xy;
	return dither8x8(loc, col);
}


//	returns the pattern based on the index
vec4 stylizeSelect(vec4 color, int sIndex, vec2 seed)	{
	
	//	invert
	if (sIndex == 0)	{
		return invert(color);
	}
	//	dither
	else if (sIndex == 1)	{
		return dither(color);
	}
	//	hue shift
	else if (sIndex == 2)	{
		return hueShift(color);
	}
	
	return vec4(0.0);
}

vec4 applyStylize(vec4 color, vec2 seed)	{
	//	seed contains values to use in randomization
	int	sIndex = stylizeMode;
	if (sIndex == 0)	{
		float	val = 3.0 * rand(seed);
		val = min(val, 2.0);
		sIndex = int(val);
	}
	else	{
		sIndex = sIndex - 1;
	}
	
	return stylizeSelect(color, sIndex, seed);
}



void main()	{
	vec2	loc = isf_FragNormCoord.xy;
	vec4	returnMe = IMG_THIS_PIXEL(inputImage);
	int		glitchCount = 3;
	
	if (glitchRate > 0.0 && maxGlitchSize > 0.0)	{
		for (int i = 0; i < 10; ++i)	{
			if (i > glitchCount)
				break;
			float	adjustedTime = floor(120.0 * TIME * glitchRate);
			vec4	seeds1 = (float(i)+adjustedTime) * vec4(0.2123,0.34517,0.53428,0.7431);
			vec4	randCoords = rand4(seeds1);
			randCoords.zw *= maxGlitchSize;
			if (randCoords.x + randCoords.z > 1.0)
				randCoords.z = 1.0 - randCoords.x;
			if (randCoords.y + randCoords.w > 1.0)
				randCoords.w = 1.0 - randCoords.y;
	
			bool	isInShape = pointInRect(loc,randCoords);
			if (isInShape)	{
				vec2	seeds2= vec2(2.7413+float(i),1.325821*TIME);
				returnMe = applyStylize(returnMe, seeds2);
			}
		}
	}
	gl_FragColor = returnMe;
}
