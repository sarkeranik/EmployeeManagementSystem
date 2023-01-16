namespace EmployeeManagement.UnitTests.UnitTests.Domain.Employees.Features;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Domain.Employees.Mappings;
using EmployeeManagement.Domain.Employees.Features;
using EmployeeManagement.Domain.Employees.Services;
using MapsterMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetEmployeeListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IEmployeeRepository> _employeeRepository;

    public GetEmployeeListTests()
    {
        _employeeRepository = new Mock<IEmployeeRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
    }
    
    [Test]
    public async Task can_get_paged_list_of_employee()
    {
        //Arrange
        var fakeEmployeeOne = FakeEmployee.Generate();
        var fakeEmployeeTwo = FakeEmployee.Generate();
        var fakeEmployeeThree = FakeEmployee.Generate();
        var employee = new List<Employee>();
        employee.Add(fakeEmployeeOne);
        employee.Add(fakeEmployeeTwo);
        employee.Add(fakeEmployeeThree);
        var mockDbData = employee.AsQueryable().BuildMock();
        
        var queryParameters = new EmployeeParametersDto() { PageSize = 1, PageNumber = 2 };

        _employeeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetEmployeeList.Query(queryParameters);
        var handler = new GetEmployeeList.Handler(_employeeRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_employee_list_using_Name()
    {
        //Arrange
        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Name, _ => "alpha")
            .Generate());
        var fakeEmployeeTwo = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Name, _ => "bravo")
            .Generate());
        var queryParameters = new EmployeeParametersDto() { Filters = $"Name == {fakeEmployeeTwo.Name}" };

        var employeeList = new List<Employee>() { fakeEmployeeOne, fakeEmployeeTwo };
        var mockDbData = employeeList.AsQueryable().BuildMock();

        _employeeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetEmployeeList.Query(queryParameters);
        var handler = new GetEmployeeList.Handler(_employeeRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeEmployeeTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_employee_list_using_Salary()
    {
        //Arrange
        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Salary, _ => 1)
            .Generate());
        var fakeEmployeeTwo = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Salary, _ => 2)
            .Generate());
        var queryParameters = new EmployeeParametersDto() { Filters = $"Salary == {fakeEmployeeTwo.Salary}" };

        var employeeList = new List<Employee>() { fakeEmployeeOne, fakeEmployeeTwo };
        var mockDbData = employeeList.AsQueryable().BuildMock();

        _employeeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetEmployeeList.Query(queryParameters);
        var handler = new GetEmployeeList.Handler(_employeeRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeEmployeeTwo, options =>
                options.ExcludingMissingMembers());
    }



    [Test]
    public async Task can_get_sorted_list_of_employee_by_Name()
    {
        //Arrange
        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Name, _ => "alpha")
            .Generate());
        var fakeEmployeeTwo = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Name, _ => "bravo")
            .Generate());
        var queryParameters = new EmployeeParametersDto() { SortOrder = "-Name" };

        var EmployeeList = new List<Employee>() { fakeEmployeeOne, fakeEmployeeTwo };
        var mockDbData = EmployeeList.AsQueryable().BuildMock();

        _employeeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetEmployeeList.Query(queryParameters);
        var handler = new GetEmployeeList.Handler(_employeeRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeEmployeeTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeEmployeeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_employee_by_Salary()
    {
        //Arrange
        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Salary, _ => 1)
            .Generate());
        var fakeEmployeeTwo = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.Salary, _ => 2)
            .Generate());
        var queryParameters = new EmployeeParametersDto() { SortOrder = "-Salary" };

        var EmployeeList = new List<Employee>() { fakeEmployeeOne, fakeEmployeeTwo };
        var mockDbData = EmployeeList.AsQueryable().BuildMock();

        _employeeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetEmployeeList.Query(queryParameters);
        var handler = new GetEmployeeList.Handler(_employeeRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeEmployeeTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeEmployeeOne, options =>
                options.ExcludingMissingMembers());
    }


}