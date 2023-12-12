import {vec2} from 'gl-matrix';

function random2(p: vec2) {
    let v1 = Math.sin(vec2.dot(p, vec2.fromValues(127.1, 311.7)));
    let v2 = Math.sin(vec2.dot(p, vec2.fromValues(269.5, 183.3)));
    return vec2.fromValues((v1 * 43758.5453)%1, (v2 * 43758.5453)%1);
}

function perlinNoise(uv1: vec2) {
	let surfletSum = 0;

    let uv = vec2.create();
    vec2.scale(uv, uv1, 0.04);

    let uv_floor = vec2.create();
    vec2.floor(uv_floor, uv);

	// Iterate over the four integer corners surrounding uv
	for(let dx = 0; dx <= 1; ++dx) {
		for(let dy = 0; dy <= 1; ++dy) {
            let new_gp = vec2.create();
            vec2.add(new_gp, uv_floor, vec2.fromValues(dx, dy))
			surfletSum += surflet(uv, new_gp);
		}
	}
	return surfletSum;
}

function surflet(P: vec2, gridPoint: vec2) {
    // Compute falloff function by converting linear distance to a polynomial
    let distX = Math.abs(P[0] - gridPoint[0]);
    let distY = Math.abs(P[1] - gridPoint[1]);
    let tX = 1.0 - 6.0 * Math.pow(distX, 5) + 15.0 * Math.pow(distX, 4) - 10.0 * Math.pow(distX, 3);
    let tY = 1.0 - 6.0 * Math.pow(distY, 5) + 15.0 * Math.pow(distY, 4) - 10.0 * Math.pow(distY, 3);
    // Get the random vector for the grid point
    let gradient = vec2.create();
    let scaled_gp = vec2.create();
    vec2.scale(scaled_gp, random2(gridPoint), 2)
    vec2.sub(gradient, scaled_gp, vec2.fromValues(1, 1));
    // Get the vector from the grid point to P
    let diff = vec2.create();
    vec2.sub(diff, P, gridPoint);
    // Get the value of our height field by dotting grid->P with our gradient
    let height = vec2.dot(diff, gradient);
    // Scale our height field (i.e. reduce it) by our polynomial falloff function
    return height * tX * tY;
}

export default perlinNoise;
