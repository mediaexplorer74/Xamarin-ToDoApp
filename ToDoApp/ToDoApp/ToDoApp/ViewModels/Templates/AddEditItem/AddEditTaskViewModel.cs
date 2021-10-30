﻿using Prism.Navigation;
using Prism.Regions.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Auth;
using ToDoApp.Helpers;
using ToDoApp.Models;
using ToDoApp.Repositories.FirestoreRepository;
using Xamarin.Forms;

namespace ToDoApp.ViewModels.Templates.AddEditItem
{
    public class AddEditTaskViewModel : BaseRegionViewModel
    {
        #region Private & Protected

        private IFirestoreRepository<ListModel> _listRepository;
        private IFirestoreRepository<TaskModel> _taskRepository;

        #endregion

        #region Properties

        public ObservableCollection<ListModel> ProjectList { get; set; }
        public TaskModel AddTask { get; set; }
        public string Mode { get; set; }

        #endregion

        #region Commands

        public ICommand CreateCommand { get; set; }

        #endregion

        #region Constructors

        public AddEditTaskViewModel(
            INavigationService navigationService,
            IFirestoreRepository<TaskModel> taskRepository,
            IFirestoreRepository<ListModel> listRepository) : base(navigationService)
        {
            _taskRepository = taskRepository;
            _listRepository = listRepository;

            CreateCommand = new Command(CreateCommandHandler);
        }

        #endregion

        #region Command Handlers

        private async void CreateCommandHandler()
        {
            try
            {
                var auth = DependencyService.Get<IFirebaseAuthentication>();
                var userId = auth.GetUserId();

                if(Mode == "Edit")
                {
                    var model = new TaskModel()
                    {
                        archived = false,
                        list = AddTask.listObject.name,
                        task = AddTask.task,
                        userId = userId,
                        date = AddTask.dateObject.ToString("dd/MM/yyyy"),
                        id = AddTask.id
                    };
                    await _taskRepository.Update(model);
                }
                else
                {
                    var model = new TaskModel()
                    {
                        archived = false,
                        list = AddTask.listObject.name,
                        task = AddTask.task,
                        userId = userId,
                        date = AddTask.dateObject.ToString("dd/MM/yyyy")
                    };
                    await _taskRepository.Add(model);
                }

                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                //display error message
                Debug.Write(ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<ListModel>> GetProjectList()
        {
            var auth = DependencyService.Get<IFirebaseAuthentication>();
            var userId = auth.GetUserId();

            var querySnapshot = await _listRepository.GetAll(userId).GetAsync();
            var list = querySnapshot.ToObjects<ListModel>();
            var listToAdd = new List<ListModel>();
            if (list.Count() > 0)
            {
                listToAdd = list.ToList();
                listToAdd.Insert(0, Constants.InboxList);
            }
            else
            {
                listToAdd.Add(Constants.InboxList);
            }
            return listToAdd;
        }

        #endregion

        #region Region Navigation Handlers

        public override async void OnNavigatedTo(INavigationContext navigationContext)
        {
            var isEdit = navigationContext.Parameters.GetValue<bool>("isEdit");
            var task = navigationContext.Parameters.GetValue<TaskModel>("task");

            var projectList = await GetProjectList();
            ProjectList = new ObservableCollection<ListModel>(projectList);

            Mode = isEdit ? "Edit" : "Add";

            if (Mode == "Edit")
            {
                task.listObject = projectList.FirstOrDefault(pr => pr.name == task.list);
                AddTask = new TaskModel()
                {
                    task = task.task,
                    archived = task.archived,
                    dateObject = DateTime.ParseExact(task.date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    listObject = task.listObject,
                    id = task.id
                };
            }
            else
            {
                AddTask = new TaskModel()
                {
                    task = Constants.DefaultTask.task,
                    archived = Constants.DefaultTask.archived,
                    dateObject = Constants.DefaultTask.dateObject,
                    listObject = Constants.DefaultTask.listObject,
                };
            }
        }

        #endregion
    }
}