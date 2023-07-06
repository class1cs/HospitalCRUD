using HospitalBaseApp.DB;
using HospitalBaseApp.Services;
using HospitalBaseApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalBaseApp
{
    public static class Ioc
    {
        private static IServiceProvider _provider;

        static Ioc()
        {
            var services = new ServiceCollection();

            services.AddTransient<AddOperationRecordViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<MessageBus>();
            services.AddTransient<EditOperationViewModel>();
            services.AddTransient<AddOperationViewModel>();
            services.AddScoped<LoginViewModel>();
            services.AddTransient<SurgeonViewModel>();
            services.AddTransient<GeneralDoctorViewModel>();
            services.AddTransient<EditOperationRecordViewModel>();
            services.AddTransient<AddPatientViewModel>();
            services.AddTransient<EditPatientViewModel>();
            services.AddTransient<AddDoctorViewModel>();
            services.AddTransient<DoctorViewModel>();
            services.AddDbContext<DatabaseContext>();
            services.AddSingleton<PageService>();
            services.AddTransient<EditDoctorViewModel>();
            services.AddTransient<AddAppointmentViewModel>();
            services.AddTransient<EditAppointmentViewModel>();
            _provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _provider.GetRequiredService<T>();
    }

    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => Ioc.Resolve<MainViewModel>();

        public AddOperationViewModel AddOperationToBaseViewModel => Ioc.Resolve<AddOperationViewModel>();

        public EditOperationViewModel EditOperationViewModel => Ioc.Resolve<EditOperationViewModel>();

        public AddOperationRecordViewModel AddOperationViewModel => Ioc.Resolve<AddOperationRecordViewModel>();

        public EditPatientViewModel EditPatientViewModel => Ioc.Resolve<EditPatientViewModel>();

        public EditOperationRecordViewModel EditOperationRecordViewModel => Ioc.Resolve<EditOperationRecordViewModel>();

        public AddPatientViewModel AddPatientViewModel => Ioc.Resolve<AddPatientViewModel>();

        public LoginViewModel LoginViewModel => Ioc.Resolve<LoginViewModel>();

        public SurgeonViewModel SurgeonViewModel => Ioc.Resolve<SurgeonViewModel>();

        public AddDoctorViewModel AddDoctorViewModel => Ioc.Resolve<AddDoctorViewModel>();

        public GeneralDoctorViewModel GeneralDoctorViewModel => Ioc.Resolve<GeneralDoctorViewModel>();

        public EditDoctorViewModel EditDoctorViewModel => Ioc.Resolve<EditDoctorViewModel>();

        public DoctorViewModel DoctorViewModel => Ioc.Resolve<DoctorViewModel>();

        public AddAppointmentViewModel AddAppointmentViewModel => Ioc.Resolve<AddAppointmentViewModel>();

        public EditAppointmentViewModel EditAppointmentViewModel => Ioc.Resolve<EditAppointmentViewModel>();
    }

}
