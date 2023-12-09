using UnityEngine;

public class TeslaCoils : SimpleMechanism
{
        [SerializeField] private SpriteRenderer[] teslaCoilRenderers;
        [SerializeField] private GameObject electricitySpline;
        private static readonly int Enabled = Shader.PropertyToID("_Enabled");
        
        public new void Activate()
        {
                Set(true);
                base.Activate();
        }
        public new void Deactivate()
        {
                Set(false);
                base.Deactivate();
        }

        private void Set(bool coilsEnabled)
        {
                foreach (SpriteRenderer teslaCoilRenderer in teslaCoilRenderers) teslaCoilRenderer.material.SetFloat(Enabled, coilsEnabled ? 1 : 0);
                electricitySpline.SetActive(coilsEnabled);
        }
}