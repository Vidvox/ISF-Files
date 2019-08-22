/*{
    "CATEGORIES": [
        "Transition"
    ],
    "CREDIT": null,
    "DESCRIPTION": "Automatically converted from https://gl-transitions.com/",
    "INPUTS": [
        {
            "NAME": "startImage",
            "TYPE": "image"
        },
        {
            "NAME": "endImage",
            "TYPE": "image"
        },
        {
            "DEFAULT": 0,
            "MAX": 1,
            "MIN": 0,
            "NAME": "progress",
            "TYPE": "float"
        },
        {
            "DEFAULT": 0.04,
            "MAX": 1,
            "MIN": 0,
            "NAME": "size",
            "TYPE": "float"
        },
        {
            "DEFAULT": 0.3,
            "MAX": 1,
            "MIN": 0,
            "NAME": "colorSeparation",
            "TYPE": "float"
        },
        {
            "DEFAULT": 50,
            "MAX": 100,
            "MIN": 0,
            "NAME": "zoom",
            "TYPE": "float"
        }
    ],
    "ISFVSN": "2",
    "VSN": null
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

vec4 transition(vec2 p) {
  float inv = 1. - progress;
  vec2 disp = size*vec2(cos(zoom*p.x), sin(zoom*p.y));
  vec4 texTo = getToColor(p + inv*disp);
  vec4 texFrom = vec4(
    getFromColor(p + progress*disp*(1.0 - colorSeparation)).r,
    getFromColor(p + progress*disp).g,
    getFromColor(p + progress*disp*(1.0 + colorSeparation)).b,
    1.0);
  return texTo*progress + texFrom*inv;
}



void main()	{
	gl_FragColor = transition(isf_FragNormCoord.xy);
}