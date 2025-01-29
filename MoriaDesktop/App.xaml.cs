﻿using System.IO;
using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoriaBaseServices.Services;
using MoriaDesktop.Services;
using MoriaDesktop.ViewModels.Base;
using MoriaDesktop.ViewModels.Contacts;
using MoriaDesktop.Views.Base;
using MoriaDesktop.Views.Dictionary;
using MoriaDesktop.Views.Contacts;
using MoriaDesktopServices.Interfaces;
using MoriaDesktopServices.Interfaces.API;
using MoriaDesktopServices.Services;
using MoriaDesktopServices.Services.API;
using MoriaDesktop.Views.Dictionary.DetailView;
using MoriaDesktop.Views.DriveComponents;
using MoriaDesktop.ViewModels.DriveComponents;
using MoriaDesktop.Views.Products;
using MoriaDesktop.ViewModels.Products;
using MoriaDesktop.ViewModels.Dictionary.DetailView;
using MoriaDesktop.Views.Dictionary.ListView;
using MoriaDesktop.ViewModels.Dictionary.ListView;
using MoriaModelsDo.Models.Contacts;
using MoriaDesktop.Views.Dictionary.Window;
using MoriaModelsDo.Models.Dictionaries;

namespace MoriaDesktop;

public partial class App : Application
{
    public readonly IHost? AppHost;

    public App()
    {

        AppHost = Host.CreateDefaultBuilder()
                     .ConfigureServices((hostContext, services) =>
                     {
                         services.AddSingleton<MainWindow>();
                         services.AddSingleton<MainWindowViewModel>();
                         services.AddSingleton<AppStateService>();


                         services.AddScoped<LoginView>();
                         services.AddScoped<LoginViewModel>();
                         services.AddScoped<SecondView>();
                         services.AddScoped<SecondViewModel>();
                         services.AddScoped<WarehouseDetailView>();
                         services.AddScoped<WarehouseDetailViewModel>();
                         services.AddScoped<MotorDetailView>();
                         services.AddScoped<MotorDetailViewModel>();
                         services.AddScoped<MotorGearDetailView>();
                         services.AddScoped<MotorGearDetailViewModel>();
                         services.AddScoped<ColorDetailView>();
                         services.AddScoped<ColorDetailViewModel>();
                         services.AddScoped<EmployeeListView>();
                         services.AddScoped<EmployeeListViewModel>();
                         services.AddScoped<DriveDetailView>();
                         services.AddScoped<DriveDetailViewModel>();
                         services.AddScoped<ContactDetailView>();
                         services.AddScoped<ContactDetailViewModel>();
                         services.AddScoped<PositionDetailView>();
                         services.AddScoped<PositionDetailViewModel>();
                         services.AddScoped<SteelKindDetailView>();
                         services.AddScoped<SteelKindDetailViewModel>();

                         services.AddScoped<ColorListView>();
                         services.AddScoped<ColorListViewModel>();
                         services.AddScoped<ContactListView>();
                         services.AddScoped<ContactListViewModel>();
                         services.AddScoped<MotorGearListView>();
                         services.AddScoped<MotorGearListViewModel>();
                         services.AddScoped<MotorListView>();
                         services.AddScoped<MotorListViewModel>();
                         services.AddScoped<PositionListView>();
                         services.AddScoped<PositionListViewModel>();
                         services.AddScoped<SteelKindListView>();
                         services.AddScoped<SteelKindListViewModel>();
                         services.AddScoped<WarehouseListView>();
                         services.AddScoped<WarehouseListViewModel>();
                         services.AddScoped<ProductDetailView>();
                         services.AddScoped<ProductDetailViewModel>();
                         services.AddScoped<EmployeeDetailView>();
                         services.AddScoped<EmployeeDetailViewModel>();

                         services.AddScoped<LookupWindow>();
                         services.AddScoped<LookupWindowViewModel>();
                         services.AddScoped<INewObjectService, NewObjectService>();

                         services.AddScoped<IDetailedWindow, PositionWindowView>();
                         services.AddScoped<IDetailedWindow, ColorWindowView>();

                         services.AddScoped<IPageService, DesktopPageService>();
                         services.AddSingleton<INavigationService, NavigationService>();
                         services.AddScoped<ApiRequestService>();
                         services.AddScoped<IApiCredentialsService, MoriaApiCredentialsService>();
                         services.AddScoped<IApiService, MoriaApiService>();
                         services.AddScoped<IApiTokenService, ApiTokenService>();
                         services.AddScoped<ApiTestService>();
                         services.AddScoped<ITokensManager, TokensManager>();
                         services.AddScoped<IApiEmployeeService, ApiEmployeeService>();
                         services.AddScoped<IApiColorService, ApiColorService>();
                         services.AddScoped<IApiContactService, ApiContactService>();
                         services.AddScoped<IApiMotorGearService, ApiMotorGearService>();
                         services.AddScoped<IApiMotorService, ApiMotorService>();
                         services.AddScoped<IApiPositionService, ApiPositionService>();
                         services.AddScoped<IApiSteelKindService, ApiSteelKindService>();
                         services.AddScoped<IApiWarehouseService, ApiWarehouseService>();
                         services.AddScoped<ILookupService, LookupService>();

                         services.AddScoped<IApiLookupService, ApiLookupService>();
                         services.AddScoped<IApiLockService, ApiLockService>();
                         services.AddHttpClient();
                         services.AddHttpClient(ApiRequestService.HttpsApiClientName)
                         .ConfigurePrimaryHttpMessageHandler(c =>
                         {
                             var credentialsService = c.GetService<IApiCredentialsService>();
                             return new HttpClientHandler
                             {
                                 ServerCertificateCustomValidationCallback = (m, c, ch, e) =>
                                 {
                                     if (c.Thumbprint.ToLower() == credentialsService?.GetCertificateThumbprint())
                                         return true;
                                     else
                                         return false;
                                 }
                             };
                         });
                     })
                     .ConfigureAppConfiguration((context, config) =>
                     {
                         config.SetBasePath(Directory.GetCurrentDirectory());
                         config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                     })
                     .Build();

        DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    protected async override void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var wnd = AppHost.Services.GetRequiredService<MainWindow>();
        wnd.Closing += Wnd_Closing;
        var navigationService = AppHost.Services.GetRequiredService<INavigationService>();
        navigationService.SetFrame(wnd.NavigationFrame);

        var appStateService = AppHost.Services.GetRequiredService<AppStateService>();
        appStateService.SetMainViewModel(AppHost.Services.GetRequiredService<MainWindowViewModel>());

        wnd.Title = "MoriaDesktop";
        wnd.Resources.MergedDictionaries.Add(Application.Current.Resources);
        wnd.Show();
        base.OnStartup(e);
    }

    private async void Wnd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await UnlockObject();
    }

    private async void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        await UnlockObject();
    }

    async Task UnlockObject()
    {
        var appStateService = AppHost.Services.GetRequiredService<AppStateService>();
        if (appStateService != null && appStateService.CurrentDetailViewObjectType != null && appStateService.LoggedUser != null)
        {
            var lockService = AppHost.Services.GetRequiredService<IApiLockService>();

            try
            {
                appStateService.SetupLoading(true);
                await lockService.Unlock(appStateService.LoggedUser.Username, appStateService.CurrentDetailViewObjectType, appStateService.CurrentDetailViewObjectId);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                appStateService.SetupLoading();
            }
        }
    }
}