// Toolbox
float rxEase(float x, float k)
{
	k = clamp(k, 0.0001, 10000.0) - 1.0; // clamp optional, if you know your k
	x = clamp(x, 0.0, 1.0);
	float kx = k * x;
	return (x + kx) / (kx + 1.0);
}

float GetBias(float t, float bias)
{
	return (t / ((((1.0 / bias) - 2.0) * (1.0 - t)) + 1.0));
}

float GetGain(float t, float gain)
{
	if (t < 0.5)
		return GetBias(t * 2.0, gain) / 2.0;
	else
		return GetBias(t * 2.0 - 1.0, 1.0 - gain) / 2.0 + 0.5;
}

void SphereToUV_float(float3 Direction, out float2 UV) {
	float phi = atan2(Direction.z, Direction.x);
	if (phi < 0.0) {
		phi += 2.0 * PI;
	}
	float theta = acos(clamp(Direction.y, -0.999999, 0.999999));
	UV = float2(1.0 - phi / (2.0 * PI), 1.0 - theta / PI);
}

void GetSunDirection_float(float Time, float Cycle, out float3 SunDirection) {
	Time = frac(Time / Cycle) * Cycle;
	float angle = float(Time) * 2.0 * PI / float(Cycle);
	SunDirection = float3(0.0, sin(angle), cos(angle));
}

// Differ in different biome

// float3 SkyColor_Night, float3 HorizonColor_Night, // Night
void GetGradientSkyColor_float(float2 SkyUV/*[0, 1]*/,
	float3 SkyColor_Day, float3 HorizonColor_Day, float3 GroundColor_Day, // Day
	out float3 Color) {

	if (SkyUV.y < -0.1) Color = GroundColor_Day;
	else if (SkyUV.y < 0.0) Color = lerp(GroundColor_Day, HorizonColor_Day, (SkyUV.y + 0.1) / 0.1);
	else if (SkyUV.y < 0.2) Color = lerp(HorizonColor_Day, SkyColor_Day, rxEase((SkyUV.y) / 0.2, 3.0));
	else Color = SkyColor_Day;
}

