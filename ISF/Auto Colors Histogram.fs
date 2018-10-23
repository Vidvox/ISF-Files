/*
{
  "CATEGORIES" : [
    "Color Adjustment"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "sourceHistogram",
      "TYPE" : "image"
    },
    {
      "NAME" : "maxGain",
      "TYPE" : "float",
      "MAX" : 8,
      "DEFAULT" : 2,
      "MIN" : 1
    }
  ],
  "PASSES" : [
    {
      "WIDTH" : "2",
      "TARGET" : "minmax",
      "HEIGHT" : "1"
    },
    {

    }
  ],
  "CREDIT" : "VIDVOX"
}
*/

void main()	{
	vec4		returnMe = vec4(0.0);
	if (PASSINDEX==0)	{
		float		minOrMax = (isf_FragNormCoord.x < 0.5) ? 0.0 : 1.0;
		//	go through each pixel of sourceHistogram
		//		find the darkest and brightest for each color channel
		if (isf_FragNormCoord.x < 0.5)	{
			returnMe.a = 1.0;
			for(int i=0;i<256;++i)	{
				vec4	histo = IMG_PIXEL(sourceHistogram,vec2(float(i),0.5));
				if (returnMe.r == 0.0)	{
					if (histo.r > 0.01)	{
						returnMe.r = float(i)/255.0;
					}
				}
				if (returnMe.g == 0.0)	{
					if (histo.g > 0.01)	{
						returnMe.g = float(i)/255.0;
					}
				}
				if (returnMe.b == 0.0)	{
					if (histo.b > 0.01)	{
						returnMe.b = float(i)/255.0;
					}
				}
			}
		}
		else	{
			returnMe = vec4(1.0);
			for(int i=255;i>=0;--i)	{
				vec4	histo = IMG_PIXEL(sourceHistogram,vec2(float(i),0.5));
				if (returnMe.r == 1.0)	{
					if (histo.r > 0.01)	{
						returnMe.r = float(i)/255.0;
					}
				}
				if (returnMe.g == 1.0)	{
					if (histo.g > 0.01)	{
						returnMe.g = float(i)/255.0;
					}
				}
				if (returnMe.b == 1.0)	{
					if (histo.b > 0.01)	{
						returnMe.b = float(i)/255.0;
					}
				}
			}
		}
	}
	else if (PASSINDEX==1)	{
		returnMe = IMG_THIS_PIXEL(inputImage);
		vec4	minVal = IMG_PIXEL(minmax,vec2(0.5,0.5));
		vec4	maxVal = IMG_PIXEL(minmax,vec2(1.5,0.5));
		vec4	contrastVal = 1.0 / (maxVal - minVal);
		if (contrastVal.r > maxGain)
			contrastVal.r = maxGain;

		if (contrastVal.g > maxGain)
			contrastVal.g = maxGain;
		
		if (contrastVal.b > maxGain)
			contrastVal.b = maxGain;
		
		returnMe.r = (returnMe.r - minVal.r) * contrastVal.r;
		returnMe.g = (returnMe.g - minVal.g) * contrastVal.g;
		returnMe.b = (returnMe.b - minVal.b) * contrastVal.b;
	}
	
	gl_FragColor = returnMe;
}
