namespace GameFramework.Data
{
    public abstract class Data:IData
    {
        public abstract void OnPreload();

        public abstract void OnLoad();
    }
}