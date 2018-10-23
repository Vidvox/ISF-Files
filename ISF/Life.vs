varying vec2 left_coord;
varying vec2 right_coord;
varying vec2 above_coord;
varying vec2 below_coord;

varying vec2 lefta_coord;
varying vec2 righta_coord;
varying vec2 leftb_coord;
varying vec2 rightb_coord;


void main()
{
	isf_vertShaderInit();
	vec2 texc = vec2(isf_FragNormCoord[0],isf_FragNormCoord[1]) * RENDERSIZE;
	
	left_coord = vec2(texc.xy + vec2(-1.0 , 0));
	right_coord = vec2(texc.xy + vec2(1.0 , 0));
	above_coord = vec2(texc.xy + vec2(0,1.0));
	below_coord = vec2(texc.xy + vec2(0,-1.0));
	
	lefta_coord = vec2(texc.xy + vec2(-1.0 , 1.0));
	righta_coord = vec2(texc.xy + vec2(1.0 , 1.0));
	leftb_coord = vec2(texc.xy + vec2(-1.0 , -1.0));
	rightb_coord = vec2(texc.xy + vec2(1.0 , -1.0));
}