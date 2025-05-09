/*{
    "CATEGORIES": [
        "Overlay", "Utility"
    ],
    "CREDIT": "VIDVOX",
    "DESCRIPTION": "Draws a small (80x48 pixel) time / date stamp.",
    "INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
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
                0,
                0,
                0,
                1.0
            ],
            "NAME": "bgColor",
            "TYPE": "color"
        },
        {
            "LABEL": "24 Hours",
            "NAME": "hours24",
            "TYPE": "bool",
            "DEFAULT": 0
        },
        {
            "DEFAULT": 1,
            "LABEL": "Month First",
            "NAME": "showMonthFirst",
            "TYPE": "bool"
        },
        {
            "DEFAULT": 0,
            "LABEL": "Show at top",
            "NAME": "showAtTop",
            "TYPE": "bool"
        },
        {
            "DEFAULT": 1,
            "LABEL": "Show on right",
            "NAME": "showOnRight",
            "TYPE": "bool"
        }
    ],
    "ISFVSN": "2"
}
*/


//	Inspired / adapted from Tim Gfrerer's Texture-less Text Rendering blog post:
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


int charCodeForDigit(int digit)	{
	if (digit < 0 || digit > 9)
		return 56;
	//	the char code for 0 is 16, 1 is 17, ... 9 is 25
	return digit + 16;
}

//	this converts a 2 digit number (0-99) into character codes
ivec2 charsForInt2Digit(int val)	{
	ivec2	iChars = ivec2(0);
	
	//	store the 10s in the x
	iChars.x = charCodeForDigit(val / 10);
	
	//	store the 1s in the y
	int	tmp = val / 10;
	tmp *= 10;
	iChars.y = charCodeForDigit((val - tmp));
	
	return iChars;
}

//	this converts a 4 digit number (0-9999) into character codes
ivec4 charsFor4Digit(float val, bool decimals)	{
	ivec4	iChars = ivec4(0);
	int		valAsInt = int(val);
	//	if we're not doing decimals, or the value is over 100, return 4 full digits
	//	eg if the number is 103.23, with the decimal place, there isn't room for anything after
	if (!decimals || val >= 100.0)	{
		//	store the 1000s in the x
		iChars.x = charCodeForDigit(valAsInt / 1000);
	
		//	store the 100s in the y
		int	tmp = valAsInt / 1000;
		tmp *= 1000;
		iChars.y = charCodeForDigit((valAsInt - tmp)/100);
	
		//	store the 10s in the z
		tmp = valAsInt / 100;
		tmp *= 100;
		iChars.z = charCodeForDigit((valAsInt - tmp)/10);
	
		//	store the 1s in the w
		tmp = valAsInt / 10;
		tmp *= 10;
		iChars.w = charCodeForDigit((valAsInt - tmp));
	}
	//	but for 97.34, there is an extra after the decimal
	else if (val > 10.0)	{
		//	get the 2 digits
		iChars.xy = charsForInt2Digit(valAsInt);
		//	char code 14 is '.'
		iChars.z = 14;
		iChars.w = charCodeForDigit(int(val * 10.0) - valAsInt * 10);
	}
	//	or for 3.872, there are two spaces after the decimal
	else	{
		//	get the first digit
		iChars.x = charCodeForDigit(valAsInt);
		//	char code 14 is '.'
		iChars.y = 14;
		//	now get first two digits after the decimal point
		iChars.zw = charsForInt2Digit(int(val * 100.0) - valAsInt * 100);
	}
	
	return iChars;
}

uint charCodeForDateByIndex(vec3 d, uint tIndex, bool monthFirst)	{
	//	The first element of the vector is the year, the second element is the month, the third element is the day
	uint	char_code = 0;

	//	do the / marks
	if (tIndex == 2 || tIndex == 5)	{
		char_code = 15;
	}
	//	month (or day for european)
	else if (tIndex < 2)	{
		float	tmpVal = (monthFirst) ? d.y : d.z;
		ivec2	iChars = charsForInt2Digit(int(tmpVal));
		int		localIndex = int(mod(float(tIndex), 2.0));
		char_code = iChars[localIndex];
	}
	//	day (or month for european)
	else if (tIndex < 5)	{
		float	tmpVal = (monthFirst) ? d.z : d.y;
		ivec2	iChars = charsForInt2Digit(int(tmpVal));
		int		localIndex = int(mod(float(tIndex - 3), 2.0));
		char_code = iChars[localIndex];
	}
	//	year (four digits)
	else if (tIndex < 10)	{
		float	year = d.x;
		ivec4	iChars = charsFor4Digit(int(year), false);
		int		localIndex = int(mod(float(tIndex - 6), 4.0));
		char_code = iChars[localIndex];
	}
	
	return char_code;	
}

