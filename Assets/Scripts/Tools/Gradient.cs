using System;
using UnityEngine;

namespace Tools {
	public class Gradient : MonoBehaviour {
		
		private static Gradient _this;
	
		public Color TopColor, BottomColor;
		public Shader Shader;

		private Color _topLeftColor, _topRightColor, _btmRightColor, _btmLeftColor;
		private Mesh _mesh; 
		private Camera _camera;
		private GameObject _background;
	
		public static Color TopLeftColor {
			get { return _this._topLeftColor; }
			set { _this._topLeftColor = value; _this.setMeshColors(); }
		}
	
		public static Color TopRightColor {
			get { return _this._topRightColor; }
			set { _this._topRightColor = value; _this.setMeshColors(); }
		}

		public static Color BtmLeftColor {
			get { return _this._btmLeftColor; }
			set { _this._btmLeftColor = value; _this.setMeshColors(); }
		}

		public Color BtmRightColor {
			get { return _this._btmRightColor; }
			set { _this._btmRightColor = value; _this.setMeshColors(); }
		}	
	
		void Awake() {
			_this = this;	
			_camera = _this.GetComponent<Camera>();

			_topLeftColor = TopColor;
			_topRightColor = TopColor;
			_btmLeftColor = BottomColor;
			_btmRightColor = BottomColor;
		}
	
		void Start () {
			if (_camera == null) {
				throw new MemberAccessException("BackgroundPlane must be attached to a Camera object.");	
			}
		
			float farClip = _camera.farClipPlane - 0.01f;
		
			Vector3 
				topLeftPosition  = _camera.ViewportToWorldPoint(new Vector3(0, 1, farClip)),
				topRightPosition = _camera.ViewportToWorldPoint(new Vector3(1, 1, farClip)),
				btmLeftPosition  = _camera.ViewportToWorldPoint(new Vector3(0, 0, farClip)),
				btmRightPosition = _camera.ViewportToWorldPoint(new Vector3(1, 0, farClip));
		
			Vector3[] verts = new Vector3[] {
				topLeftPosition, topRightPosition, btmLeftPosition, btmRightPosition
			};
		
			int[] tris = new int[] {
				0, 1, 2, 2, 1, 3
			};
		
			_mesh = new Mesh();
			_mesh.vertices = verts;
			_mesh.triangles = tris;

			_background = new GameObject("_backgroundPlane");
			_background.transform.parent = transform;
			_background.AddComponent<MeshFilter>().mesh = _mesh;
			_background.AddComponent<MeshRenderer>().material = new Material(Shader);

			setMeshColors(); 
		}
	
		void setMeshColors() {
			_mesh.colors = new Color[] {
				_topLeftColor, _topRightColor, _btmLeftColor, _btmRightColor	
			};
		}
	
	}
}