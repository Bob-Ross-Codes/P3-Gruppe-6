// This shader is from https://www.google.com/search?q=raindrops+unity&sca_esv=a7fad6824b8365dd&sxsrf=ADLYWILvuP66SrrW-C477kdeudL5EgUMFg%3A1729762179379&ei=gxMaZ9CAFuyci-gPk5TAKQ&ved=0ahUKEwiQzvyh2qaJAxVszgIHHRMKMAUQ4dUDCBA&uact=5&oq=raindrops+unity&gs_lp=Egxnd3Mtd2l6LXNlcnAiD3JhaW5kcm9wcyB1bml0eTIFECEYoAEyBRAhGKABMgUQIRifBUixEFCKA1i7DHABeAGQAQCYAVqgAccDqgEBNrgBA8gBAPgBAZgCB6AC2APCAgoQABiwAxjWBBhHwgINEAAYgAQYsAMYQxiKBcICDhAAGLADGOQCGNYE2AEBwgITEC4YgAQYsAMYQxjIAxiKBdgBAcICChAAGIAEGEMYigXCAgoQLhiABBhDGIoFwgIIEC4YgAQYywHCAggQABiABBjLAcICBRAAGIAEwgIFEC4YgATCAgsQABiABBiRAhiKBcICBhAAGBYYHsICCBAAGBYYHhgPwgILEAAYgAQYhgMYigXCAggQABiABBiiBJgDAIgGAZAGE7oGBggBEAEYCZIHATegB-0q&sclient=gws-wiz-serp#fpstate=ive&vld=cid:e2381e77,vid:dtP_DWRQ9GE,st:0

