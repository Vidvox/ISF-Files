/*{
    "CATEGORIES": [
        "Pattern", "Noise"
    ],
    "CREDIT": "VIDVOX",
    "DESCRIPTION": "Drawing random characters from a font encoded as an array.",
    "INPUTS": [
        {
            "DEFAULT": 0.3841,
            "MAX": 1,
            "MIN": 0,
            "NAME": "charCodeSeed",
            "TYPE": "float"
        },
        {
            "DEFAULT": 1.0,
            "MAX": 8.0,
            "MIN": 1.0,
            "NAME": "blockSize",
            "TYPE": "float"
        },
        {
            "DEFAULT": [
                0,
                0.83,
                0.13,
                1
            ],
            "NAME": "charColor",
            "TYPE": "color"
        },
        {
            "DEFAULT": [
                0.0,
                0.0,
                0.0,
                1
            ],
            "NAME": "bgColor",
            "TYPE": "color"
        }
    ],
    "ISFVSN": "2"
}
*/


//	Inspired / adapted from Tim Gfrerer's brilliant Texture-less Text Rendering blog post:
//	https://poniesandlight.co.uk/reflect/debug_print_text/



// Original Font Data: 
//
// http://www.fial.com/~scott/tamsyn-font/ 
//
// Range 0x20 to 0x7f (inclusive)
//
// Every uvec4 holds the bitmap for one 8x16 bit character. 
//

uvec2	glyphSize = uvec2(8, 16);

