using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Cafe.Application.Commands.Cafe;
using Cafe.Application.Services;
using Cafe.Infrastructure.Repositories.Commands;
using Cafe.Infrastructure.Repositories.Queries;
using Cafe.Infrastructure.UnitOfWork;
using FluentValidation;
using MediatR;
using Cafe.Application.Behaviors;


namespace Cafe.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(CreateCafeCommand).Assembly;

            // Register MediatR
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerLifetimeScope();

            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // Register AutoMapper
            builder.RegisterAutoMapper(assembly);

            // Register repositories
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeQueryRepository>().As<IEmployeeQueryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CafeQueryRepository>().As<ICafeQueryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CafeEmployeeQueryRepository>().As<ICafeEmployeeQueryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeCommandRepository>().As<IEmployeeCommandRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CafeCommandRepository>().As<ICafeCommandRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CafeEmployeeCommandRepository>().As<ICafeEmployeeCommandRepository>().InstancePerLifetimeScope();

            // Register services
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().InstancePerLifetimeScope();
            builder.RegisterType<CafeService>().As<ICafeService>().InstancePerLifetimeScope();

            // Register Validators
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
        .As(typeof(IPipelineBehavior<,>))
        .InstancePerLifetimeScope();
        }
    }
}