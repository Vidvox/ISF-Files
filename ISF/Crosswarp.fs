/*
{
  "CATEGORIES" : [
    "Transition"
  ],
  "DESCRIPTION": "",
  "CREDIT": "Automatically converted from https://www.github.com/gl-transitions/gl-transitions/tree/master/crosswarp.glsl",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "startImage"
    },
    {
      "TYPE" : "image",
      "NAME" : "endImage"
    },
    {
      "TYPE" : "float",
      "MAX" : 1,
      "NAME" : "progress",
      "DEFAULT" : 0,
      "MIN" : 0
    }
  ]
}
*/



vec4 getFromColor(vec2 inUV)	{
	return IMG_NORM_PIXEL(startImage, inUV);
}
vec4 getToColor(vec2 inUV)	{
	return IMG_NORM_PIXEL(endImage, inUV);
}



// Author: Eke Péter <peterekepeter@gmail.com>
// License: MIT
vec4 transition(vec2 p) {
  float x = progress;
  x=smoothstep(.0,1.0,(x*2.0+p.x-1.0));
  return mix(getFromColor((p-.5)*(1.-x)+.5), getToColor((p-.5)*x+.5), x);
}



void main()	{
	gl_FragColor = transition(isf_FragNormCoord.xy);
}