using System.Collections.Generic;

namespace Dfe.Wizard.Prototype.Models.Rules
{
    public interface IValidatable
    {
        bool IsValid();
        List<string> Validate(string answer);
    }
}
