namespace MidlandFarm.Scripts.Cuttables
{
    public interface ICutter
    {
        void Cutted(ICuttable cuttable);
        void SetTargetCuttable(ICuttable cuttable);
    }
}