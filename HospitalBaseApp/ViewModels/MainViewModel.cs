using DevExpress.Mvvm;
using HospitalBaseApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Windows.Input;
using HospitalBaseApp.Views;
using System.Data;
using HospitalBaseApp.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HospitalBaseApp.Services;
using HospitalBaseApp.Extensions;
using System.Windows.Controls;
using HospitalBaseApp.Pages;

namespace HospitalBaseApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        

        private readonly MessageBus _messageBus;
        private readonly PageService _pageService;


        public Page CurrentPage { get; set; }

        public MainViewModel(PageService pageService)
        {
            _pageService = pageService;


            _pageService.OnPageChanged += (page) => CurrentPage = page;
            _pageService.ChangePage(new LoginPage());
          
        }

        public ICommand GoToBack => new DelegateCommand(async () =>
        {

            _pageService.GoToBack();
        }, () => _pageService.CanGoToBack);






    }
}


   


