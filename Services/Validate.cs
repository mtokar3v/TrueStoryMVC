using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Services
{
    public class ValidateResult 
    {
        public bool IsValid { get; set; }
        public List<string> ErrorList { get; set; } = new List<string>();
    }

    public interface IValidate
    {
        public bool isValid();
        public string ErrorMessage { get; }
    }

    public class TextValidate : IValidate
    {
        IEnumerable<string> _texts;
        public TextValidate(IEnumerable<string> texts)
        {
            _texts = texts;
        }

        public string ErrorMessage{ get; private set; }

        public bool isValid()
        {
            bool result = false;
            foreach (var t in _texts)
                result |= !string.IsNullOrEmpty(t);

            if (result == false)
                ErrorMessage = "Пост должен иметь хотя бы один блок: текст, изображение"; 

            return result;
        }
    }

    public class ImageValidate : IValidate
    {
        List<IEnumerable<byte>> _images;
        public ImageValidate(List<IEnumerable<byte>> images)
        {
            _images = images;
        }
        public string ErrorMessage { get; private set; }

        public bool isValid()
        {
            bool result = false;
            foreach (var i in _images)
                result |= i.Count() > 0;

            if(result == false)
                ErrorMessage = "Пост должен иметь хотя бы один блок: текст, изображение";
            return result;
        }
    }

    public class BlankHeaderValidate : IValidate
    {
        string _text;
        public BlankHeaderValidate(string text)
        {
            _text = text;
        }

        public string ErrorMessage { get; private set; }

        public bool isValid()
        {
            bool result = !string.IsNullOrEmpty(_text);

            if (result == false)
                ErrorMessage = "Введите название заголовка";

            return result;
        }
    }

    public class BlankTagValidate : IValidate
    {
        string _text;
        public BlankTagValidate(string text)
        {
            _text = text;
        }

        public string ErrorMessage { get; private set; }

        public bool isValid()
        {
            bool result = !string.IsNullOrEmpty(_text);

            if (result == false)
                ErrorMessage = "Введите теги";

            return result;
        }
    }
}
