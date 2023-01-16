namespace EmployeeManagement.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

    public static class Employees
    {
        public static string GetList => $"{Base}/employees";
        public static string GetRecord(Guid id) => $"{Base}/employees/{id}";
        public static string Delete(Guid id) => $"{Base}/employees/{id}";
        public static string Put(Guid id) => $"{Base}/employees/{id}";
        public static string Create => $"{Base}/employees";
        public static string CreateBatch => $"{Base}/employees/batch";
    }

    public static class Items
    {
        public static string GetList => $"{Base}/items";
        public static string GetRecord(Guid id) => $"{Base}/items/{id}";
        public static string Delete(Guid id) => $"{Base}/items/{id}";
        public static string Put(Guid id) => $"{Base}/items/{id}";
        public static string Create => $"{Base}/items";
        public static string CreateBatch => $"{Base}/items/batch";
    }

    public static class Clients
    {
        public static string GetList => $"{Base}/clients";
        public static string GetRecord(Guid id) => $"{Base}/clients/{id}";
        public static string Delete(Guid id) => $"{Base}/clients/{id}";
        public static string Put(Guid id) => $"{Base}/clients/{id}";
        public static string Create => $"{Base}/clients";
        public static string CreateBatch => $"{Base}/clients/batch";
    }

    public static class Projects
    {
        public static string GetList => $"{Base}/projects";
        public static string GetRecord(Guid id) => $"{Base}/projects/{id}";
        public static string Delete(Guid id) => $"{Base}/projects/{id}";
        public static string Put(Guid id) => $"{Base}/projects/{id}";
        public static string Create => $"{Base}/projects";
        public static string CreateBatch => $"{Base}/projects/batch";
    }

    public static class UserProfiles
    {
        public static string GetList => $"{Base}/userProfiles";
        public static string GetRecord(Guid id) => $"{Base}/userProfiles/{id}";
        public static string Delete(Guid id) => $"{Base}/userProfiles/{id}";
        public static string Put(Guid id) => $"{Base}/userProfiles/{id}";
        public static string Create => $"{Base}/userProfiles";
        public static string CreateBatch => $"{Base}/userProfiles/batch";
    }

    public static class Users
    {
        public static string GetList => $"{Base}/users";
        public static string GetRecord(Guid id) => $"{Base}/users/{id}";
        public static string Delete(Guid id) => $"{Base}/users/{id}";
        public static string Put(Guid id) => $"{Base}/users/{id}";
        public static string Create => $"{Base}/users";
        public static string CreateBatch => $"{Base}/users/batch";
    }
}
