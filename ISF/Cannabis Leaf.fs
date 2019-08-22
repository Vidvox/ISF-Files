/*
{
  "DESCRIPTION" : "Automatically converted from https://gl-transitions.com/",
  "CREDIT": "Automatically converted from https://www.github.com/gl-transitions/gl-transitions/tree/master/cannabisleaf.glsl",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "startImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "endImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "progress",
      "MIN" : 0,
      "MAX" : 1,
      "TYPE" : "float",
      "DEFAULT" : 0
    }
  ],
  "CATEGORIES" : [
    "Transition"
  ]
}
*/



vec4 getFromColor(vec2 inUV)	{
	return IMG_NORM_PIXEL(startImage, inUV);
}
vec4 getToColor(vec2 inUV)	{
	return IMG_NORM_PIXEL(endImage, inUV);
}



// Author: @Flexi23
// License: MIT

// inspired by http://www.wolframalpha.com/input/?i=cannabis+curve

vec4 transition (vec2 uv) {
  if(progress == 0.0){
    return getFromColor(uv);
  }
  vec2 leaf_uv = (uv - vec2(0.5))/10./pow(progress,3.5);
	leaf_uv.y += 0.35;
	float r = 0.18;
	float o = atan(leaf_uv.y, leaf_uv.x);
  return mix(getFromColor(uv), getToColor(uv), 1.-step(1. - length(leaf_uv)+r*(1.+sin(o))*(1.+0.9 * cos(8.*o))*(1.+0.1*cos(24.*o))*(0.9+0.05*cos(200.*o)), 1.));
}



void main()	{
	gl_FragColor = transition(isf_FragNormCoord.xy);
}