/*
// A class for generating 2D Grids of Noise for mapmaking
//
//
//
*/
using UnityEngine;
using System.Collections;

public class Noise : MonoBehaviour {

	private float[][] noise;
	private float[][] smooth_noise;
	private int width = 0;
	private int length = 0;

	public Noise(int x, int y){
		this.noise = new float[x,y];
		this.width = x;
		this.length = y;
		this.generateWhiteNoise();
		this.generateSmoothNoise();
	}

	public void generateWhiteNoise(){
		for(int i=0; i<width; i++){
			for (int j=0; j<length; j++){
				this.noise[i,j] = (float)UnityEngine.Random.Range(0.0,1.0);
			}
		}
	}

	public void generateSmoothNoise(){
		int samplePeriod = 1;
		float sampleFrequency = 1.0f/ (float)samplePeriod;
		smooth_noise = new float[width,length];

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
	}

	public float Interpolate(float x0, float x1, float alpha){
	   return x0 * (1 - alpha) + alpha * x1;
	}

	public getNoiseResults(){
		return noise;
	}

}