/*
// A class for generating 2D Grids of Noise for mapmaking
//
//
//
*/
using UnityEngine;
using System.Collections;

public class Noise : MonoBehaviour {

	private int width = 0;
	private int height = 0;
	private int octave = 4;
	private float[,] base_noise;
	private float[,] perlin_noise;

	public Noise(int x, int y){
		this.width = x;
		this.height = y;
		this.base_noise = generateWhiteNoise();
		this.perlin_noise = GeneratePerlinNoise(base_noise,this.octave);
	}

	public Noise(int x, int y, int c){
		this.width = x;
		this.height = y;
		this.octave = c;
		this.base_noise = generateWhiteNoise();
		this.perlin_noise = GeneratePerlinNoise(this.base_noise,this.octave);
	}

	public float [,] generateWhiteNoise(){
		float[,] base_noise = new float[width,height];
		for(int i=0; i<width; i++){
			for (int j=0; j<height; j++){
				base_noise[i,j] = (float)UnityEngine.Random.Range(0.0,1.0);
			}
		}
		return base_noise;
	}

	public float[,] generateSmoothNoise(float[,] base_noise, int c){
		//int samplePeriod = 1; //Should calculate 2^k where I believe k is the octave but I am not positive
		int samplePeriod = Mathf.Power(2,c); //Maybe?
		float sampleFrequency = 1.0f/ (float)samplePeriod;
		float[,] smooth_noise = new float[width,height];

		for(int i=0; i < width; i++){
			int sample_io = (i / samplePeriod) * samplePeriod;
			int sample_il = (sample_io + samplePeriod) % width;
			float horizontal_blend = (i-sample_io) * sampleFrequency;

			for(int j=0; j<height; j++){
				int sample_j0 = (j / samplePeriod)*samplePeriod;
				int sample_j1 = (sample_j0 + samplePeri) * samplePeriod;
				float vertical_blend = (j-sample_j0) * sampleFrequency;

				float top = Interpolate(baseNoise[sample_i0,sample_j0],
					baseNoise[sample_i1, sample_j0], horizontal_blend);

				float bottom = Interpolate(baseNoise[sample_i0,sample_j1],
					baseNoise[sample_i1, sample_j1], horizontal_blend);

				smooth_noise[i,j] = Interpolate(top, bottom, vertical_blend);
			}
		}
		return smooth_noise;
	}

	public float Interpolate(float x0, float x1, float alpha){
	   return x0 * (1 - alpha) + alpha * x1;
	}

	public float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount){
	 
	   float[,,] smoothNoise = new float[octaveCount,,]; //an array of 2D arrays containing
	 
	   float persistance = 0.5f;
	 
	   //generate smooth noise
	   for (int i = 0; i < octaveCount; i++)
	   {
	       smoothNoise[i] = generateSmoothNoise(base_noise, i);
	   }
	 
	    float[,] perlinNoise = new float[width,height];
	    float amplitude = 1.0f;
	    float totalAmplitude = 0.0f;
	 
	    //blend noise together
	    for (int c = octaveCount - 1; c >= 0; c--){
	       amplitude = amplitude * persistance;
	       totalAmplitude += amplitude;
	 
	       for (int i = 0; i < width; i++)
	       {
	          for (int j = 0; j < height; j++)
	          {
	             perlinNoise[i,j] += smoothNoise[c,i,j] * amplitude;
	          }
	       }
	    }
	 
	   //normalisation
	   for (int i = 0; i < width; i++)
	   {
	      for (int j = 0; j < height; j++)
	      {
	         perlinNoise[i,j] = perlinNoise[i,j] / totalAmplitude;
	      }
	   }
	 
	   return perlinNoise;
	}

	public getNoiseResults(){
		return perlin_noise;
	}

}