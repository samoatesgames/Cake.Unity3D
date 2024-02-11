namespace Cake.Unity3D.Test;

/// <summary>
/// Code coverage options available when performing a Unity3D build.
/// </summary>
public class Unity3DCodeCoverageOptions 
{
    /// <summary>
    /// (optional), to set the location where the coverage results and report are saved to.
    /// The default location is the project's path.
    /// </summary>
    public string CodeCoverageResultsLocation { get; set; }
        
    /// <summary>
    /// (optional) Set the location where the coverage report history is saved to.
    /// The default location is the project's path
    /// </summary>
    public string CodeCoverageResultsHistoryLocation { get; set; }
        
    /// <summary>
    /// Generate a HTML report.
    /// </summary>
    public bool GenerateHtmlReport { get; set; }
        
    /// <summary>
    /// Generate a HTML report history.
    /// </summary>
    public bool GenerateHtmlReportHistory { get; set; }
        
    /// <summary>
    /// Generate and include additional metrics in the HTML report.
    /// </summary>
    public bool GenerateAdditionalMetrics { get; set; }
        
    /// <summary>
    /// Generage coverage summary badges.
    /// </summary>
    public bool GenerateBadgeReport { get; set; }
        
    /// <summary>
    /// Generate SonarQube, Cobertura and LCOV reports.
    /// </summary>
    public bool GenerateAdditionalReports { get; set; }
        
    /// <summary>
    /// include test references to the generated coverage results
    /// and enable the Coverage by test methods section in the HTML report.
    /// This shows how each test contributes to the overall coverage. 
    /// </summary>
    public bool GenerateTestReferences { get; set; } 
        
    /// <summary>
    /// Use the project settings for the code coverage options.
    /// </summary>
    public bool UseProjectSettings { get; set; }
}