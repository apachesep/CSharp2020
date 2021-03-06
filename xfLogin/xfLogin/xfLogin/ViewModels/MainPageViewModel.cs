﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xfLogin.ViewModels
{
    using System.ComponentModel;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Xamarin.Forms.Internals;
    using xfLogin.Models;

    public class MainPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        public string Account { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public DelegateCommand LoginCommand { get; set; }
       string dataFile= System.IO.Path.Combine(
           System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Account.txt");

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            LoginCommand = new DelegateCommand(async () =>
            {
                Message = "";
                string url = "https://contososyncfusion.azurewebsites.net/api/Login";
                HttpClient client = new HttpClient();
                LoginQueryString loginQueryString = new LoginQueryString()
                {
                    Account = Account,
                    Password = Password,
                };
                string postPayload = JsonConvert.SerializeObject(loginQueryString);
                HttpResponseMessage response = await client
                .PostAsync(url, new StringContent(postPayload, Encoding.UTF8, "application/json"));


                if (response.IsSuccessStatusCode == false)
                {
                    Message = "登入驗證發生錯誤";
                }
                else
                {
                    String strResult = await response.Content.ReadAsStringAsync();
                    StandardResponse<LoginData> standardResponse =
                    JsonConvert.DeserializeObject<StandardResponse<LoginData>>
                    (strResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                    if (standardResponse.Success == true)
                    {
                        Message = "登入驗證程序成功";
                        File.WriteAllText(dataFile, postPayload);
                    }
                    else
                    {
                        Message = standardResponse.ErrorMessage;
                    }
                }
            });

#if DEBUG
            //Account = "admin";
            //Password = "123";
#endif
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                var content =File.ReadAllText(dataFile);
                if(string.IsNullOrEmpty(content) ==false)
                {
                    LoginQueryString loginQueryString = 
                        JsonConvert.DeserializeObject<LoginQueryString>(content);
                    Account = loginQueryString.Account;
                    Password = loginQueryString.Password;
                }
            }
            catch { }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
