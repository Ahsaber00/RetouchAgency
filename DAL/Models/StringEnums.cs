namespace DAL.Models;

public static class UserRole
{
    public const string Admin = "admin";
    public const string Applicant = "applicant";
}

public static class UserAuthMethod
{
    public const string Local = "local";
    public const string Google = "google";
}

public static class OpportunityStatus
{
    public const string Open = "opened";
    public const string Closed = "closed";
    public const string Scheduled = "scheduled";
}

public static class OpportunityType
{
    public const string Internship = "internship";
    public const string FullTime = "job";
}

public static class ApplicationStatus
{
    public const string Submitted = "submitted";
    public const string Reviewed = "reviewed";
    public const string Hired = "hired";
    // public const string Interviewed = "interviewed";
    // public const string Offered = "offered";
    // public const string Rejected = "rejected";
}