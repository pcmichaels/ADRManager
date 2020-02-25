namespace ADR.Rules
{
    public class RulesAnalyser : IRulesAnalyser
    {
        public bool IsProjectItemNameValid(string projectItemName) =>
            projectItemName.EndsWith("md")
            && projectItemName != "readme.md";
    }
}
