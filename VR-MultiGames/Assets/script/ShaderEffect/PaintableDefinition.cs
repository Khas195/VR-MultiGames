using UnityEngine;

namespace Assets.script
{
	public class PaintableDefinition : MonoBehaviour
    {
		public static string  DrawOnHeigtMap = "_ParallaxMap";

		public static string DrawOnNormalMap = "_PaintNormal";

		public static string ColorProperty = "_Color";

		public static string BulletShader = "Custom/Bullet";

		public const string PaintableShaderName = "Custom/PaintOn";
        
		public const string DrawOnTextureName = "_DrawOnTex";
        
        public const string MainTexture = "_MainTex";
    }
}