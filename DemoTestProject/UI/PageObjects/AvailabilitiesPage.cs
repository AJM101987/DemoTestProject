namespace DemoTestProject.UI.PageObjects
{
    public class AvailabilitiesPage
    {
        public string AvailabilityValue => "[controlname=availabilityValue]";

        public string EmbeddedBenefits => "[controlname=embeddedBenefits]";

        public string VariableOperatingAndMaintenance => "[controlname=variableOperatingAndMaintenance]";

        public string VariableGasTransportationCharges => "[controlname=variableGasTransportationCharges]";

        public string MinimumProfitMargin => "[controlname=minimumProfitMargin]";

        public string VariableGtEfficiency => "[controlname=variableGtEfficiency]";

        public string SaveButton => "Save";

        public string RefreshButton => "Refresh Data";

        public string CopyDownArrow => "Copy down value";
    }
}