const uvec4 font_data[96] = uvec4[96](

    uvec4( 0x00000000, 0x00000000, 0x00000000, 0x00000000 ),
    uvec4( 0x00001010, 0x10101010, 0x00001010, 0x00000000 ),
    uvec4( 0x00242424, 0x24000000, 0x00000000, 0x00000000 ),
    uvec4( 0x00000024, 0x247E2424, 0x247E2424, 0x00000000 ),
    uvec4( 0x00000808, 0x1E20201C, 0x02023C08, 0x08000000 ),
    uvec4( 0x00000030, 0x494A3408, 0x16294906, 0x00000000 ),
    uvec4( 0x00003048, 0x48483031, 0x49464639, 0x00000000 ),
    uvec4( 0x00101010, 0x10000000, 0x00000000, 0x00000000 ),
    uvec4( 0x00000408, 0x08101010, 0x10101008, 0x08040000 ),
    uvec4( 0x00002010, 0x10080808, 0x08080810, 0x10200000 ),
    uvec4( 0x00000000, 0x0024187E, 0x18240000, 0x00000000 ),
    uvec4( 0x00000000, 0x0808087F, 0x08080800, 0x00000000 ),
    uvec4( 0x00000000, 0x00000000, 0x00001818, 0x08081000 ),
    uvec4( 0x00000000, 0x0000007E, 0x00000000, 0x00000000 ),
    uvec4( 0x00000000, 0x00000000, 0x00001818, 0x00000000 ),
    uvec4( 0x00000202, 0x04040808, 0x10102020, 0x40400000 ),
    uvec4( 0x0000003C, 0x42464A52, 0x6242423C, 0x00000000 ),
    uvec4( 0x00000008, 0x18280808, 0x0808083E, 0x00000000 ),
    uvec4( 0x0000003C, 0x42020204, 0x0810207E, 0x00000000 ),
    uvec4( 0x0000007E, 0x04081C02, 0x0202423C, 0x00000000 ),
    uvec4( 0x00000004, 0x0C142444, 0x7E040404, 0x00000000 ),
    uvec4( 0x0000007E, 0x40407C02, 0x0202423C, 0x00000000 ),
    uvec4( 0x0000001C, 0x2040407C, 0x4242423C, 0x00000000 ),
    uvec4( 0x0000007E, 0x02040408, 0x08101010, 0x00000000 ),
    uvec4( 0x0000003C, 0x4242423C, 0x4242423C, 0x00000000 ),
    uvec4( 0x0000003C, 0x4242423E, 0x02020438, 0x00000000 ),
    uvec4( 0x00000000, 0x00181800, 0x00001818, 0x00000000 ),
    uvec4( 0x00000000, 0x00181800, 0x00001818, 0x08081000 ),
    uvec4( 0x00000004, 0x08102040, 0x20100804, 0x00000000 ),
    uvec4( 0x00000000, 0x00007E00, 0x007E0000, 0x00000000 ),
    uvec4( 0x00000020, 0x10080402, 0x04081020, 0x00000000 ),
    uvec4( 0x00003C42, 0x02040810, 0x00001010, 0x00000000 ),
    uvec4( 0x00001C22, 0x414F5151, 0x51534D40, 0x201F0000 ),
    uvec4( 0x00000018, 0x24424242, 0x7E424242, 0x00000000 ),
    uvec4( 0x0000007C, 0x4242427C, 0x4242427C, 0x00000000 ),
    uvec4( 0x0000001E, 0x20404040, 0x4040201E, 0x00000000 ),
    uvec4( 0x00000078, 0x44424242, 0x42424478, 0x00000000 ),
    uvec4( 0x0000007E, 0x4040407C, 0x4040407E, 0x00000000 ),
    uvec4( 0x0000007E, 0x4040407C, 0x40404040, 0x00000000 ),
    uvec4( 0x0000001E, 0x20404046, 0x4242221E, 0x00000000 ),
    uvec4( 0x00000042, 0x4242427E, 0x42424242, 0x00000000 ),
    uvec4( 0x0000003E, 0x08080808, 0x0808083E, 0x00000000 ),
    uvec4( 0x00000002, 0x02020202, 0x0242423C, 0x00000000 ),
    uvec4( 0x00000042, 0x44485060, 0x50484442, 0x00000000 ),
    uvec4( 0x00000040, 0x40404040, 0x4040407E, 0x00000000 ),
    uvec4( 0x00000041, 0x63554949, 0x41414141, 0x00000000 ),
    uvec4( 0x00000042, 0x62524A46, 0x42424242, 0x00000000 ),
    uvec4( 0x0000003C, 0x42424242, 0x4242423C, 0x00000000 ),
    uvec4( 0x0000007C, 0x4242427C, 0x40404040, 0x00000000 ),
    uvec4( 0x0000003C, 0x42424242, 0x4242423C, 0x04020000 ),
    uvec4( 0x0000007C, 0x4242427C, 0x48444242, 0x00000000 ),
    uvec4( 0x0000003E, 0x40402018, 0x0402027C, 0x00000000 ),
    uvec4( 0x0000007F, 0x08080808, 0x08080808, 0x00000000 ),
    uvec4( 0x00000042, 0x42424242, 0x4242423C, 0x00000000 ),
    uvec4( 0x00000042, 0x42424242, 0x24241818, 0x00000000 ),
    uvec4( 0x00000041, 0x41414149, 0x49495563, 0x00000000 ),
    uvec4( 0x00000041, 0x41221408, 0x14224141, 0x00000000 ),
    uvec4( 0x00000041, 0x41221408, 0x08080808, 0x00000000 ),
    uvec4( 0x0000007E, 0x04080810, 0x1020207E, 0x00000000 ),
    uvec4( 0x00001E10, 0x10101010, 0x10101010, 0x101E0000 ),
    uvec4( 0x00004040, 0x20201010, 0x08080404, 0x02020000 ),
    uvec4( 0x00007808, 0x08080808, 0x08080808, 0x08780000 ),
    uvec4( 0x00001028, 0x44000000, 0x00000000, 0x00000000 ),
    uvec4( 0x00000000, 0x00000000, 0x00000000, 0x00FF0000 ),
    uvec4( 0x00201008, 0x04000000, 0x00000000, 0x00000000 ),
    uvec4( 0x00000000, 0x003C0202, 0x3E42423E, 0x00000000 ),
    uvec4( 0x00004040, 0x407C4242, 0x4242427C, 0x00000000 ),
    uvec4( 0x00000000, 0x003C4240, 0x4040423C, 0x00000000 ),
    uvec4( 0x00000202, 0x023E4242, 0x4242423E, 0x00000000 ),
    uvec4( 0x00000000, 0x003C4242, 0x7E40403E, 0x00000000 ),
    uvec4( 0x00000E10, 0x107E1010, 0x10101010, 0x00000000 ),
    uvec4( 0x00000000, 0x003E4242, 0x4242423E, 0x02023C00 ),
    uvec4( 0x00004040, 0x407C4242, 0x42424242, 0x00000000 ),
    uvec4( 0x00000808, 0x00380808, 0x0808083E, 0x00000000 ),
    uvec4( 0x00000404, 0x001C0404, 0x04040404, 0x04043800 ),
    uvec4( 0x00004040, 0x40444850, 0x70484442, 0x00000000 ),
    uvec4( 0x00003808, 0x08080808, 0x0808083E, 0x00000000 ),
    uvec4( 0x00000000, 0x00774949, 0x49494949, 0x00000000 ),
    uvec4( 0x00000000, 0x007C4242, 0x42424242, 0x00000000 ),
    uvec4( 0x00000000, 0x003C4242, 0x4242423C, 0x00000000 ),
    uvec4( 0x00000000, 0x007C4242, 0x4242427C, 0x40404000 ),
    uvec4( 0x00000000, 0x003E4242, 0x4242423E, 0x02020200 ),
    uvec4( 0x00000000, 0x002E3020, 0x20202020, 0x00000000 ),
    uvec4( 0x00000000, 0x003E4020, 0x1804027C, 0x00000000 ),
    uvec4( 0x00000010, 0x107E1010, 0x1010100E, 0x00000000 ),
    uvec4( 0x00000000, 0x00424242, 0x4242423E, 0x00000000 ),
    uvec4( 0x00000000, 0x00424242, 0x24241818, 0x00000000 ),
    uvec4( 0x00000000, 0x00414141, 0x49495563, 0x00000000 ),
    uvec4( 0x00000000, 0x00412214, 0x08142241, 0x00000000 ),
    uvec4( 0x00000000, 0x00424242, 0x4242423E, 0x02023C00 ),
    uvec4( 0x00000000, 0x007E0408, 0x1020407E, 0x00000000 ),
    uvec4( 0x000E1010, 0x101010E0, 0x10101010, 0x100E0000 ),
    uvec4( 0x00080808, 0x08080808, 0x08080808, 0x08080000 ),
    uvec4( 0x00700808, 0x08080807, 0x08080808, 0x08700000 ),
    uvec4( 0x00003149, 0x46000000, 0x00000000, 0x00000000 ),
    uvec4( 0x00000000, 0x00000000, 0x00000000, 0x00000000 )
);


