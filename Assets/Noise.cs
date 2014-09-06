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
	private double[,] base_noise;
	private double[,] perlin_noise;

	public Noise(int x, int y){
		this.width = x;
		this.height = y;
		this.base_noise = generateWhiteNoise();
		this.perlin_noise = GeneratePerlinNoise(base_noise,4);
	}

	public Noise(int x, int y, int c){
		this.width = x;
		this.height = y;
		this.octave = c;
		this.base_noise = generateWhiteNoise();
		this.perlin_noise = GeneratePerlinNoise(this.base_noise,this.octave);
	}

	public double [,] generateWhiteNoise(){
		double[,] base_noise = new double[width,height];
		for(int i=0; i<width; i++){
			for (int j=0; j<height; j++){
				base_noise[i,j] = (double)UnityEngine.Random.Range(0.0f,1.0f);
			}
		}
		return base_noise;
	}

	public double[,] generateSmoothNoise(double[,] baseNoise, int c){
		//int samplePeriod = 1; //Should calculate 2^k where I believe k is the octave but I am not positive
		int samplePeriod = (int)Mathf.Pow(2.0f,c*1.0f); //Maybe?
		Debug.Log(samplePeriod);
		double sampleFrequency = (double)Mathf.Floor(1.0f/ samplePeriod);
		double[,] smooth_noise = new double[width,height];
		//int sample_i0 = 0;
		//int sample_i1 = 0;
int count = 0;
		for(int i=0; i < width; i++){
                if(count > ((width)*(height))){
                    break;
                }
			int sample_i0 = (int)((Mathf.Floor(i / samplePeriod)) * samplePeriod);
			int sample_i1 = (sample_i0 + samplePeriod) % width;
			double horizontal_blend = (i-sample_i0) * sampleFrequency;

			for(int j=0; j<height; j++){
				int sample_j0 = (int)((Mathf.Floor(j / samplePeriod))*samplePeriod);
				int sample_j1 = (sample_j0 + samplePeriod) * samplePeriod;
				double vertical_blend = (j-sample_j0) * sampleFrequency;

				if(sample_i0 >= width || sample_i1 >= width || sample_j1 >=height || sample_j0 >=height){
					break;
				}

				double top = Interpolate(baseNoise[sample_i0,sample_j0],
					baseNoise[sample_i1, sample_j0], horizontal_blend);
				sample_j1 -= 1;
				Debug.Log(width + " " + height + " " + sample_j1 + " " + sample_i1);
				double bottom = Interpolate(baseNoise[sample_i0,sample_j1],
					baseNoise[sample_i1, sample_j1], horizontal_blend);
				//double bottom = 0.0;
				smooth_noise[i,j] = Interpolate(top, bottom, vertical_blend);
			}
		}
		return smooth_noise;
	}

	public double Interpolate(double x0, double x1, double alpha){
	   return x0 * (1 - alpha) + alpha * x1;
	}

	public double[,] GeneratePerlinNoise(double[,] baseNoise, int octaveCount){
	 
	   double[,,] smoothNoise = new double[octaveCount,width,height]; //an array of 2D arrays containing
	 
	   double persistance = 0.5f;
	 
	   //generate smooth noise
	   for (int i = 0; i < octaveCount; i++)
	   {
	       double [,] smoothNoiseIteration = generateSmoothNoise(base_noise, i);
	       for(int x = 0; x < width; x++){
	       	for(int y = 0; y < height; y++){
	       		smoothNoise[i,x,y] = smoothNoiseIteration[x,y];
	       	}
	       }
	   }
	 
	    double[,] perlinNoise = new double[width,height];
	    double amplitude = 1.0f;
	    double totalAmplitude = 0.0f;
	 
	    //blend noise together

	    for (int c = octaveCount - 1; c >= 0; c--){
	       amplitude *= persistance;
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

	public double [,] getNoise(){
		return perlin_noise;
	}

}