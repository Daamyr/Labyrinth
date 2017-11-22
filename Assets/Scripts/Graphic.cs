using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicManager {

	private static GraphicManager instance;

	Material mat = new Material(new Shader());

	Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
	private static Hashtable loadedTextures = new Hashtable ();

	private GraphicManager(){
		textures.Add("wall", (Texture2D)Resources.Load("tex_wall"));
	}
	


	public static GraphicManager getInstance(){
		if (instance == null) {
			instance = new GraphicManager ();
		}

		return instance;
	}

	public Texture2D LoadTexture (string key){
		if (loadedTextures.Contains (key)) {
			return loadedTextures [key] as Texture2D;
		} else {
			Debug.Log ("Unable to load " + key);
			return null;
		}
	}

	public Material getMat (){
		return mat;
	}
}
//
//public class WallTexture : GraphicManager { 
//
//	private static WallTexture instance;
//
//	private WallTexture(){
//
//	}
//
//	public GraphicManager LoadTexture(string key){
//		if (this.instance == null) {
//			instance = new WallTexture ();
//			Debug.Log ("Wall texture is loaded for the first time " + this);
//		} else {
//			Debug.Log ("Wall texture has already been loaded " + this);
//		}
//
//		GraphicManager instance;
//
//	}
//
//}
