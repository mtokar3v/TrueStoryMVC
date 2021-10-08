using System.Collections.Generic;
using System.Linq;
using TrueStoryMVC.Services;

namespace TrueStoryMVC.Models.ViewModels
{
    public class PostModel
    {
        public string Header { get; set; }
        public List<string> Texts { get; set; }
        public List<IEnumerable<byte>> Images { get; set; }
        public string TagsLine { get; set; }
        public string Scheme { get; set; }
        public ValidateResult Validate()
        {
            ValidateResult result = new ValidateResult();

            List<IValidate> mandatoryValidates = new List<IValidate>();
            mandatoryValidates.Add(new BlankHeaderValidate(Header));
            mandatoryValidates.Add(new BlankTagValidate(TagsLine));
            mandatoryValidates.Add(new SchemeValidate(Images.Count() + Texts.Count(), Scheme));

            List<IValidate> optionalValidates = new List<IValidate>();
            optionalValidates.Add(new ImageValidate(Images));
            optionalValidates.Add(new TextValidate(Texts));

            bool flag = false;

            bool mandatoryResult = true;
            foreach (var v in mandatoryValidates)
            {
                flag = v.isValid();
                if (flag == false)
                    result.ErrorList.Add(v.ErrorMessage);

                mandatoryResult &= flag;
            }

            bool optionalResult = false;

            foreach (var v in optionalValidates)
            {
                flag = v.isValid();

                if (flag == false)
                    if (result.ErrorList.Count != 0 && !result.ErrorList[result.ErrorList.Count - 1].Equals(v.ErrorMessage))
                        result.ErrorList.Add(v.ErrorMessage);
                optionalResult |= flag;
            }

            result.IsValid = optionalResult && mandatoryResult;

            return result;
        }
    }
}
