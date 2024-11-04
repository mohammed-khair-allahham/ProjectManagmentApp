using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using ProjectManagmentApp.Application.Common.Interfaces;
using NUnit.Framework;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Application.Projects.Queries.GetProjectList;
using ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskList;
using ProjectManagmentApp.Application.Projects.Commands;
using ProjectManagmentApp.Application.ProjectTasks.Commands;

namespace ProjectManagmentApp.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    [TestCase(typeof(Project), typeof(GetProjectListDto))]
    [TestCase(typeof(ProjectTask), typeof(GetTaskListDto))]
    [TestCase(typeof(Project), typeof(ProjectResponseDto))]
    [TestCase(typeof(ProjectTask), typeof(TaskResponseDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
