import { vec2 } from "gl-matrix";
import { SimplexNoise } from "ts-perlin-simplex";

var simplex = new SimplexNoise();

function fbm(x: number, y: number, o: number) {
    let total = 0.0;
    let persistence = 0.5;
    let octaves = o;
    let freq = 2.0;
    let amp = 0.5;
    for(let i = 1; i <= octaves; i++) {
        // total += interpNoise2D(x * freq,
        //                        y * freq) * amp;
        total += simplex.noise(x*freq, y*freq) * amp;

        freq *= 2.0;
        amp *= persistence;
    }
    return total;
}

function interpNoise2D(x: number, y: number) {
    let intX = Math.floor(x);
    let fractX = x%1;
    let intY = Math.floor(y);
    let fractY = y%1;

    let v1 = simplex.noise(intX, intY);
    let v2 = simplex.noise(intX + 1, intY);
    let v3 = simplex.noise(intX, intY + 1);
    let v4 = simplex.noise(intX + 1, intY + 1);

    let i1 = v1*fractX + v2*(1-fractX);
    let i2 = v3*fractX + v4*(1-fractX);
    return i1*fractY + i2*(1-fractY);
}


// function rand(x: number, y: number){
// 	return fract(sin(dot(c.xy ,vec2(12.9898,78.233))) * 43758.5453);



// }

// function noise(x: number, y: number, freq: number){
// 	let unit = 1;//screenWidth/freq;
// 	let ij = vec2.fromValues(Math.floor(x/unit), Math.floor(y/unit));
// 	let xy = vec2.fromValues((x%unit)/unit, (y%unit)/unit);
// 	//xy = 3.*xy*xy-2.*xy*xy*xy;
// 	xy = .5*(1.-cos(PI*xy));
// 	let a = rand((ij+vec2(0.,0.)));
// 	let b = rand((ij+vec2(1.,0.)));
// 	let c = rand((ij+vec2(0.,1.)));
// 	let d = rand((ij+vec2(1.,1.)));
// 	float x1 = mix(a, b, xy.x);
// 	float x2 = mix(c, d, xy.x);
// 	return mix(x1, x2, xy.y);
// }

// function pNoise(vec2 p, int res){
// 	let persistance = .5;
// 	let n = 0.;
// 	let normK = 0.;
// 	let f = 4.;
// 	let amp = 1.;
// 	let iCount = 0;
// 	for (let i = 0; i<50; i++){
// 		n+=amp*noise(p, f);
// 		f*=2.;
// 		normK+=amp;
// 		amp*=persistance;
// 		if (iCount == res) break;
// 		iCount++;
// 	}
// 	let nf = n/normK;
// 	return nf*nf*nf*nf;
// }

export default fbm;
