/*
{
  "CATEGORIES" : [
    "Halftone Effect"
  ],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "sharpness",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "offset",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "scale",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "colorize",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "center",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "DEFAULT" : [
        0.5,
        0.5
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "CREDIT" : "by VIDVOX"
}
*/

varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;

const float tau = 6.28318530718;

float pattern() {
	float s = 0.0;
	float c = 1.0;
	vec2 tex = isf_FragNormCoord * RENDERSIZE;
	vec2 point = vec2( c * tex.x - s * tex.y, s * tex.x + c * tex.y );
	float d = distance(point, center * RENDERSIZE) * max(scale,0.001);
	return ( sin(d + offset * tau) ) * 4.0;
}

void main() {
	vec4 color = IMG_THIS_PIXEL(inputImage);
	vec4 colorL = IMG_NORM_PIXEL(inputImage, left_coord);
	vec4 colorR = IMG_NORM_PIXEL(inputImage, right_coord);
	vec4 colorA = IMG_NORM_PIXEL(inputImage, above_coord);
	vec4 colorB = IMG_NORM_PIXEL(inputImage, below_coord);

	vec4 colorLA = IMG_NORM_PIXEL(inputImage, lefta_coord);
	vec4 colorRA = IMG_NORM_PIXEL(inputImage, righta_coord);
	vec4 colorLB = IMG_NORM_PIXEL(inputImage, leftb_coord);
	vec4 colorRB = IMG_NORM_PIXEL(inputImage, rightb_coord);

	vec4 final = color + sharpness * (8.0*color - colorL - colorR - colorA - colorB - colorLA - colorRA - colorLB - colorRB);
	
	float average = ( final.r + final.g + final.b ) / 3.0;
	final = vec4( vec3( average * 10.0 - 5.0 + pattern() ), color.a );
	final = mix (color * final, final, 1.0-colorize);
	gl_FragColor = final;
}