namespace otcalculate.Services
{
    public interface IDataService
    {
        string GetData();
        void SetData(string d);
    }
    public class DataService: IDataService
    {
        private string data = "";

        public string GetData()
        {
            return data;
        }

        public void SetData(string d)
        {
            data = d;
        }
    }
}