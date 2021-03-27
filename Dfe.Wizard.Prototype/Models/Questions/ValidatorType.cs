namespace Dfe.Wizard.Prototype.Models.Questions
{
    public enum ValidatorType
    {
        None,
        Regex,
        DateTimeHistorical,
        DateTimeFuture,
        AlphabeticalValues,
        AlphabeticalIncludingSpecialChars,
        Number,
        NumberHigher,
        NumberLower,
        NumberHigherOrEqual,
        NumberLowerOrEqual,
        LAESTABNumber,
        MaxCharacters
    }
}