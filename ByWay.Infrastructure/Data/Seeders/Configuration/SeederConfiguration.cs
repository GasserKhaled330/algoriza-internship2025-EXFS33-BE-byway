namespace ByWay.Infrastructure.Data.Seeders.Configuration;

public static class SeederConfiguration
{
    public const string BaseDirectory = @"../ByWay.Infrastructure/Data/InitialData";

    public static class FileNames
    {
        public const string Categories = @"categories.json";
        public const string Instructors = @"instructors.json";
        public const string Courses = @"courses.json";
        public const string CourseContents = @"course_contents.json";
        public const string Users = @"users.json";
    }
}