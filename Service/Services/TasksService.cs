﻿using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TasksService : ITasksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskValidationService _taskValidationService;

        public TasksService(IUnitOfWork unitOfWork,ITaskValidationService taskValidationService)
        {
            _unitOfWork = unitOfWork;
            _taskValidationService = taskValidationService;
        }

        public async Task AddTask(Tasks task)
        {
            if(task.TaskType == TaskType.SubTask)
            {
                var baseTaskExists = await _taskValidationService.ValidateTaskExistence(task.BaseTaskId.Value);

                if(!baseTaskExists)
                {
                    return;
                }
            }

            await _unitOfWork.TasksRepository.Add(task);
            await _unitOfWork.Save();
        }

        public async Task DeleteTask(Tasks task)
        {
            if(task.TaskType == TaskType.Task)
            {
                var subTasks = await _unitOfWork.TasksRepository.Get(x=>x.BaseTaskId == task.Id);

                if (subTasks.Any())
                {
                    foreach (var subTask in subTasks)
                    {
                        _unitOfWork.TasksRepository.Delete(subTask);
                    }
                }
            }

            _unitOfWork.TasksRepository.Delete(task);
            await _unitOfWork.Save();
        }

        public async Task UpdateTask(Tasks task)
        {
            _unitOfWork.TasksRepository.Update(task);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Tasks>> GetTasksForUser(string userId, Specification<Tasks> specification)
        {
            var tasks = (await _unitOfWork.TasksRepository.Find(specification)).Where(x=>x.UserId == userId);
            return tasks;
        }

        public async Task<Tasks> GetTaskById(string userId,int taskId)
        {
            var task = (await _unitOfWork.TasksRepository.Get(x=>x.UserId == userId && x.Id == taskId)).FirstOrDefault();

            if(task?.TaskType == TaskType.Task)
            {
                task.SubTasks = (await _unitOfWork.TasksRepository.GetSubTasksForTask(taskId)).ToList();
            }

            return task;
        }
    }
}
