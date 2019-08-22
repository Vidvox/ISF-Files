/*
{
  "ISFVSN" : "2",
  "CATEGORIES" : [
    "Transition"
  ],
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "startImage"
    },
    {
      "NAME" : "endImage",
      "TYPE" : "image"
    },
    {
      "TYPE" : "float",
      "MIN" : 0,
      "DEFAULT" : 0,
      "NAME" : "progress",
      "MAX" : 1
    },
    {
      "NAME" : "luma",
      "TYPE" : "image"
    }
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/gl-transitions.com\/"
}
*/



vec4 getFromColor(vec2 inUV)	{
	return IMG_NORM_PIXEL(startImage, inUV);
}
vec4 getToColor(vec2 inUV)	{
	return IMG_NORM_PIXEL(endImage, inUV);
}



// Author: gre
// License: MIT


vec4 transition(vec2 uv) {
  return mix(
    getToColor(uv),
    getFromColor(uv),
    step(progress, IMG_NORM_PIXEL(luma, uv).r-0.001)
  );
}



void main()	{
	gl_FragColor = transition(isf_FragNormCoord.xy);
}