﻿using Domain.Interfaces;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TaskValidationService: ITaskValidationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ValidateTaskExistence(int taskId)
        {
            var task = await _unitOfWork.TasksRepository.GetById(taskId);
            if(task != null)
            {
                return true;
            }    
            return false;
        }
    }
}