Shader "Custom/Raindrop" {
    Properties {
        iChannel0("Albedo (RGB)", 2D) = "white" {}  // Texture input for the base (albedo) surface.
    }
    SubShader {
        Tags { "RenderType"="Opaque" }  // Marks the shader as an opaque surface.
        LOD 200  // Level of detail set to 200, indicating a reasonably complex shader.

        Pass {
            CGPROGRAM

            #pragma vertex vert_img  // Use Unity's built-in vertex shader for 2D texture mapping.
            #pragma fragment frag  // Use the custom fragment shader defined below.

            #include "UnityCG.cginc"  // Include Unity's common shader utilities and functions.

            sampler2D iChannel0;  // Declare the texture sampler for iChannel0 (albedo map).

            // Define macros for smoothstep and conditional shader settings.
            #define S(a, b, t) smoothstep(a, b, t)
            //#define CHEAP_NORMALS  // Option to use cheap normals (commented out for now).
            #define HAS_HEART  // Enables the heart effect.
            #define USE_POST_PROCESSING  // Enables post-processing effects.

            // Generate a pseudo-random float3 based on input value p.
            float3 N13(float p) {
                //  from DAVE HOSKINS
                float3 p3 = frac(float3(p, p, p) * float3(.1031, .11369, .13787));  // Generate fractional part for randomness.
                p3 += dot(p3, p3.yzx + 19.19);  // Combine values for more randomness.
                return frac(float3((p3.x + p3.y)*p3.z, (p3.x + p3.z)*p3.y, (p3.y + p3.z)*p3.x));  // Return the resulting random value.
            }

            // Generate a random float4 based on input time value t.
            float4 N14(float t) {
                return frac(sin(t*float4(123., 1024., 1456., 264.))*float4(6547., 345., 8799., 1564.));
            }

            // Generate a random float based on input value t.
            float N(float t) {
                return frac(sin(t*12345.564)*7658.76);
            }

            // A sawtooth wave function with smoothing.
            float Saw(float b, float t) {
                return S(0., b, t)*S(1., b, t);  // Combines smoothstep functions to create the sawtooth effect.
            }

            // Calculate the first layer of raindrops on the UV coordinates and time.
            float2 DropLayer2(float2 uv, float t) {
                float2 UV = uv;  // Copy the original UV.
                uv.y += t*0.75;  // Animate the UV over time (vertical movement).
                float2 a = float2(6., 1.);  // Aspect ratio of the raindrop grid.
                float2 grid = a*2.;  // Calculate grid size.
                float2 id = floor(uv*grid);  // Get the current grid cell.

                float colShift = N(id.x);  // Random offset for column shifts.
                uv.y += colShift;  // Apply the column shift to the UV.

                id = floor(uv*grid);  // Recalculate grid cell after shift.
                float3 n = N13(id.x*35.2 + id.y*2376.1);  // Random value based on grid ID.
                float2 st = frac(uv*grid) - float2(.5, 0);  // Fractional part of UV.

                float x = n.x - .5;  // Adjust x position of the raindrop center.

                // Add a wiggle effect to the raindrop position.
                float y = UV.y*20.;
                float wiggle = sin(y + sin(y));  // Sinusoidal wiggle effect.
                x += wiggle*(.5 - abs(x))*(n.z - .5);  // Adjust x based on wiggle and randomness.
                x *= .7;  // Further adjust x scaling.
                float ti = frac(t + n.z);  // Time-based randomization for raindrop appearance.
                y = (Saw(.85, ti) - .5)*.9 + .5;  // Y-position adjustment for the drop's tail.
                float2 p = float2(x, y);  // Final drop position.

                // Calculate distance to the center of the raindrop.
                float d = length((st - p)*a.yx);

                float mainDrop = S(.4, .0, d);  // Main raindrop using smoothstep.

                // Create a trail effect behind the raindrop.
                float r = sqrt(S(1., y, st.y));  // Radius for the drop trail.
                float cd = abs(st.x - x);  // Horizontal distance from the drop.
                float trail = S(.23*r, .15*r*r, cd);  // Trail thickness.
                float trailFront = S(-.02, .02, st.y - y);  // Front of the trail.
                trail *= trailFront*r*r;  // Apply trail front effect.

                // Create small droplet effect behind the main drop.
                y = UV.y;
                float trail2 = S(.2*r, .0, cd);  // Trail falloff.
                float droplets = max(0., (sin(y*(1. - y)*120.) - st.y))*trail2*trailFront*n.z;  // Small droplet effect.
                y = frac(y*10.) + (st.y - .5);  // Droplet spacing.
                float dd = length(st - float2(x, y));  // Distance for the droplet calculation.
                droplets = S(.3, 0., dd);  // Drop intensity.
                float m = mainDrop + droplets*r*trailFront;  // Combine the main drop and droplets.

                return float2(m, trail);  // Return the final raindrop and trail value.
            }

            // Function to calculate static raindrops.
            float StaticDrops(float2 uv, float t) {
                uv *= 40.;  // Scale UV for small raindrops.
                float2 id = floor(uv);  // Get the grid ID for static drops.
                uv = frac(uv) - .5;  // Calculate the fractional part of UV.
                float3 n = N13(id.x*107.45 + id.y*3543.654);  // Generate random drop position.
                float2 p = (n.xy - .5)*.7;  // Random position of the static drop.
                float d = length(uv - p);  // Distance to the drop center.

                float fade = Saw(.025, frac(t + n.z));  // Fade effect for the drop appearance.
                float c = S(.3, 0., d)*frac(n.z*10.)*fade;  // Drop intensity with random fade.
                return c;
            }

            // Combines static and dynamic raindrop layers.
            float2 Drops(float2 uv, float t, float l0, float l1, float l2) {
                float s = StaticDrops(uv, t)*l0;  // Static raindrops.
                float2 m1 = DropLayer2(uv, t)*l1;  // First dynamic layer of raindrops.
                float2 m2 = DropLayer2(uv*1.85, t)*l2;  // Second dynamic layer.

                float c = s + m1.x + m2.x;  // Combine all the layers.
                c = S(.3, 1., c);  // Smoothstep for final drop intensity.

                return float2(c, max(m1.y*l0, m2.y*l1));  // Return the combined raindrop and trail.
            }

            // Fragment (pixel) shader.
            fixed4 frag(v2f_img i) : SV_Target {
                float2 uv = ((i.uv * _ScreenParams.xy) - .5*_ScreenParams.xy) / _ScreenParams.y;  // Normalize UV coordinates.
                float2 UV = i.uv.xy;  // Store the original UV coordinates.
                float3 M = float3(0.0, 0.0, 0.0);  // Placeholder for input control (e.g., mouse).
                float T = _Time.y + M.x*2.;  // Time-based animation.

                #ifdef HAS_HEART
                T = fmod(_Time.y, 102.);  // Loop time when "HAS_HEART" is enabled.
                T = lerp(T, M.x*102., M.z>0. ? 1. : 0.);  // Blend time with input.
                #endif

                float t = T*.2;  // Scale time for the raindrop animation.

                float rainAmount = M.y;  // Amount of rain controlled by input.

                float maxBlur = lerp(3., 6., rainAmount);  // Maximum blur intensity based on rain.
                float minBlur = 2.;  // Minimum blur intensity.

                float story = 0.;  // Initialize story variable for heart effect.
                float heart = 0.;  // Initialize heart variable for heart effect.

                #ifdef HAS_HEART
                // Heart effect logic (conditional on HAS_HEART).
                float r = length(UV - .5);  // Distance from center.
                heart = S(.5, .0, r);  // Create heart effect using smoothstep.
                float3 NH = N13(100.);  // Random value for heart appearance.
                story = step(.75, S(.2, .25, sin(T*.5)) + NH.x*.8);  // Story progression logic.
                #endif

                // Calculate the drops and trails.
                float2 drops = Drops(UV, t, 1., .5, .2);
                float blur = lerp(minBlur, maxBlur, drops.y);  // Interpolate blur intensity based on trails.

                fixed4 col = tex2D(iChannel0, i.uv);  // Sample the texture at the given UV coordinates.

                col.rgb *= drops.x;  // Modulate color by the raindrop intensity.

                return col;  // Output the final color.
            }

            ENDCG
        }
    }
    FallBack "Diffuse"  // Fallback to Diffuse shader if this one is not supported.
}
