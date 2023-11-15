namespace Svendeproeve.SQLRepo
{
    public interface ISQLProvider
    {
        public dynamic Create(string query);
        public dynamic Read(string query);
        public dynamic Update(string query);
        public dynamic Delete(string query);
    }
}
