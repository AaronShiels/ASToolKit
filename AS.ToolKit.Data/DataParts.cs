using System.Reflection;

namespace AS.ToolKit.Data
{
    public static class DataParts
    {
        public static Assembly Assembly
        {
            get { return (typeof (DataParts)).Assembly; }
        }
    }
}