//	Returns a pseudo-random value
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}


void main()	{
	
	// A character bitmap is encoded in 16 bytes, 
	// each byte represents one line of the character bitmap.
	//
	// Every 4 lines are encoded in one uint; with 
	// 4 uints encoding the full character. 
	
	vec2	inTexCoord = gl_FragCoord.xy / blockSize;
	vec2	scaledSize = RENDERSIZE / blockSize;
	
	vec2	totalCells = floor(scaledSize / glyphSize);
	vec2	thisCell = floor(inTexCoord.xy / glyphSize);
	vec2	cSeed = charCodeSeed * vec2(5.621,3.872) + vec2(1.42,2.17) + thisCell / totalCells;
	
	//	which character to draw?
	float	rando = rand(cSeed);
	uint char_code = uint(96.0 * rando);
	
	if (char_code > 95)	{
		char_code = 95;
	}

	//	what data do we draw for this code? the uvec4 contains it!
	uvec4 char_data = font_data[char_code];
	
	//	A character bitmap is encoded in 16 bytes, 
	//	each byte represents one line of the character bitmap.
	//	Every 4 lines are encoded in one uint
	//	4 uints encoding the full character. 

	//	get the local coordinates
	vec2	uv = gl_FragCoord.xy;
	uv = mod(uv, glyphSize);
	
	//	now get our row within fourRowsData 
	uint	localRow = uint(mod(uv.y - 1.0, 4.0));

	//	flip the y so we start from the top left instead of the bottom left
	uv.y = glyphSize.y - uv.y + 1.0;

	//	figure out which component we need based on the line number
	//	note that the original frag shader from Tim has a much more elegant & efficient way of doing this
	//	right now uv.y contains a value from 0-15
	//	byteData will hold four rows worth of data
	uint	fourRowsData = 0;
	if (uv.y < 4.0)
		fourRowsData = char_data.x;
	else if (uv.y < 8.0)
		fourRowsData = char_data.y;
	else if (uv.y < 12.0)
		fourRowsData = char_data.z;
	else if (uv.y < 16.0)
		fourRowsData = char_data.w;
	
	uint	localCol = uint(uv.x);
	
	//	NOW get just the data for this row
	uint	current_line  = fourRowsData >> (8*localRow) & uint(0xff);
	
	//	NOW get just the data for this column on this row
	uint 	current_pixel = (current_line >> (7-localCol)) & uint(0x01);
	
	vec4	out_color = mix(bgColor, charColor, float(current_pixel));
	
	//out_color += vec4(uv.xy / glyphSize,0.0,1.0);
	//out_color += vec4(uv.x / glyphSize.x,0.0,mod(float(localRow),4.0) / 4.0,1.0);
	
	//	now draw the bgColor or charColor from the inputs
	gl_FragColor = out_color;

}