/*
{
  "CATEGORIES" : [
    "Stylize", "Color Effect"
  ],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "intensity",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : -1
    }
  ],
  "PASSES" : [
    {
      "TARGET" : "pass1"
    },
    {
      "TARGET" : "pass2"
    },
    {
      "TARGET" : "pass3"
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

float gray(vec4 n)
{
	return (n.r + n.g + n.b)/3.0;
}

void main()
{
	vec4 final = vec4(0.0);
	
	if (PASSINDEX == 0)	{
		vec4 color = IMG_THIS_PIXEL(inputImage);
		vec4 colorL = (IMG_NORM_PIXEL(inputImage, left_coord));
		vec4 colorR = (IMG_NORM_PIXEL(inputImage, right_coord));
		vec4 colorA = (IMG_NORM_PIXEL(inputImage, above_coord));
		vec4 colorB = (IMG_NORM_PIXEL(inputImage, below_coord));

		vec4 colorLA = (IMG_NORM_PIXEL(inputImage, lefta_coord));
		vec4 colorRA = (IMG_NORM_PIXEL(inputImage, righta_coord));
		vec4 colorLB = (IMG_NORM_PIXEL(inputImage, leftb_coord));
		vec4 colorRB = (IMG_NORM_PIXEL(inputImage, rightb_coord));
		
		final = color - color * intensity * (8.0*gray(color) - colorL - colorR - colorA - colorB - colorLA - colorRA - colorLB - colorRB);
		final.a = color.a;
	}
	else if (PASSINDEX == 1)	{
		vec4 color = IMG_THIS_PIXEL(pass1);
		vec4 colorL = (IMG_NORM_PIXEL(pass1, left_coord));
		vec4 colorR = (IMG_NORM_PIXEL(pass1, right_coord));
		vec4 colorA = (IMG_NORM_PIXEL(pass1, above_coord));
		vec4 colorB = (IMG_NORM_PIXEL(pass1, below_coord));

		vec4 colorLA = (IMG_NORM_PIXEL(pass1, lefta_coord));
		vec4 colorRA = (IMG_NORM_PIXEL(pass1, righta_coord));
		vec4 colorLB = (IMG_NORM_PIXEL(pass1, leftb_coord));
		vec4 colorRB = (IMG_NORM_PIXEL(pass1, rightb_coord));
		
		final = (1.0 - abs(intensity)) * color + abs(intensity) * (colorL + colorR + colorA + colorB + colorLA + colorRA + colorLB + colorRB) / 8.0;
		final.a = color.a;
	}
	else if (PASSINDEX == 2)	{
		vec4 color = IMG_THIS_PIXEL(pass2);
		vec4 colorL = (IMG_NORM_PIXEL(pass2, left_coord));
		vec4 colorR = (IMG_NORM_PIXEL(pass2, right_coord));
		vec4 colorA = (IMG_NORM_PIXEL(pass2, above_coord));
		vec4 colorB = (IMG_NORM_PIXEL(pass2, below_coord));

		vec4 colorLA = (IMG_NORM_PIXEL(pass2, lefta_coord));
		vec4 colorRA = (IMG_NORM_PIXEL(pass2, righta_coord));
		vec4 colorLB = (IMG_NORM_PIXEL(pass2, leftb_coord));
		vec4 colorRB = (IMG_NORM_PIXEL(pass2, rightb_coord));
		
		final = color - color * intensity * (8.0*gray(color) - colorL - colorR - colorA - colorB - colorLA - colorRA - colorLB - colorRB);
		final.a = color.a;
	}
	
	
	
	gl_FragColor = final;
}