uint charCodeForTimeInSecondsByIndex(float timeInSec, uint tIndex, bool showAMPM)	{
	
	uint	char_code = 0;

	//	do the colons
	if (tIndex == 2 || tIndex == 5)	{
		char_code = 26;
	}
	//	hours
	else if (tIndex < 2)	{
		float	hours = timeInSec / (60.0 * 60.0);
		if (showAMPM)	{
			hours = mod(hours, 12.0);
		}
		ivec2	iChars = charsForInt2Digit(int(hours));
		int		localIndex = int(mod(float(tIndex), 2.0));
		char_code = iChars[localIndex];
	}
	//	minutes
	else if (tIndex < 5)	{
		float	hours = floor(timeInSec / (60.0 * 60.0));
		float	mins = (timeInSec / 60.0) - hours * 60.0;
		ivec2	iChars = charsForInt2Digit(int(mins));
		int		localIndex = int(mod(float(tIndex - 3), 2.0));
		char_code = iChars[localIndex];
	}
	//	seconds
	else if (tIndex < 8)	{
		float	mins = floor(timeInSec / 60.0);
		float	secs = (timeInSec - mins * 60.0);
		ivec2	iChars = charsForInt2Digit(int(secs));
		int		localIndex = int(mod(float(tIndex - 6), 2.0));
		char_code = iChars[localIndex];
	}
	else if (showAMPM && tIndex < 10)	{
		float	hours = timeInSec / (60.0 * 60.0);
		bool	isPM = (hours >= 12.0);
		ivec2	iChars = (isPM) ? ivec2(48,45) : ivec2(33,45);
		int		localIndex = int(mod(float(tIndex - 6), 2.0));
		char_code = iChars[localIndex];

	}
	return char_code;
}

uint charCodeForTimeStampAtCell(uvec2 cellCoord)	{
	uint	char_code = 0;
	
	//	show the date
	if (cellCoord.y == 0)	{
		//	The first element of the vector is the year, the second element is the month, the third element is the day, and the fourth element is the time (in seconds) within the day.
		char_code = charCodeForDateByIndex(DATE.xyz, cellCoord.x, showMonthFirst);
	}
	//	show time from date
	else if (cellCoord.y == 1)	{
		//	The first element of the vector is the year, the second element is the month, the third element is the day, and the fourth element is the time (in seconds) within the day.
		//	if we're doing not doing the AM / PM at the end, shift over by 1 so this appears centered
		//	(the date and time readout are both 10 characters wide
		uint	startIndex = (!hours24) ? cellCoord.x : cellCoord.x - 1;
		char_code = charCodeForTimeInSecondsByIndex(DATE.w, startIndex, !hours24);
	}
	//	show running time
	else if (cellCoord.y == 2)	{
		//	do T+ in front
		if (cellCoord.x < 1)	{
			char_code = 52;
		}
		else if (cellCoord.x < 2)	{
			char_code = 11;
		}
		else	{
			char_code = charCodeForTimeInSecondsByIndex(TIME, cellCoord.x - 2, false);
		}
	}
	
	return char_code;
}

float fillForCharCodeAtPixel(uint charCode, vec2 inTexCoord)	{
	//	what data do we draw for this code? the uvec4 contains it!
	uvec4 char_data = font_data[charCode];

	
	//	A character bitmap is encoded in 16 bytes, 
	//	each byte represents one line of the character bitmap.
	//	Every 4 lines are encoded in one uint
	//	4 uints encoding the full character. 

	//	get the local coordinates
	vec2	uv = inTexCoord.xy;
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
	
	return float(current_pixel);
}

void main()	{
	
	// A character bitmap is encoded in 16 bytes, 
	// each byte represents one line of the character bitmap.
	//
	// Every 4 lines are encoded in one uint; with 
	// 4 uints encoding the full character. 
	
	vec4	out_color = IMG_THIS_PIXEL(inputImage);
	vec2	inTexCoord = gl_FragCoord.xy;
	
	//	figure out which cell we are in, starting from the top left corner
	ivec2	totalCells = ivec2(RENDERSIZE / glyphSize);
	ivec2	thisCell = ivec2(inTexCoord.xy / glyphSize);
	
	//	do this so we get an extra blank space in the lead
	thisCell.x = thisCell.x;
	
	if (showOnRight)	{
		thisCell.x -= (totalCells.x - 10);
	}
	
	if (showAtTop)	{
		thisCell.y = totalCells.y - thisCell.y - 1;
	}
	
	if (thisCell.y < 3 && thisCell.x < 10 && thisCell.x >= 0)	{	
		//	which character to draw?
		uint char_code = charCodeForTimeStampAtCell(uvec2(thisCell));
		//	debug - show the index of the cell
		//uint char_code = charCodeForDigit(thisCell.x);
	
		vec2	uv = inTexCoord.xy;
		uv = mod(uv, glyphSize);
		float	current_pixel = fillForCharCodeAtPixel(char_code, uv);
	
		vec4	overlay = mix(bgColor, charColor, float(current_pixel));
		
		out_color = mix(out_color, overlay, overlay.a);
		
		//	debug
		//out_color += vec4(uv.xy / glyphSize,0.0,1.0);
		//out_color += vec4(uv.x / glyphSize.x,0.0,mod(float(localRow),4.0) / 4.0,1.0);
	}
	
	//	now draw the bgColor or charColor from the inputs
	gl_FragColor = out_color;
	
}
