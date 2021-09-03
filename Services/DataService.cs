using System;
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
        private DateTime createDate;

        public string GetData()
        {
            var difffDate = (DateTime.Now - createDate).TotalMinutes;
            if(difffDate >= 10) {
                data = "";
                return "";
            }

            return data;
        }

        public void SetData(string d)
        {
            data = d;
            createDate = DateTime.Now;
        }
    }
}