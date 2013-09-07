using System;

namespace AS.ToolKit.Web.ViewModels
{
    public class AlertMessage
    {
        public AlertMessage(string heading, string text, Type type)
        {
            Heading = heading;
            Text = text;

            switch (type)
            {
                case Type.Hidden:
                    TypeCss = "hide";
                    break;
                case Type.Error:
                    TypeCss = "alert-danger";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public string Heading { get; set; }
        public string Text { get; set; }
        public string TypeCss { get; set; }

        public enum Type
        {
            Hidden = 1,
            Error = 2
        }
    }
}